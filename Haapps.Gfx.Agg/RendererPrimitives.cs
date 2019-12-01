using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererPrimitives<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private int _currX;
		private int _currY;

		private readonly TRendererBase* _renderer;

		public RendererPrimitives(ref TRendererBase rb) : this() => _renderer = (TRendererBase*) Unsafe.AsPointer(ref rb);

		public TColor FillColor { get; set; }

		public TColor LineColor { get; set; }

		public static int Coord(double c)
		{
			var v = c*BresenhamLineInterpolator.SubpixelScale;
			return Common.RoundToI32(Common.RoundToU32(v));
		}

		public readonly void Rectangle(int x1, int y1, int x2, int y2)
		{
			_renderer->BlendHLine(x1, y1, x2 - 1, LineColor, Common.CoverFull);
			_renderer->BlendVLine(x2, y1, y2 - 1, LineColor, Common.CoverFull);
			_renderer->BlendHLine(x1 + 1, y2, x2, LineColor, Common.CoverFull);
			_renderer->BlendVLine(x1, y1 + 1, y2, LineColor, Common.CoverFull);
		}

		public readonly void SolidRectangle(int x1, int y1, int x2, int y2)
		{
			_renderer->BlendBar(x1, y1, x2, y2, FillColor, Common.CoverFull);
		}

		public readonly void OutlinedRectangle(int x1, int y1, int x2, int y2)
		{
			Rectangle(x1, y1, x2, y2);
			_renderer->BlendBar(x1 + 1, y1 + 1, x2 - 1, y2 - 1, FillColor, Common.CoverFull);
		}

		public readonly void Ellipse(int x, int y, int rx, int ry)
		{
			var interpolator = new BresenhamEllipseInterpolator(rx, ry);
			var dx = 0;
			var dy = -ry;
			do
			{
				dx += interpolator.DX;
				dy += interpolator.DY;
				_renderer->BlendPixel(x + dx, y + dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x + dx, y - dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x - dx, y - dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x - dx, y + dy, LineColor, Common.CoverFull);
				interpolator.Inc();
			}
			while(dy < 0);
		}

		public readonly void SolidEllipse(int x, int y, int rx, int ry)
		{
			var interpolator = new BresenhamEllipseInterpolator(rx, ry);
			var dx = 0;
			var dy = -ry;
			var dy0 = dy;
			var dx0 = dx;

			do
			{
				dx += interpolator.DX;
				dy += interpolator.DY;

				if(dy != dy0)
				{
					_renderer->BlendHLine(x-dx0, y+dy0, x+dx0, FillColor, Common.CoverFull);
					_renderer->BlendHLine(x - dx0, y - dy0, x + dx0, FillColor, Common.CoverFull);
				}
				dx0 = dx;
				dy0 = dy;
				interpolator.Inc();
			}
			while(dy < 0);
			_renderer->BlendHLine(x - dx0, y + dy0, x + dx0, FillColor, Common.CoverFull);
		}

		public readonly void OutlinedEllipse(int x, int y, int rx, int ry)
		{
			var interpolator = new BresenhamEllipseInterpolator(rx, ry);
			var dx = 0;
			var dy = -ry;

			do
			{
				dx += interpolator.DX;
				dy += interpolator.DY;

				_renderer->BlendPixel(x + dx, y + dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x + dx, y - dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x - dx, y - dy, LineColor, Common.CoverFull);
				_renderer->BlendPixel(x - dx, y + dy, LineColor, Common.CoverFull);

				if(interpolator.DY != 0 && dx != 0)
				{
					_renderer->BlendHLine(x - dx + 1, y + dy, x + dx - 1, FillColor, Common.CoverFull);
					_renderer->BlendHLine(x - dx + 1, y - dy, x + dx - 1, FillColor, Common.CoverFull);
				}
				interpolator.Inc();
			}
			while(dy < 0);
		}

		public readonly void Line(int x1, int y1, int x2, int y2, bool last=false)
		{
			var interpolator = new BresenhamLineInterpolator(x1, y1, x2, y2);

			var len = interpolator.Length;
			if(len == 0)
			{
				if(last)
				{
					_renderer->BlendPixel(BresenhamLineInterpolator.LineLr(x1), BresenhamLineInterpolator.LineLr(y1), LineColor, Common.CoverFull);
				}
				return;
			}

			if(last) ++len;

			if(interpolator.IsVer)
			{
				do
				{
					_renderer->BlendPixel(interpolator.X2, interpolator.Y1, LineColor, Common.CoverFull);
					interpolator.VStep();
				}
				while(--len != 0);
			}
			else
			{
				do
				{
					_renderer->BlendPixel(interpolator.X1, interpolator.Y2, LineColor, Common.CoverFull);
					interpolator.HStep();
				}
				while(--len != 0);
			}
		}

		public void MoveTo(int x, int y)
		{
			_currX = x;
			_currY = y;
		}

		public void LineTo(int x, int y, bool last = false)
		{
			Line(_currX, _currY, x, y, last);
			_currX = x;
			_currY = y;
		}
	}
}