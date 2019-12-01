namespace Haapps.Gfx.Agg
{
	internal unsafe struct LineInterpolatorAA0<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private struct DistanceInterpolator1 : IDistanceInterpolator
		{
			private readonly int _dx;
			private readonly int _dy;

			public DistanceInterpolator1(int x1, int y1, int x2, int y2, int x, int y)
			{
				_dx = x2 - x1;
				_dy = y2 - y1;
				Dist = Common.RoundToI32((double)(x + LineAA.LineSubpixelScaleDiv2 - x2) * _dy - (double)(y + LineAA.LineSubpixelScaleDiv2 - y2) * _dx);
				_dx <<= LineAA.LineSubpixelShift;
				_dy <<= LineAA.LineSubpixelShift;
			}

			public int Dist { get; private set; }

			public void IncX(int dy)
			{
				Dist += _dy;
				if (dy > 0)
				{
					Dist -= _dx;
				}
				if (dy < 0)
				{
					Dist += _dx;
				}
			}

			public void DecX(int dy)
			{
				Dist -= _dy;
				if (dy > 0)
				{
					Dist -= _dx;
				}
				if (dy < 0)
				{
					Dist += _dx;
				}
			}

			public void IncY(int dx)
			{
				Dist -= _dx;
				if (dx > 0)
				{
					Dist += _dy;
				}
				if (dx < 0)
				{
					Dist -= _dy;
				}
			}

			public void DecY(int dx)
			{
				Dist += _dx;
				if (dx > 0)
				{
					Dist += _dy;
				}
				if (dx < 0)
				{
					Dist -= _dy;
				}
			}
		}
		private DistanceInterpolator1 _di;
		private LineInterpolatorAABase<TColor, TRendererBase> _base;

		internal void Init(ref RendererOutlineAA<TColor, TRendererBase> ren, LineParameters* lp)
		{
			_base.Init(ref ren, lp);
			_di = new DistanceInterpolator1(lp->X1, lp->Y1, lp->X2, lp->Y2, lp->X1 & ~LineAA.LineSubpixelMask, lp->Y1 & ~LineAA.LineSubpixelMask);
			_base.Li.AdjustForward();
		}

		public bool IsVertical => _base.IsVertical;

		public int Width => _base.Width;

		public int Count => _base.Count;

		public bool StepHor()
		{
			int dist;
			var s1 = _base.StepHorBase(ref _di);
			var p0 = _base.Covers + LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			*p1++ = _base.Renderer->Cover(s1);

			var dy = 1;
			while ((dist = _base.Dist[dy] - s1) <= _base.Width)
			{
				*p1++ = _base.Renderer->Cover(dist);
				++dy;
			}

			dy = 1;
			while ((dist = _base.Dist[dy] + s1) <= _base.Width)
			{
				*--p0 = _base.Renderer->Cover(dist);
				++dy;
			}
			_base.Renderer->BlendSolidVSpan(_base.X, _base.Y - dy + 1, (int)(p1 - p0), p0);
			return ++_base.Step < _base.Count;
		}

		public bool StepVer()
		{
			int dist;
			var s1 = _base.StepVerBase(ref _di);
			var p0 = _base.Covers + LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			*p1++ = _base.Renderer->Cover(s1);

			var dx = 1;
			while ((dist = _base.Dist[dx] - s1) <= _base.Width)
			{
				*p1++ = _base.Renderer->Cover(dist);
				++dx;
			}

			dx = 1;
			while ((dist = _base.Dist[dx] + s1) <= _base.Width)
			{
				*--p0 = _base.Renderer->Cover(dist);
				++dx;
			}
			_base.Renderer->BlendSolidHSpan(_base.X - dx + 1, _base.Y, (int)(p1 - p0), p0);
			return ++_base.Step < _base.Count;
		}
	}
}