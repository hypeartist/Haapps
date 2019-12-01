using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public struct RasterizerSlClip64<TRasConv> : IRasterizerClipper64<TRasConv>
		where TRasConv : unmanaged, IRasterizerConverter64
	{
		private static TRasConv _conv = default;

		private Rectangle64 _clipRect;
		private bool _clipping;
		private double _x;
		private double _y;
		private int _flags;

		public void ResetClipping()
		{
			_clipping = false;
		}

		public void ClipBox(double x1, double y1, double x2, double y2)
		{
			_clipRect = new Rectangle64 { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
			_clipRect.Normalize();
			_clipping = true;
		}

		private void LineClipY(ref RasterizerCellsAA<Cell> ras, double x1, double y1, double x2, double y2, int f1, int f2)
		{
			f1 &= 10;
			f2 &= 10;
			if ((f1 | f2) == 0)
			{
				ras.Line(_conv.Xi(x1), _conv.Yi(y1), _conv.Xi(x2), _conv.Yi(y2));
			}
			else
			{
				if (f1 == f2)
				{
					// Invisible by Y
					return;
				}

				var tx1 = x1;
				var ty1 = y1;
				var tx2 = x2;
				var ty2 = y2;

				if ((f1 & 8) != 0) // y1 < clip.y1
				{
					tx1 = x1 + _conv.MulDiv(_clipRect.Y1 - y1, x2 - x1, y2 - y1);
					ty1 = _clipRect.Y1;
				}

				if ((f1 & 2) != 0) // y1 > clip.y2
				{
					tx1 = x1 + _conv.MulDiv(_clipRect.Y2 - y1, x2 - x1, y2 - y1);
					ty1 = _clipRect.Y2;
				}

				if ((f2 & 8) != 0) // y2 < clip.y1
				{
					tx2 = x1 + _conv.MulDiv(_clipRect.Y1 - y1, x2 - x1, y2 - y1);
					ty2 = _clipRect.Y1;
				}

				if ((f2 & 2) != 0) // y2 > clip.y2
				{
					tx2 = x1 + _conv.MulDiv(_clipRect.Y2 - y1, x2 - x1, y2 - y1);
					ty2 = _clipRect.Y2;
				}
				ras.Line(_conv.Xi(tx1), _conv.Yi(ty1), _conv.Xi(tx2), _conv.Yi(ty2));
			}
		}

		public void MoveTo(double x1, double y1)
		{
			_x = x1;
			_y = y1;
			if (_clipping)
			{
				_flags = ((_x > _clipRect.X2 ? 1 : 0) | ((_y > _clipRect.Y2 ? 1 : 0) << 1) | ((_x < _clipRect.X1 ? 1 : 0) << 2) | ((_y < _clipRect.Y1 ? 1 : 0) << 3));
			}
		}

		void IRasterizerClipper64<TRasConv>.LineTo(ref RasterizerCellsAA<Cell> ras, double x2, double y2)
		{
			if (_clipping)
			{
				var f2 = ClipLiangBarsky.ClippingFlags(x2, y2, ref _clipRect);
				if ((_flags & 10) == (f2 & 10) && (_flags & 10) != 0)
				{
					_x = x2;
					_y = y2;
					_flags = f2;
					return;
				}

				var x1 = _x;
				var y1 = _y;
				var f1 = _flags;
				double y3, y4;
				int f3, f4;

				switch (((f1 & 5) << 1) | f2 & 5)
				{
					case 0:
						LineClipY(ref ras, x1, y1, x2, y2, f1, f2);
						break;

					case 1: // x2 > clip.x2
						y3 = y1 + _conv.MulDiv(_clipRect.X2 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						LineClipY(ref ras, x1, y1, _clipRect.X2, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X2, y3, _clipRect.X2, y2, f3, f2);
						break;

					case 2: // x1 > clip.x2
						y3 = y1 + _conv.MulDiv(_clipRect.X2 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						LineClipY(ref ras, _clipRect.X2, y1, _clipRect.X2, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X2, y3, x2, y2, f3, f2);
						break;

					case 3: // x1 > clip.x2 && x2 > clip.x2
						LineClipY(ref ras, _clipRect.X2, y1, _clipRect.X2, y2, f1, f2);
						break;

					case 4: // x2 < clip.x1
						y3 = y1 + _conv.MulDiv(_clipRect.X1 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						LineClipY(ref ras, x1, y1, _clipRect.X1, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X1, y3, _clipRect.X1, y2, f3, f2);
						break;

					case 6: // x1 > clip.x2 && x2 < clip.x1
						y3 = y1 + _conv.MulDiv(_clipRect.X2 - x1, y2 - y1, x2 - x1);
						y4 = y1 + _conv.MulDiv(_clipRect.X1 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						f4 = ClipLiangBarsky.ClippingFlagsY(y4, ref _clipRect);
						LineClipY(ref ras, _clipRect.X2, y1, _clipRect.X2, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X2, y3, _clipRect.X1, y4, f3, f4);
						LineClipY(ref ras, _clipRect.X1, y4, _clipRect.X1, y2, f4, f2);
						break;

					case 8: // x1 < clip.x1
						y3 = y1 + _conv.MulDiv(_clipRect.X1 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						LineClipY(ref ras, _clipRect.X1, y1, _clipRect.X1, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X1, y3, x2, y2, f3, f2);
						break;

					case 9: // x1 < clip.x1 && x2 > clip.x2
						y3 = y1 + _conv.MulDiv(_clipRect.X1 - x1, y2 - y1, x2 - x1);
						y4 = y1 + _conv.MulDiv(_clipRect.X2 - x1, y2 - y1, x2 - x1);
						f3 = ClipLiangBarsky.ClippingFlagsY(y3, ref _clipRect);
						f4 = ClipLiangBarsky.ClippingFlagsY(y4, ref _clipRect);
						LineClipY(ref ras, _clipRect.X1, y1, _clipRect.X1, y3, f1, f3);
						LineClipY(ref ras, _clipRect.X1, y3, _clipRect.X2, y4, f3, f4);
						LineClipY(ref ras, _clipRect.X2, y4, _clipRect.X2, y2, f4, f2);
						break;

					case 12: // x1 < clip.x1 && x2 < clip.x1
						LineClipY(ref ras, _clipRect.X1, y1, _clipRect.X1, y2, f1, f2);
						break;
				}
				_flags = f2;
			}
			else
			{
				ras.Line(_conv.Xi(_x), _conv.Yi(_y), _conv.Xi(x2), _conv.Yi(y2));
			}
			_x = x2;
			_y = y2;
		}
	}
}