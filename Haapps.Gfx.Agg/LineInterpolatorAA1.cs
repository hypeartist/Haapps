namespace Haapps.Gfx.Agg
{
	internal unsafe struct LineInterpolatorAA1<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private struct DistanceInterpolator2 : IDistanceInterpolator
		{
			private readonly int _dx;
			private readonly int _dy;

			public readonly int DxStart;
			public readonly int DyStart;
			public int DistStart;

			public DistanceInterpolator2(int x1, int y1, int x2, int y2, int sx, int sy, int x, int y)
			{
				_dx = x2 - x1;
				_dy = y2 - y1;
				DxStart = LineAA.LineMr(sx) - LineAA.LineMr(x1);
				DyStart = LineAA.LineMr(sy) - LineAA.LineMr(y1);

				Dist = Common.RoundToI32((double)(x + LineAA.LineSubpixelScaleDiv2 - x2) * _dy - (double)(y + LineAA.LineSubpixelScaleDiv2 - y2) * _dx);

				DistStart = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(sx)) * DyStart - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(sy)) * DxStart;

				_dx <<= LineAA.LineSubpixelShift;
				_dy <<= LineAA.LineSubpixelShift;
				DxStart <<= LineAA.LineMrSubpixelShift;
				DyStart <<= LineAA.LineMrSubpixelShift;
			}

			public int Dist { get; private set; }

			public void IncX(int dy)
			{
				Dist += _dy;
				DistStart += DyStart;
				if (dy > 0)
				{
					Dist -= _dx;
					DistStart -= DxStart;
				}

				if (dy >= 0) return;
				Dist += _dx;
				DistStart += DxStart;
			}

			public void DecX(int dy)
			{
				Dist -= _dy;
				DistStart -= DyStart;
				if (dy > 0)
				{
					Dist -= _dx;
					DistStart -= DxStart;
				}

				if (dy >= 0) return;
				Dist += _dx;
				DistStart += DxStart;
			}

			public void IncY(int dx)
			{
				Dist -= _dx;
				DistStart -= DxStart;
				if (dx > 0)
				{
					Dist += _dy;
					DistStart += DyStart;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				DistStart -= DyStart;
			}

			public void DecY(int dx)
			{
				Dist += _dx;
				DistStart += DxStart;
				if (dx > 0)
				{
					Dist += _dy;
					DistStart += DyStart;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				DistStart -= DyStart;
			}
		}

		private DistanceInterpolator2 _di;
		private LineInterpolatorAABase<TColor, TRendererBase> _base;

		public bool IsVertical => _base.IsVertical;

		public int Width => _base.Width;

		public int Count => _base.Count;

		public void Init(ref RendererOutlineAA<TColor, TRendererBase> ren, LineParameters* lp, int sx, int sy)
		{
			_base.Init(ref ren, lp);

			_di = new DistanceInterpolator2(lp->X1, lp->Y1, lp->X2, lp->Y2, sx, sy, lp->X1 & ~LineAA.LineSubpixelMask, lp->Y1 & ~LineAA.LineSubpixelMask);

			int dist1Start;
			int dist2Start;

			var npix = 1;

			if (lp->IsVertical)
			{
				do
				{
					_base.Li.Dec();
					_base.Y -= lp->Increment;
					_base.X = (lp->X1 + _base.Li.Y) >> LineAA.LineSubpixelShift;

					if (lp->Increment > 0)
					{
						_di.DecY(_base.X - _base.OldX);
					}
					else
					{
						_di.IncY(_base.X - _base.OldX);
					}

					_base.OldX = _base.X;

					dist1Start = dist2Start = _di.DistStart;

					var dx = 0;
					if (dist1Start < 0) ++npix;
					do
					{
						dist1Start += _di.DyStart;
						dist2Start -= _di.DyStart;
						if (dist1Start < 0) ++npix;
						if (dist2Start < 0) ++npix;
						++dx;
					}
					while (_base.Dist[dx] <= _base.Width);
					--_base.Step;
					if (npix == 0) break;
					npix = 0;
				}
				while (_base.Step >= -_base.MaxExtent);
			}
			else
			{
				do
				{
					_base.Li.Dec();
					_base.X -= lp->Increment;
					_base.Y = (lp->Y1 + _base.Li.Y) >> LineAA.LineSubpixelShift;

					if (lp->Increment > 0)
					{
						_di.DecX(_base.Y - _base.OldY);
					}
					else
					{
						_di.IncX(_base.Y - _base.OldY);
					}

					_base.OldY = _base.Y;

					dist1Start = dist2Start = _di.DistStart;

					var dy = 0;
					if (dist1Start < 0) ++npix;
					do
					{
						dist1Start -= _di.DxStart;
						dist2Start += _di.DxStart;
						if (dist1Start < 0) ++npix;
						if (dist2Start < 0) ++npix;
						++dy;
					}
					while (_base.Dist[dy] <= _base.Width);
					--_base.Step;
					if (npix == 0) break;
					npix = 0;
				}
				while (_base.Step >= -_base.MaxExtent);
			}
			_base.Li.AdjustForward();
		}

		public bool StepHor()
		{
			int dist;
			var s1 = _base.StepHorBase(ref _di);

			var distStart = _di.DistStart;
			var p0 = _base.Covers + LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			*p1 = 0;
			if (distStart <= 0)
			{
				*p1 = _base.Renderer->Cover(s1);
			}
			++p1;

			var dy = 1;
			while ((dist = _base.Dist[dy] - s1) <= _base.Width)
			{
				distStart -= _di.DxStart;
				*p1 = 0;
				if (distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(dist);
				}
				++p1;
				++dy;
			}

			dy = 1;
			distStart = _di.DistStart;
			while ((dist = _base.Dist[dy] + s1) <= _base.Width)
			{
				distStart += _di.DxStart;
				*--p0 = 0;
				if (distStart <= 0)
				{
					*p0 = _base.Renderer->Cover(dist);
				}
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

			var distStart = _di.DistStart;

			*p1 = 0;
			if (distStart <= 0)
			{
				*p1 = _base.Renderer->Cover(s1);
			}
			++p1;

			var dx = 1;
			while ((dist = _base.Dist[dx] - s1) <= _base.Width)
			{
				distStart += _di.DyStart;
				*p1 = 0;
				if (distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(dist);
				}
				++p1;
				++dx;
			}

			dx = 1;
			distStart = _di.DistStart;
			while ((dist = _base.Dist[dx] + s1) <= _base.Width)
			{
				distStart -= _di.DyStart;
				*--p0 = 0;
				if (distStart <= 0)
				{
					*p0 = _base.Renderer->Cover(dist);
				}
				++dx;
			}
			_base.Renderer->BlendSolidHSpan(_base.X - dx + 1, _base.Y, (int)(p1 - p0), p0);
			return ++_base.Step < _base.Count;
		}
	}
}