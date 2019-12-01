using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererMarkers<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private RendererPrimitives<TColor, TRendererBase> _rendererPrimitives;
		private readonly TRendererBase* _renderer;

		public RendererMarkers(ref TRendererBase renderer) : this()
		{
			_renderer = (TRendererBase*) Unsafe.AsPointer(ref renderer);
			_rendererPrimitives = new RendererPrimitives<TColor, TRendererBase>(ref renderer);
		}

		public TColor FillColor
		{
			get => _rendererPrimitives.FillColor;
			set => _rendererPrimitives.FillColor = value;
		}

		public TColor LineColor
		{
			get => _rendererPrimitives.LineColor;
			set => _rendererPrimitives.LineColor = value;
		}

		public readonly void Line(int x1, int y1, int x2, int y2, bool last = false) => _rendererPrimitives.Line(x1, y1, x2, y2, last);

		public readonly bool Visible(int x, int y, int r)
		{
			var rc = new Rectangle32(x - r, y - r, x + y, y + r);
			var clipBox = _renderer->ClipBox;
			return rc.Clip(ref clipBox);
		}

		public readonly void Square(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_rendererPrimitives.OutlinedRectangle(x - r, y - r, x + r, y + r);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Diamond(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				do
				{
					_renderer->BlendPixel(x - dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y + dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
						_renderer->BlendHLine(x - dx + 1, y - dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					++dx;
				} while (dy <= 0);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Circle(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_rendererPrimitives.OutlinedEllipse(x, y, r, r);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void CrossedCircle(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_rendererPrimitives.OutlinedEllipse(x, y, r, r);
				var r6 = r + (r >> 1);
				if (r <= 2)
				{
					r6++;
				}
				r >>= 1;
				_renderer->BlendHLine(x - r6, y, x - r, _rendererPrimitives.LineColor, Common.CoverFull);
				_renderer->BlendHLine(x + r, y, x + r6, _rendererPrimitives.LineColor, Common.CoverFull);
				_renderer->BlendVLine(x, y - r6, y - r, _rendererPrimitives.LineColor, Common.CoverFull);
				_renderer->BlendVLine(x, y + r, y + r6, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void SemiEllipseLeft(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var r8 = r*4/5;
				var dy = -r;
				var dx = 0;
				var interpolator = new BresenhamEllipseInterpolator(r*3/5, r + r8);
				do
				{
					dx += interpolator.DX;
					dy += interpolator.DY;

					_renderer->BlendPixel(x + dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);

					if (interpolator.DY != 0 && dx != 0)
					{
						_renderer->BlendVLine(x + dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					interpolator.Inc();
				} while (dy < r8);
				_renderer->BlendVLine(x + dy, y - dx, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void SemiEllipseRight(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var r8 = r*4/5;
				var dy = -r;
				var dx = 0;
				var interpolator = new BresenhamEllipseInterpolator(r * 3 / 5, r + r8);
				do
				{
					dx += interpolator.DX;
					dy += interpolator.DY;

					_renderer->BlendPixel(x - dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);

					if (interpolator.DY != 0 && dx != 0)
					{
						_renderer->BlendVLine(x - dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					interpolator.Inc();
				} while (dy < r8);
				_renderer->BlendVLine(x - dy, y - dx, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void SemiEllipseUp(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var r8 = r*4/5;
				var dy = -r;
				var dx = 0;
				var interpolator = new BresenhamEllipseInterpolator(r * 3 / 5, r + r8);
				do
				{
					dx += interpolator.DX;
					dy += interpolator.DY;

					_renderer->BlendPixel(x + dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);

					if (interpolator.DY != 0 && dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y - dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					interpolator.Inc();
				} while (dy < r8);
				_renderer->BlendHLine(x - dx, y - dy - 1, x + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void SemiEllipseDown(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var r8 = r*4/5;
				var dy = -r;
				var dx = 0;
				var interpolator = new BresenhamEllipseInterpolator(r * 3 / 5, r + r8);
				do
				{
					dx += interpolator.DX;
					dy += interpolator.DY;

					_renderer->BlendPixel(x + dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);

					if (interpolator.DY != 0 && dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y + dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					interpolator.Inc();
				} while (dy < r8);
				_renderer->BlendHLine(x - dx, y + dy + 1, x + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void TriangleLeft(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				var flip = 0;
				var r6 = r*3/5;
				do
				{
					_renderer->BlendPixel(x + dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendVLine(x + dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					dx += flip;
					flip ^= 1;
				} while (dy < r6);
				_renderer->BlendVLine(x + dy, y - dx, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void TriangleRight(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				var flip = 0;
				var r6 = r*3/5;
				do
				{
					_renderer->BlendPixel(x - dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendVLine(x - dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					dx += flip;
					flip ^= 1;
				} while (dy < r6);
				_renderer->BlendVLine(x - dy, y - dx, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void TriangleUp(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				var flip = 0;
				var r6 = r*3/5;
				do
				{
					_renderer->BlendPixel(x - dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y - dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					dx += flip;
					flip ^= 1;
				} while (dy < r6);
				_renderer->BlendHLine(x - dx, y - dy, x + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void TriangleDown(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				var flip = 0;
				var r6 = r*3/5;
				do
				{
					_renderer->BlendPixel(x - dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y + dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					dx += flip;
					flip ^= 1;
				} while (dy < r6);
				_renderer->BlendHLine(x - dx, y + dy, x + dx, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void FourRays(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r;
				var dx = 0;
				var flip = 0;
				var r3 = -(r/3);
				do
				{
					_renderer->BlendPixel(x - dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dx, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y - dx, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y + dx, _rendererPrimitives.LineColor, Common.CoverFull);

					if (dx != 0)
					{
						_renderer->BlendHLine(x - dx + 1, y + dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
						_renderer->BlendHLine(x - dx + 1, y - dy, x + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
						_renderer->BlendVLine(x + dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
						_renderer->BlendVLine(x - dy, y - dx + 1, y + dx - 1, _rendererPrimitives.FillColor, Common.CoverFull);
					}
					++dy;
					dx += flip;
					flip ^= 1;
				} while (dy <= r3);
				_rendererPrimitives.SolidRectangle(x + r3 + 1, y + r3 + 1, x - r3 - 1, y - r3 - 1);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Cross(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_renderer->BlendVLine(x, y - r, y + r, _rendererPrimitives.LineColor, Common.CoverFull);
				_renderer->BlendHLine(x - r, y, x + r, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Xing(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				var dy = -r*7/10;
				do
				{
					_renderer->BlendPixel(x + dy, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y + dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x + dy, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					_renderer->BlendPixel(x - dy, y - dy, _rendererPrimitives.LineColor, Common.CoverFull);
					++dy;
				} while (dy < 0);
			}
			_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
		}

		public readonly void Dash(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_renderer->BlendHLine(x - r, y, x + r, _rendererPrimitives.LineColor, Common.CoverFull);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Dot(int x, int y, int r)
		{
			if (!Visible(x, y, r)) return;
			if (r != 0)
			{
				_rendererPrimitives.SolidEllipse(x, y, r, r);
			}
			else
			{
				_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
			}
		}

		public readonly void Pixel(int x, int y, int r)
		{
			_renderer->BlendPixel(x, y, _rendererPrimitives.FillColor, Common.CoverFull);
		}

		public readonly void Marker(int x, int y, int r, Markers type)
		{
			switch (type)
			{
				case Markers.Square:
					Square(x, y, r);
					break;
				case Markers.Diamond:
					Diamond(x, y, r);
					break;
				case Markers.Circle:
					Circle(x, y, r);
					break;
				case Markers.CrossedCircle:
					CrossedCircle(x, y, r);
					break;
				case Markers.SemiEllipseLeft:
					SemiEllipseLeft(x, y, r);
					break;
				case Markers.SemiEllipseRight:
					SemiEllipseRight(x, y, r);
					break;
				case Markers.SemiEllipseUp:
					SemiEllipseUp(x, y, r);
					break;
				case Markers.SemiEllipseDown:
					SemiEllipseDown(x, y, r);
					break;
				case Markers.TriangleLeft:
					TriangleLeft(x, y, r);
					break;
				case Markers.TriangleRight:
					TriangleRight(x, y, r);
					break;
				case Markers.TriangleUp:
					TriangleUp(x, y, r);
					break;
				case Markers.TriangleDown:
					TriangleDown(x, y, r);
					break;
				case Markers.FourRays:
					FourRays(x, y, r);
					break;
				case Markers.Cross:
					Cross(x, y, r);
					break;
				case Markers.Xing:
					Xing(x, y, r);
					break;
				case Markers.Dash:
					Dash(x, y, r);
					break;
				case Markers.Dot:
					Dot(x, y, r);
					break;
				case Markers.Pixel:
					Pixel(x, y, r);
					break;
			}
		}
	}
}