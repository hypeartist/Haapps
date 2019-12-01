using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererBase<TColor, TPixfmt> : IRendererBase<TColor>
		where TColor : unmanaged, IColor
		where TPixfmt : unmanaged, IPixfmt<TColor>, IPixfmtAlphaBlend<TColor>
	{
		private TPixfmt* _pixfmt;
		private Rectangle32 _clipBox;

		public RendererBase(ref TPixfmt pixfmt)
		{
			_pixfmt = (TPixfmt*) Unsafe.AsPointer(ref pixfmt);
			_clipBox = new Rectangle32(0, 0, _pixfmt->Width - 1, _pixfmt->Height - 1);
		}

		public int Width => _pixfmt->Width;

		public int Height => _pixfmt->Height;

		public int MaxX => _clipBox.X2;

		public int MaxY => _clipBox.Y2;

		public int MinX => _clipBox.X1;

		public int MinY => _clipBox.Y1;

		public Rectangle32 ClipBox => _clipBox;

		public void Clear(TColor color)
		{
			if (_pixfmt->Width == 0)
			{
				return;
			}

			for (var y = 0; y < _pixfmt->Height; y++)
			{
				_pixfmt->CopyHLine(0, y, _pixfmt->Width, color);
			}
		}

		public bool InBox(int x, int y)
		{
			return x >= _clipBox.X1 && y >= _clipBox.Y1 && x <= _clipBox.X2 && y <= _clipBox.Y2;
		}

		public void BlendPixel(int x, int y, TColor color, byte cover)
		{
			if (InBox(x, y))
			{
				_pixfmt->BlendPixel(x, y, color, cover);
			}
		}

		public void CopyBar(int x1, int y1, int x2, int y2, TColor c)
		{
			var rc = new Rectangle32(x1, y1, x2, y2);
			rc.Normalize();
			if (!rc.Clip(ref _clipBox)) return;
			int y;
			for (y = rc.Y1; y <= rc.Y2; y++)
			{
				_pixfmt->CopyHLine(rc.X1, y, (rc.X2 - rc.X1 + 1), c);
			}
		}

		public void BlendBar(int x1, int y1, int x2, int y2, TColor c, byte cover)
		{
			var rc = new Rectangle32(x1, y1, x2, y2);
			rc.Normalize();
			if (!rc.Clip(ref _clipBox)) return;
			for (var y = rc.Y1; y <= rc.Y2; y++)
			{
				_pixfmt->BlendHLine(rc.X1, y, rc.X2 - rc.X1 + 1, c, cover);
			}
		}

		public void CopyHLine(int x1, int y, int x2, TColor color)
		{
			if (x1 > x2)
			{
				var t = x2;
				x2 = x1;
				x1 = t;
			}

			if (y > MaxY || y < MinY || x1 > MaxX || x2 < MinX) return;

			if (x1 < MinX)
			{
				x1 = MinX;
			}

			if (x2 > MaxX)
			{
				x2 = MaxX;
			}

			_pixfmt->CopyHLine(x1, y, x2 - x1 + 1, color);
		}

		public void CopyVLine(int x, int y1, int y2, TColor color)
		{
			if (y1 > y2)
			{
				var t = y2;
				y2 = y1;
				y1 = t;
			}

			if (x > MaxX || x < MinX || y1 > MaxY || y2 < MinY) return;

			if (y1 < MinY)
			{
				y1 = MinY;
			}

			if (y2 > MaxY)
			{
				y2 = MaxY;
			}

			_pixfmt->CopyVLine(x, y1, y2 - y1 + 1, color);
		}

		public void BlendHLine(int x1, int y, int x2, TColor color, byte cover)
		{
			if (x1 > x2)
			{
				var t = x2;
				x2 = x1;
				x1 = t;
			}

			if (y > MaxY || y < MinY || x1 > MaxX || x2 < MinX) return;

			if (x1 < MinX)
			{
				x1 = MinX;
			}

			if (x2 > MaxX)
			{
				x2 = MaxX;
			}

			_pixfmt->BlendHLine(x1, y, x2 - x1 + 1, color, cover);
		}

		public void BlendVLine(int x, int y1, int y2, TColor color, byte cover)
		{
			if (y1 > y2)
			{
				var t = y2;
				y2 = y1;
				y1 = t;
			}

			if (x > MaxX || x < MinX || y1 > MaxY || y2 < MinY) return;

			if (y1 < MinY)
			{
				y1 = MinY;
			}

			if (y2 > MaxY)
			{
				y2 = MaxY;
			}

			_pixfmt->BlendVLine(x, y1, y2 - y1 + 1, color, cover);
		}

		public void BlendSolidHSpan(int x, int y, int length, TColor color, byte* covers)
		{
			if (y > MaxY || y < MinY) return;

			if (x < MinX)
			{
				var d = MinX - x;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				covers += d;
				x = MinX;
			}

			if (x + length > MaxX)
			{
				length = MaxX - x + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->BlendSolidHSpan(x, y, length, color, covers);
		}

		public void BlendSolidVSpan(int x, int y, int length, TColor color, byte* covers)
		{
			if (x > MaxX || x < MinX) return;

			if (y < MinY)
			{
				var d = MinY - y;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				covers += d;
				y = MinY;
			}

			if (y + length > MaxY)
			{
				length = MaxY - y + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->BlendSolidVSpan(x, y, length, color, covers);
		}

		public void CopyColorHSpan(int x, int y, int length, TColor* colors)
		{
			if (y > MaxY || y < MinY) return;

			if (x < MinX)
			{
				var d = MinX - x;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				colors += d;
				x = MinX;
			}

			if (x + length > MaxX)
			{
				length = MaxX - x + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->CopyColorHSpan(x, y, length, colors);
		}

		public void CopyColorVSpan(int x, int y, int length, TColor* colors)
		{
			if (x > MaxX || x < MinX) return;

			if (y < MinY)
			{
				var d = MinY - y;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				colors += d;
				y = MinY;
			}

			if (y + length > MaxY)
			{
				length = MaxY - y + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->CopyColorVSpan(x, y, length, colors);
		}

		public void BlendColorHSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover)
		{
			if (y > MaxY || y < MinY) return;

			if (x < MinX)
			{
				var d = MinX - x;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				if (covers != null)
				{
					covers += d;
				}

				colors += d;
				x = MinX;
			}

			if (x + length > MaxX)
			{
				length = MaxX - x + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->BlendColorHSpan(x, y, length, colors, covers, cover);
		}

		public void BlendColorVSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover)
		{
			if (x > MaxX || x < MinX) return;

			if (y < MinY)
			{
				var d = MinY - y;
				length -= d;
				if (length <= 0)
				{
					return;
				}

				if (covers != null)
				{
					covers += d;
				}

				colors += d;
				y = MinY;
			}

			if (y + length > MaxY)
			{
				length = MaxY - y + 1;
				if (length <= 0)
				{
					return;
				}
			}

			_pixfmt->BlendColorVSpan(x, y, length, colors, covers, cover);
		}

		public void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor color, int dx, int dy, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			var rsrc = new Rectangle32(0, 0, src.Width, src.Height);
			var rdst = new Rectangle32(rsrc.X1 + dx, rsrc.Y1 + dy, rsrc.X2 + dx, rsrc.Y2 + dy);
			var rc = ClipRectArea(ref rdst, ref rsrc, src.Width, src.Height);

			if (rc.X2 <= 0) return;
			var incy = 1;
			if (rdst.Y1 > rsrc.Y1)
			{
				rsrc.Y1 += rc.Y2 - 1;
				rdst.Y1 += rc.Y2 - 1;
				incy = -1;
			}

			while (rc.Y2 > 0)
			{
				var rw = src.GetRowInfo(rsrc.Y1);
				var x1Src = rsrc.X1;
				var x1Dst = rdst.X1;
				var len = rc.X2;
				if (rw.X1 > x1Src)
				{
					x1Dst += rw.X1 - x1Src;
					len -= rw.X1 - x1Src;
					x1Src = rw.X1;
				}

				if (len > 0)
				{
					if (x1Src + len - 1 > rw.X2)
					{
						len -= x1Src + len - rw.X2 - 1;
					}

					if (len > 0)
					{
						_pixfmt->BlendFromColor(src, color, x1Dst, rdst.Y1, x1Src, rsrc.Y1, len, cover);
					}
				}

				rdst.Y1 += incy;
				rsrc.Y1 += incy;
				--rc.Y2;
			}
		}

		public void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor* colorLUT, int dx, int dy, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor
		{
			var rsrc = new Rectangle32(0, 0, src.Width, src.Height);
			var rdst = new Rectangle32(rsrc.X1 + dx, rsrc.Y1 + dy, rsrc.X2 + dx, rsrc.Y2 + dy);
			var rc = ClipRectArea(ref rdst, ref rsrc, src.Width, src.Height);

			if (rc.X2 <= 0) return;
			var incy = 1;
			if (rdst.Y1 > rsrc.Y1)
			{
				rsrc.Y1 += rc.Y2 - 1;
				rdst.Y1 += rc.Y2 - 1;
				incy = -1;
			}

			while (rc.Y2 > 0)
			{
				var rw = src.GetRowInfo(rsrc.Y1);
				if (rw.Pointer != (void*) 0)
				{
					var x1Src = rsrc.X1;
					var x1Dst = rdst.X1;
					var len = rc.X2;
					if (rw.X1 > x1Src)
					{
						x1Dst += rw.X1 - x1Src;
						len -= rw.X1 - x1Src;
						x1Src = rw.X1;
					}

					if (len > 0)
					{
						if (x1Src + len - 1 > rw.X2)
						{
							len -= x1Src + len - rw.X2 - 1;
						}

						if (len > 0)
						{
							_pixfmt->BlendFromLUT(src, colorLUT, x1Dst, rdst.Y1, x1Src, rsrc.Y1, len, cover);
						}
					}
				}

				rdst.Y1 += incy;
				rsrc.Y1 += incy;
				--rc.Y2;
			}
		}

		public void BlendFrom<TPixfmtAlphaBlendColor2>(TPixfmtAlphaBlendColor2 src, int dx, int dy, byte cover)
			where TPixfmtAlphaBlendColor2 : unmanaged, IPixfmt<TColor>
		{
			var rsrc = new Rectangle32(0, 0, src.Width, src.Height);
			var rdst = new Rectangle32(rsrc.X1 + dx, rsrc.Y1 + dy, rsrc.X2 + dx, rsrc.Y2 + dy);
			var rc = ClipRectArea(ref rdst, ref rsrc, src.Width, src.Height);

			if (rc.X2 <= 0) return;
			var incy = 1;
			if (rdst.Y1 > rsrc.Y1)
			{
				rsrc.Y1 += rc.Y2 - 1;
				rdst.Y1 += rc.Y2 - 1;
				incy = -1;
			}

			while (rc.Y2 > 0)
			{
				var rw = src.GetRowInfo(rsrc.Y1);
				var x1Src = rsrc.X1;
				var x1Dst = rdst.X1;
				var len = rc.X2;
				if (rw.X1 > x1Src)
				{
					x1Dst += rw.X1 - x1Src;
					len -= rw.X1 - x1Src;
					x1Src = rw.X1;
				}

				if (len > 0)
				{
					if (x1Src + len - 1 > rw.X2)
					{
						len -= x1Src + len - rw.X2 - 1;
					}

					if (len > 0)
					{
						_pixfmt->BlendFrom(src, x1Dst, rdst.Y1, x1Src, rsrc.Y1, len, cover);
					}
				}

				rdst.Y1 += incy;
				rsrc.Y1 += incy;
				--rc.Y2;
			}
		}

		private Rectangle32 ClipRectArea(ref Rectangle32 dst, ref Rectangle32 src, int wsrc, int hsrc)
		{
			var rc = new Rectangle32(0, 0, 0, 0);
			var cb = _clipBox;
			++cb.X2;
			++cb.Y2;

			if (src.X1 < 0)
			{
				dst.X1 -= src.X1;
				src.X1 = 0;
			}

			if (src.Y1 < 0)
			{
				dst.Y1 -= src.Y1;
				src.Y1 = 0;
			}

			if (src.X2 > wsrc)
			{
				src.X2 = wsrc;
			}

			if (src.Y2 > hsrc)
			{
				src.Y2 = hsrc;
			}

			if (dst.X1 < cb.X1)
			{
				src.X1 += cb.X1 - dst.X1;
				dst.X1 = cb.X1;
			}

			if (dst.Y1 < cb.Y1)
			{
				src.Y1 += cb.Y1 - dst.Y1;
				dst.Y1 = cb.Y1;
			}

			if (dst.X2 > cb.X2)
			{
				dst.X2 = cb.X2;
			}

			if (dst.Y2 > cb.Y2)
			{
				dst.Y2 = cb.Y2;
			}

			rc.X2 = dst.X2 - dst.X1;
			rc.Y2 = dst.Y2 - dst.Y1;

			if (rc.X2 > src.X2 - src.X1)
			{
				rc.X2 = src.X2 - src.X1;
			}

			if (rc.Y2 > src.Y2 - src.Y1)
			{
				rc.Y2 = src.Y2 - src.Y1;
			}

			return rc;
		}

	}
}