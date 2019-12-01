using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct PixfmtAlphaBlend32<TRenderingBuffer, TOrderColor, TBlenderColor> : IPixfmt<Color8>, IPixelDataSrcAttacher<TRenderingBuffer>, IPixelDataAccessorAttacher, IPixfmtAlphaBlend<Color8>
		where TRenderingBuffer : unmanaged, IRenderingBuffer
		where TOrderColor : unmanaged, IOrderColor32
		where TBlenderColor : unmanaged, IBlenderColor32<TOrderColor>
	{
		private static TBlenderColor _blender = default;
		private static readonly TOrderColor Order = default;

		private TRenderingBuffer* _buffer;

		public PixfmtAlphaBlend32(ref TRenderingBuffer buffer)
		{
			_buffer = (TRenderingBuffer*)Unsafe.AsPointer(ref buffer);
		}

		public readonly int BytesPerPixel
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => 4;
		}

		public readonly int Width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Width;
		}

		public readonly int Height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Height;
		}

		public readonly int Stride
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Stride;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int y) => _buffer->GetRowPtr(y);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int x, int y, int length) => _buffer->GetRowPtr(x, y, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetPixPtr(int x, int y) => _buffer->GetRowPtr(y) + (uint)(x << 2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly RowInfo GetRowInfo(int y) => _buffer->GetRowInfo(y);

		public void Attach(ref TRenderingBuffer buffer) => _buffer = (TRenderingBuffer*)Unsafe.AsPointer(ref buffer);

		public readonly bool Attach<TPixelDataAccessor, TColor>(ref TPixelDataAccessor pixfmt, int x1, int y1, int x2, int y2)
			where TPixelDataAccessor : unmanaged, IPixfmt<TColor>
			where TColor : unmanaged, IColor
		{
			var r1 = new Rectangle32(x1, y1, x2, y2);
			var r2 = new Rectangle32(0, 0, pixfmt.Width - 1, pixfmt.Height - 1);
			if (!r1.Clip(ref r2))
			{
				return false;
			}
			var stride = pixfmt.Stride;
			_buffer->Attach(pixfmt.GetPixPtr(r1.X1, stride < 0 ? r1.Y2 : r1.Y1), (r1.X2 - r1.X1) + 1, (r1.Y2 - r1.Y1) + 1, stride);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly Color8 Pixel(int x, int y)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x << 2);
			return new Color8(p[Order.R], p[Order.G], p[Order.B], p[Order.A]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyPixel(int x, int y, Color8 color)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x << 2);
			p[Order.R] = color.R;
			p[Order.G] = color.G;
			p[Order.B] = color.B;
			p[Order.A] = color.A;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendPixel(int x, int y, Color8 color, byte cover)
		{
			var p = _buffer->GetRowPtr(x, y, 1) + (uint)(x << 2);
			CopyOrBlendPix(p, color, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyHLine(int x, int y, int length, Color8 color)
		{
			var p = _buffer->GetRowPtr(x, y, length) + (uint)(x << 2);

			do
			{
				p[Order.R] = color.R;
				p[Order.G] = color.G;
				p[Order.B] = color.B;
				p[Order.A] = color.A;
				p += BytesPerPixel;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyVLine(int x, int y, int length, Color8 color)
		{
			do
			{
				var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);

				p[Order.R] = color.R;
				p[Order.G] = color.G;
				p[Order.B] = color.B;
				p[Order.A] = color.A;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendHLine(int x, int y, int length, Color8 color, byte cover)
		{
			// return;

			if (color.A == 0)
			{
				return;
			}

			var p = _buffer->GetRowPtr(x, y, length) + (uint)(x << 2);

			if(length >= 4)
			{
				_blender.BlendPixel4(ref p, ref length, color, cover);
			}
			if(length == 0) return;

			var alpha = (color.A * (cover + 1)) >> 8;
			color.A = (byte)alpha;
			if (alpha == Color8.BaseMask)
			{
				// var uc = (color.R << (Order.R << 3)) | (color.G << (Order.G << 3)) | (color.B << (Order.B << 3)) | (Color8.BaseMask << (Order.A << 3));
				do
				{
					// *((int*) p) = uc;
					p[Order.R] = color.R;
					p[Order.G] = color.G;
					p[Order.B] = color.B;
					p[Order.A] = Color8.BaseMask;
					p += BytesPerPixel;
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						_blender.BlendPixel(p, color.R, color.G, color.B, alpha);
						p += BytesPerPixel;
					} while (--length != 0);
				}
				else
				{
					do
					{
						_blender.BlendPixel(p, color.R, color.G, color.B, alpha, cover);
						p += BytesPerPixel;
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendVLine(int x, int y, int length, Color8 color, byte cover)
		{
			if (color.A == 0)
			{
				return;
			}

			var alpha = (color.A * (cover + 1)) >> 8;
			if (alpha == Color8.BaseMask)
			{
				do
				{
					var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);

					p[Order.R] = color.R;
					p[Order.G] = color.G;
					p[Order.B] = color.B;
					p[Order.A] = Color8.BaseMask;
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);
						_blender.BlendPixel(p, color.R, color.G, color.B, alpha);
					} while (--length != 0);
				}
				else
				{
					do
					{
						var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);
						_blender.BlendPixel(p, color.R, color.G, color.B, alpha, cover);
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidHSpan(int x, int y, int length, Color8 color, byte* covers)
		{
			if (color.A == 0)
			{
				return;
			}
			var p = _buffer->GetRowPtr(x, y, length) + (uint)(x << 2);

			if(length >= 4)
			{
				_blender.BlendPixel4(ref p, ref length, color, ref covers);
			}
			if(length == 0) return;

			do
			{
				var alpha = (color.A * (*covers + 1)) >> 8;
				if (alpha == Color8.BaseMask)
				{
					// var uc = (color.R << (Order.R << 3)) | (color.G << (Order.G << 3)) | (color.B << (Order.B << 3)) | (Color8.BaseMask << (Order.A << 3));
					// *((int*)p) = uc;
					p[Order.R] = color.R;
					p[Order.G] = color.G;
					p[Order.B] = color.B;
					p[Order.A] = Color8.BaseMask;
				}
				else
				{
					_blender.BlendPixel(p, color.R, color.G, color.B, alpha, *covers);
				}
				p += BytesPerPixel;
				covers++;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidVSpan(int x, int y, int length, Color8 color, byte* covers)
		{
			if (color.A == 0)
			{
				return;
			}

			do
			{
				var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);
				var alpha = (color.A * (*covers + 1)) >> 8;
				if (alpha == Color8.BaseMask)
				{
					p[Order.R] = color.R;
					p[Order.G] = color.G;
					p[Order.B] = color.B;
					p[Order.A] = Color8.BaseMask;
				}
				else
				{
					_blender.BlendPixel(p, color.R, color.G, color.B, alpha, *covers);
				}

				covers++;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorHSpan(int x, int y, int length, Color8* colors)
		{
			var p = _buffer->GetRowPtr(x, y, length) + (uint)(x << 2);
			do
			{
				p[Order.R] = colors->R;
				p[Order.G] = colors->G;
				p[Order.B] = colors->B;
				p[Order.A] = colors->A;
				p += BytesPerPixel;
				colors++;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorVSpan(int x, int y, int length, Color8* colors)
		{
			do
			{
				var p = _buffer->GetRowPtr(x, y++, 1) + (uint)(x << 2);
				p[Order.R] = colors->R;
				p[Order.G] = colors->G;
				p[Order.B] = colors->B;
				p[Order.A] = colors->A;
				colors++;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorHSpan(int x, int y, int length, Color8* colors, byte* covers, byte cover)
		{
			var p = _buffer->GetRowPtr(x, y, length) + (uint)(x << 2);
			if (covers != null)
			{
				do
				{
					CopyOrBlendPix(p, *colors++, *covers++);
					p += BytesPerPixel;
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						CopyOrBlendPix(p, *colors++);
						p += BytesPerPixel;
					} while (--length != 0);
				}
				else
				{
					do
					{
						CopyOrBlendPix(p, *colors++, cover);
						p += BytesPerPixel;
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorVSpan(int x, int y, int length, Color8* colors, byte* covers, byte cover)
		{
			if (covers != null)
			{
				do
				{
					var p = _buffer->GetRowPtr(y++) + (uint)(x << 2);
					CopyOrBlendPix(p, *colors++, *covers++);
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						var p = _buffer->GetRowPtr(y++) + (uint)(x << 2);
						CopyOrBlendPix(p, *colors++);
					} while (--length != 0);
				}
				else
				{
					do
					{
						var p = _buffer->GetRowPtr(y++) + (uint)(x << 2);
						CopyOrBlendPix(p, *colors++, cover);
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, Color8 color, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			var psrc = src.GetRowPtr(ysrc);
			if (psrc == null) return;

			var pdst = _buffer->GetRowPtr(ydst) + xdst * BytesPerPixel;
			do
			{
				CopyOrBlendPix(pdst, color, (byte)((*psrc * cover + Color8.BaseMask) >> Color8.BaseShift));
				++psrc;
				pdst += BytesPerPixel;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, Color8* colorLUT, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			var psrc = src.GetRowPtr(ysrc);
			if (psrc == null) return;

			var pdst = _buffer->GetRowPtr(ydst) + (uint)(xdst + xdst + xdst);

			if (cover == Common.CoverFull)
			{
				do
				{
					var color = colorLUT[*psrc];
					_blender.BlendPixel(pdst, color.R, color.G, color.B, color.A);
					++psrc;
					pdst += BytesPerPixel;
				} while (--length != 0);
			}
			else
			{
				do
				{
					CopyOrBlendPix(pdst, colorLUT[*psrc], cover);
					++psrc;
					pdst += BytesPerPixel;
				} while (--length != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendFrom<TPixfmtAlphaBlendColor>(TPixfmtAlphaBlendColor src, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlendColor : unmanaged, IPixfmt<Color8>
		{
			var psrc = src.GetRowPtr(ysrc);
			if (psrc == null) return;
			psrc += xsrc * src.BytesPerPixel;
			var pdst = _buffer->GetRowPtr(ydst) + (uint)(xdst + xdst + xdst);
			if (cover == Common.CoverFull)
			{
				do
				{
					var csrc = src.Pixel(xsrc, ysrc);
					var cdst = Pixel(xdst, ydst);
					if (csrc.A != 0)
					{
						if (csrc.A == Color8.BaseMask)
						{
							src.CopyPixel(xdst, ydst, cdst);
						}
						else
						{
							_blender.BlendPixel(pdst, csrc.R, csrc.G, csrc.B, csrc.A);
						}
					}
					psrc += src.BytesPerPixel;
					pdst += BytesPerPixel;
				} while (--length != 0);
			}
			else
			{
				do
				{
					var csrc = src.Pixel(xsrc, ysrc);
					CopyOrBlendPix(pdst, csrc, cover);
					psrc += src.BytesPerPixel;
					pdst += BytesPerPixel;
				} while (--length != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static void CopyOrBlendPix(byte* p, Color8 color, byte cover = Common.CoverFull)
		{
			if (color.A == 0)
			{
				return;
			}
			if (cover == Common.CoverFull)
			{
				if (color.A == Color8.BaseMask)
				{
					var uc = (color.R << (Order.R << 3)) | (color.G << (Order.G << 3)) | (color.B << (Order.B << 3)) | (Color8.BaseMask << (Order.A << 3));
					*((int*)p) = uc;
					// p[Order.R] = color.R;
					// p[Order.G] = color.G;
					// p[Order.B] = color.B;
					// p[Order.A] = Color8.BaseMask;
				}
				else
				{
					_blender.BlendPixel(p, color.R, color.G, color.B, color.A);
				}
			}
			else
			{
				var alpha = (color.A * (cover + 1)) >> 8;
				if (alpha == Color8.BaseMask)
				{
					var uc = (color.R << (Order.R << 3)) | (color.G << (Order.G << 3)) | (color.B << (Order.B << 3)) | (Color8.BaseMask << (Order.A << 3));
					*((int*)p) = uc;
					// p[Order.R] = color.R;
					// p[Order.G] = color.G;
					// p[Order.B] = color.B;
					// p[Order.A] = Color8.BaseMask;
				}
				else
				{
					_blender.BlendPixel(p, color.R, color.G, color.B, alpha, cover);
				}
			}
		}
	}
}