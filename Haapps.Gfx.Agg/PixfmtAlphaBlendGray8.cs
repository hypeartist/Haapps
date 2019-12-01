using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct PixfmtAlphaBlendGray8<TRenderingBuffer, TOrderColor, TParams, TBlenderGray> : IPixfmt<Gray8>, IPixelDataSrcAttacher<TRenderingBuffer>, IPixelDataAccessorAttacher, IPixfmtAlphaBlend<Gray8>
		where TRenderingBuffer : unmanaged, IRenderingBuffer
		where TOrderColor : struct, IOrderColor24
		where TParams : struct, IMaskParams<TOrderColor>
		where TBlenderGray : struct, IBlenderGray8
	{
		private static TBlenderGray _blender = default;
		private static readonly TParams _params = default;

		private TRenderingBuffer* _buffer;

		public PixfmtAlphaBlendGray8(ref TRenderingBuffer buffer)
		{
			_buffer = (TRenderingBuffer*) Unsafe.AsPointer(ref buffer);
		}

		public readonly int BytesPerPixel
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _params.Step;
		}

		public readonly int Width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Width;
		}

		public readonly int Height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Height;
		}

		public readonly int Stride
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _buffer->Stride;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int y) => _buffer->GetRowPtr(y);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int x, int y, int length) => _buffer->GetRowPtr(x, y, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetPixPtr(int x, int y) => _buffer->GetRowPtr(y) + x * _params.Step + _params.Offset;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly RowInfo GetRowInfo(int y) => _buffer->GetRowInfo(y);

		public void Attach(ref TRenderingBuffer dataSrc)
		{
			_buffer = (TRenderingBuffer*) Unsafe.AsPointer(ref dataSrc);
		}

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

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly Gray8 Pixel(int x, int y)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);
			return new Gray8(*p);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyPixel(int x, int y, Gray8 color)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);
			*p = color.V;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendPixel(int x, int y, Gray8 color, byte cover)
		{
			CopyOrBlendPix(_buffer->GetRowPtr(y) + x * _params.Step + _params.Offset, color, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyHLine(int x, int y, int length, Gray8 color)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);

			do
			{
				*p = color.V;
				p += _params.Step;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyVLine(int x, int y, int length, Gray8 color)
		{
			do
			{
				var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);

				*p = color.V;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendHLine(int x, int y, int length, Gray8 color, byte cover)
		{
			if (color.A == 0) return;
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);

			var alpha = (byte) ((color.A * (cover + 1)) >> 8);
			if (alpha == Gray8.BaseMask)
			{
				do
				{
					*p = color.V;
					p += _params.Step;
				} while (--length != 0);
			}
			else
			{
				do
				{
					_blender.BlendPixel(p, color.V, alpha, cover);
					p += _params.Step;
				} while (--length != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendVLine(int x, int y, int length, Gray8 color, byte cover)
		{
			if (color.A == 0) return;

			var alpha = (byte) ((color.A * (cover + 1)) >> 8);
			if (alpha == Gray8.BaseMask)
			{
				do
				{
					var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);
					*p = color.V;
				} while (--length != 0);
			}
			else
			{
				do
				{
					var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);
					_blender.BlendPixel(p, color.V, alpha, cover);
				} while (--length != 0);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidHSpan(int x, int y, int length, Gray8 color, byte* covers)
		{
			if (color.A == 0) return;
			var p = _buffer->GetRowPtr(y) + x * _params.Step + _params.Offset;

			do
			{
				var alpha = (byte) ((color.A * (*covers + 1)) >> 8);
				if (alpha == Gray8.BaseMask)
				{
					*p = color.V;
				}
				else
				{
					_blender.BlendPixel(p, color.V, alpha, *covers);
				}

				p += _params.Step;
				++covers;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidVSpan(int x, int y, int length, Gray8 color, byte* covers)
		{
			if (color.A == 0) return;
			do
			{
				var alpha = (byte) ((color.A * (*covers + 1)) >> 8);

				var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);

				if (alpha == Gray8.BaseMask)
				{
					*p = color.V;
				}
				else
				{
					_blender.BlendPixel(p, color.V, alpha, *covers);
				}

				++covers;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorHSpan(int x, int y, int length, Gray8* colors)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);

			do
			{
				*p = colors->V;
				p += _params.Step;
				++colors;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorVSpan(int x, int y, int length, Gray8* colors)
		{
			do
			{
				var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);
				*p = colors->V;
				++colors;
			} while (--length != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorHSpan(int x, int y, int length, Gray8* colors, byte* covers, byte cover)
		{
			var p = _buffer->GetRowPtr(y) + (uint)(x * _params.Step + _params.Offset);

			if (covers != null)
			{
				do
				{
					CopyOrBlendPix(p, *colors++, *covers++);
					p += _params.Step;
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						if (colors->A == Gray8.BaseMask)
						{
							*p = colors->V;
						}
						else
						{
							CopyOrBlendPix(p, *colors);
						}

						p += _params.Step;
						++colors;
					} while (--length != 0);
				}
				else
				{
					do
					{
						CopyOrBlendPix(p, *colors++, cover);
						p += _params.Step;
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorVSpan(int x, int y, int length, Gray8* colors, byte* covers, byte cover)
		{
			if (covers != null)
			{
				do
				{
					var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);
					CopyOrBlendPix(p, *colors++, *covers++);
				} while (--length != 0);
			}
			else
			{
				if (cover == Common.CoverFull)
				{
					do
					{
						var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);

						if (colors->A == Gray8.BaseMask)
						{
							*p = colors->V;
						}
						else
						{
							CopyOrBlendPix(p, *colors);
						}

						++colors;
					} while (--length != 0);
				}
				else
				{
					do
					{
						var p = _buffer->GetRowPtr(y++) + (uint)(x * _params.Step + _params.Offset);
						CopyOrBlendPix(p, *colors++, cover);
					} while (--length != 0);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		private static void CopyOrBlendPix(byte* p, Gray8 c)
		{
			if (c.A == 0) return;
			if (c.A == Gray8.BaseMask)
			{
				*p = c.V;
			}
			else
			{
				_blender.BlendPixel(p, c.V, c.A);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		private static void CopyOrBlendPix(byte* p, Gray8 c, byte cover)
		{
			if (c.A == 0) return;
			var alpha = (byte) ((c.A * (cover + 1)) >> 8);
			if (alpha == Gray8.BaseMask)
			{
				*p = c.V;
			}
			else
			{
				_blender.BlendPixel(p, c.V, alpha, cover);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, Gray8 color, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			var p = src.GetRowPtr(ysrc);
			var dst = _buffer->GetRowPtr(ydst) + (uint)(xdst * BytesPerPixel);
			if (p == (byte*) 0) return;
			do
			{
				CopyOrBlendPix(dst, color, (byte) ((*p*cover + Gray8.BaseMask) >> Gray8.BaseShift));
				++p;
				++dst;
			} while (--length != 0);
		}

		public readonly void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, Gray8* colorLUT, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			throw new System.NotImplementedException();
		}

		public readonly void BlendFrom<TPixfmtAlphaBlendColor>(TPixfmtAlphaBlendColor src, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlendColor : unmanaged, IPixfmt<Gray8>
		{
			throw new System.NotImplementedException();
		}
	}
}