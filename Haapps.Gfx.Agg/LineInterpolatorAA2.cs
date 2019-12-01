namespace Haapps.Gfx.Agg
{
	internal unsafe struct LineInterpolatorAA2<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private struct DistanceInterpolator2 : IDistanceInterpolator
		{
			private readonly int _dx;
			private readonly int _dy;
			private readonly int _dxStart;
			private readonly int _dyStart;
			private int _distStart;

			public readonly int DistEnd;
			public readonly int DxEnd;
			public readonly int DyEnd;

			public DistanceInterpolator2(int x1, int y1, int x2, int y2, int ex, int ey, int x, int y)
			{
				_dx = x2 - x1;
				_dy = y2 - y1;
				_dxStart = LineAA.LineMr(ex) - LineAA.LineMr(x2);
				_dyStart = LineAA.LineMr(ey) - LineAA.LineMr(y2);

				Dist = Common.RoundToI32((double)(x + LineAA.LineSubpixelScaleDiv2 - x2) * _dy - (double)(y + LineAA.LineSubpixelScaleDiv2 - y2) * _dx);

				_distStart = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(ex)) * _dyStart - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(ey)) * _dxStart;

				_dx <<= LineAA.LineSubpixelShift;
				_dy <<= LineAA.LineSubpixelShift;
				_dxStart <<= LineAA.LineMrSubpixelShift;
				_dyStart <<= LineAA.LineMrSubpixelShift;

				DxEnd = _dxStart;
				DyEnd = _dyStart;
				DistEnd = _distStart;
			}

			public int Dist { get; private set; }

			public void IncX(int dy)
			{
				Dist += _dy;
				_distStart += _dyStart;
				if (dy > 0)
				{
					Dist -= _dx;
					_distStart -= _dxStart;
				}

				if (dy >= 0) return;
				Dist += _dx;
				_distStart += _dxStart;
			}

			public void DecX(int dy)
			{
				Dist -= _dy;
				_distStart -= _dyStart;
				if (dy > 0)
				{
					Dist -= _dx;
					_distStart -= _dxStart;
				}

				if (dy >= 0) return;
				Dist += _dx;
				_distStart += _dxStart;
			}

			public void IncY(int dx)
			{
				Dist -= _dx;
				_distStart -= _dxStart;
				if (dx > 0)
				{
					Dist += _dy;
					_distStart += _dyStart;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				_distStart -= _dyStart;
			}

			public void DecY(int dx)
			{
				Dist += _dx;
				_distStart += _dxStart;
				if (dx > 0)
				{
					Dist += _dy;
					_distStart += _dyStart;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				_distStart -= _dyStart;
			}
		}

		private DistanceInterpolator2 _di;
		private LineInterpolatorAABase<TColor, TRendererBase> _base;

		public bool IsVertical => _base.IsVertical;

		public int Width => _base.Width;

		public int Count => _base.Count;

		public void Init(ref RendererOutlineAA<TColor, TRendererBase> ren, LineParameters* lp, int ex, int ey)
		{
			_base.Init(ref ren, lp);

			_di = new DistanceInterpolator2(lp->X1, lp->Y1, lp->X2, lp->Y2, ex, ey, lp->X1 & ~LineAA.LineSubpixelMask, lp->Y1 & ~LineAA.LineSubpixelMask);
			_base.Li.AdjustForward();
			_base.Step -= _base.MaxExtent;
		}

		public bool StepHor()
		{
			int dist;
			var s1 = _base.StepHorBase(ref _di);
			var p0 = _base.Covers + LineInterpolatorAABase < TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			var distEnd = _di.DistEnd;

			var npix = 0;
			*p1 = 0;
			if (distEnd > 0)
			{
				*p1 = _base.Renderer->Cover(s1);
				++npix;
			}

			++p1;

			var dy = 1;
			while ((dist = _base.Dist[dy] - s1) <= _base.Width)
			{
				distEnd -= _di.DxEnd;
				*p1 = 0;
				if (distEnd > 0)
				{
					*p1 = _base.Renderer->Cover(dist);
					++npix;
				}

				++p1;
				++dy;
			}

			dy = 1;
			distEnd = _di.DistEnd;
			while ((dist = _base.Dist[dy] + s1) <= _base.Width)
			{
				distEnd += _di.DxEnd;
				*--p0 = 0;
				if (distEnd > 0)
				{
					*p0 = _base.Renderer->Cover(dist);
					++npix;
				}

				++dy;
			}

			_base.Renderer->BlendSolidVSpan(_base.X, _base.Y - dy + 1, (int)(p1 - p0), p0);
			return npix != 0 && ++_base.Step < _base.Count;
		}

		public bool StepVer()
		{
			int dist;
			var s1 = _base.StepVerBase(ref _di);
			var p0 = _base.Covers + LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			var distEnd = _di.DistEnd;

			var npix = 0;
			*p1 = 0;
			if (distEnd > 0)
			{
				*p1 = _base.Renderer->Cover(s1);
				++npix;
			}

			++p1;

			var dx = 1;
			while ((dist = _base.Dist[dx] - s1) <= _base.Width)
			{
				distEnd += _di.DyEnd;
				*p1 = 0;
				if (distEnd > 0)
				{
					*p1 = _base.Renderer->Cover(dist);
					++npix;
				}

				++p1;
				++dx;
			}

			dx = 1;
			distEnd = _di.DistEnd;
			while ((dist = _base.Dist[dx] + s1) <= _base.Width)
			{
				distEnd -= _di.DyEnd;
				*--p0 = 0;
				if (distEnd > 0)
				{
					*p0 = _base.Renderer->Cover(dist);
					++npix;
				}

				++dx;
			}

			_base.Renderer->BlendSolidHSpan(_base.X - dx + 1, _base.Y, (int)(p1 - p0), p0);
			return npix != 0 && ++_base.Step < _base.Count;
		}
	}
}