namespace Haapps.Gfx.Agg
{
	internal unsafe struct LineInterpolatorAA3<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private struct DistanceInterpolator3 : IDistanceInterpolator
		{
			private readonly int _dx;
			private readonly int _dy;

			public readonly int DxStart;
			public readonly int DyStart;
			public readonly int DxEnd;
			public readonly int DyEnd;
			public int DistStart;
			public int DistEnd;

			public DistanceInterpolator3(int x1, int y1, int x2, int y2, int sx, int sy, int ex, int ey, int x, int y)
			{
				_dx = x2 - x1;
				_dy = y2 - y1;
				DxStart = LineAA.LineMr(sx) - LineAA.LineMr(x1);
				DyStart = LineAA.LineMr(sy) - LineAA.LineMr(y1);
				DxEnd = LineAA.LineMr(ex) - LineAA.LineMr(x2);
				DyEnd = LineAA.LineMr(ey) - LineAA.LineMr(y2);

				Dist = Common.RoundToI32((double)(x + LineAA.LineSubpixelScaleDiv2 - x2) * _dy - (double)(y + LineAA.LineSubpixelScaleDiv2 - y2) * _dx);
				DistStart = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(sx)) * DyStart - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(sy)) * DxStart;
				DistEnd = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(ex)) * DyEnd - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(ey)) * DxEnd;

				_dx <<= LineAA.LineSubpixelShift;
				_dy <<= LineAA.LineSubpixelShift;
				DxStart <<= LineAA.LineMrSubpixelShift;
				DyStart <<= LineAA.LineMrSubpixelShift;
				DxEnd <<= LineAA.LineMrSubpixelShift;
				DyEnd <<= LineAA.LineMrSubpixelShift;
			}

			public int Dist { get; private set; }

			public void IncX(int dy)
			{
				Dist += _dy;
				DistStart += DyStart;
				DistEnd += DyEnd;
				if (dy > 0)
				{
					Dist -= _dx;
					DistStart -= DxStart;
					DistEnd -= DxEnd;
				}

				if (dy >= 0) return;
				Dist += _dx;
				DistStart += DxStart;
				DistEnd += DxEnd;
			}

			public void DecX(int dy)
			{
				Dist -= _dy;
				DistStart -= DyStart;
				DistEnd -= DyEnd;
				if (dy > 0)
				{
					Dist -= _dx;
					DistStart -= DxStart;
					DistEnd -= DxEnd;
				}

				if (dy >= 0) return;
				Dist += _dx;
				DistStart += DxStart;
				DistEnd += DxEnd;
			}

			public void IncY(int dx)
			{
				Dist -= _dx;
				DistStart -= DxStart;
				DistEnd -= DxEnd;
				if (dx > 0)
				{
					Dist += _dy;
					DistStart += DyStart;
					DistEnd += DyEnd;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				DistStart -= DyStart;
				DistEnd -= DyEnd;
			}

			public void DecY(int dx)
			{
				Dist += _dx;
				DistStart += DxStart;
				DistEnd += DxEnd;
				if (dx > 0)
				{
					Dist += _dy;
					DistStart += DyStart;
					DistEnd += DyEnd;
				}

				if (dx >= 0) return;
				Dist -= _dy;
				DistStart -= DyStart;
				DistEnd -= DyEnd;
			}
		}

		private DistanceInterpolator3 _di;
		private LineInterpolatorAABase<TColor, TRendererBase> _base;

		public bool IsVertical => _base.IsVertical;

		public int Width => _base.Width;

		public int Count => _base.Count;

		public void Init(ref RendererOutlineAA<TColor, TRendererBase> ren, LineParameters* lp, int sx, int sy, int ex, int ey)
		{
			_base.Init(ref ren, lp);

			_di = new DistanceInterpolator3(lp->X1, lp->Y1, lp->X2, lp->Y2, sx, sy, ex, ey, lp->X1 & ~LineAA.LineSubpixelMask, lp->Y1 & ~LineAA.LineSubpixelMask);
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
						if (dist1Start < 0)
						{
							++npix;
						}
						if (dist2Start < 0)
						{
							++npix;
						}
						++dx;
					} while (_base.Dist[dx] <= _base.Width);
					if (npix == 0) break;
					npix = 0;
				} while (--_base.Step >= -_base.MaxExtent);
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
						if (dist1Start < 0)
						{
							++npix;
						}
						if (dist2Start < 0)
						{
							++npix;
						}
						++dy;
					} while (_base.Dist[dy] <= _base.Width);
					if (npix == 0) break;
					npix = 0;
				} while (--_base.Step >= -_base.MaxExtent);
			}
			_base.Li.AdjustForward();
			_base.Step -= _base.MaxExtent;
		}

		public bool StepHor()
		{
			int dist;
			var s1 = _base.StepHorBase(ref _di);
			var p0 = _base.Covers + LineInterpolatorAABase < TColor, TRendererBase>.MaxHalfWidth + 2;
			var p1 = p0;

			var distStart = _di.DistStart;
			var distEnd = _di.DistEnd;

			var npix = 0;
			*p1 = 0;
			if (distEnd > 0)
			{
				if (distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(s1);
				}
				++npix;
			}
			++p1;

			var dy = 1;
			while ((dist = _base.Dist[dy] - s1) <= _base.Width)
			{
				distStart -= _di.DxStart;
				distEnd -= _di.DxEnd;
				*p1 = 0;
				if (distEnd > 0 && distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(dist);
					++npix;
				}
				++p1;
				++dy;
			}

			dy = 1;
			distStart = _di.DistStart;
			distEnd = _di.DistEnd;
			while ((dist = _base.Dist[dy] + s1) <= _base.Width)
			{
				distStart += _di.DxStart;
				distEnd += _di.DxEnd;
				*--p0 = 0;
				if (distEnd > 0 && distStart <= 0)
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

			var distStart = _di.DistStart;
			var distEnd = _di.DistEnd;

			var npix = 0;
			*p1 = 0;
			if (distEnd > 0)
			{
				if (distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(s1);
				}
				++npix;
			}
			++p1;

			var dx = 1;
			while ((dist = _base.Dist[dx] - s1) <= _base.Width)
			{
				distStart += _di.DyStart;
				distEnd += _di.DyEnd;
				*p1 = 0;
				if (distEnd > 0 && distStart <= 0)
				{
					*p1 = _base.Renderer->Cover(dist);
					++npix;
				}
				++p1;
				++dx;
			}

			dx = 1;
			distStart = _di.DistStart;
			distEnd = _di.DistEnd;
			while ((dist = _base.Dist[dx] + s1) <= _base.Width)
			{
				distStart -= _di.DyStart;
				distEnd -= _di.DyEnd;
				*--p0 = 0;
				if (distEnd > 0 && distStart <= 0)
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