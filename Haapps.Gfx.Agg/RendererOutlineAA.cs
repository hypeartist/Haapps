using System;
using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererOutlineAA<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private struct DistanceInterpolator0
		{
			private readonly int _dy;

			public DistanceInterpolator0(int x1, int y1, int x2, int y2, int x, int y)
			{
				var dx = LineAA.LineMr(x2) - LineAA.LineMr(x1);
				_dy = LineAA.LineMr(y2) - LineAA.LineMr(y1);
				Dist = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(x2)) * _dy - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(y2)) * dx;
				_dy <<= LineAA.LineMrSubpixelShift;
			}

			public void IncX() => Dist += _dy;

			public int Dist { get; private set; }
		}

		private struct DistanceInterpolator00
		{
			private readonly int _dy1;
			private readonly int _dy2;

			public DistanceInterpolator00(int xc, int yc, int x1, int y1, int x2, int y2, int x, int y)
			{
				var dx1 = LineAA.LineMr(x1) - LineAA.LineMr(xc);
				_dy1 = LineAA.LineMr(y1) - LineAA.LineMr(yc);
				var dx2 = LineAA.LineMr(x2) - LineAA.LineMr(xc);
				_dy2 = LineAA.LineMr(y2) - LineAA.LineMr(yc);
				Dist1 = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(x1)) * _dy1 - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(y1)) * dx1;
				Dist2 = (LineAA.LineMr(x + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(x2)) * _dy2 - (LineAA.LineMr(y + LineAA.LineSubpixelScaleDiv2) - LineAA.LineMr(y2)) * dx2;
				_dy1 <<= LineAA.LineMrSubpixelShift;
				_dy2 <<= LineAA.LineMrSubpixelShift;
			}

			public void IncX()
			{
				Dist1 += _dy1;
				Dist2 += _dy2;
			}

			public int Dist1 { get; private set; }

			public int Dist2 { get; private set; }
		}

		private LineProfileAA* _lineProfile;
		private TRendererBase* _ren;
		private Rectangle32 _clipBox;
		private bool _isClipping;
		private LineInterpolatorAA0<TColor, TRendererBase> _li0;
		private LineInterpolatorAA1<TColor, TRendererBase> _li1;
		private LineInterpolatorAA2<TColor, TRendererBase> _li2;
		private LineInterpolatorAA3<TColor, TRendererBase> _li3;

		public RendererOutlineAA(ref TRendererBase ren, ref LineProfileAA profile) : this()
		{
			_ren = (TRendererBase*) Unsafe.AsPointer(ref ren);
			_lineProfile = (LineProfileAA*) Unsafe.AsPointer(ref profile);
			_clipBox = new Rectangle32(0, 0, 0, 0);
		}

		public byte Cover(int d) => Profile.Value(d);

		public int SubPixelWidth => Profile.SubpixelWidth;

		public TColor Color { get; set; }

		public ref LineProfileAA Profile => ref Unsafe.AsRef<LineProfileAA>(_lineProfile);

		public void BlendSolidHSpan(int x, int y, int length, byte* covers) => _ren->BlendSolidHSpan(x, y, length, Color, covers);

		public void BlendSolidVSpan(int x, int y, int length, byte* covers) => _ren->BlendSolidVSpan(x, y, length, Color, covers);

		public bool AccurateJoinOnly => false;

		private void PieHLine(int xc, int yc, int xp1, int yp1, int xp2, int yp2, int xh1, int yh1, int xh2)
		{
			if (_isClipping && (((xc > _clipBox.X2) ? 1 : 0) | (((yc > _clipBox.Y2) ? 1 : 0) << 1) | (((xc < _clipBox.X1) ? 1 : 0) << 2) | (((yc < _clipBox.Y1) ? 1 : 0) << 3)) != 0) return;

			var covers = stackalloc byte[LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidthMul2 + 4];
			var p0 = covers;
			var p1 = covers;
			var x = xh1 << LineAA.LineSubpixelShift;
			var y = yh1 << LineAA.LineSubpixelShift;
			var w = SubPixelWidth;

			var di = new DistanceInterpolator00(xc, yc, xp1, yp1, xp2, yp2, x, y);
			x += LineAA.LineSubpixelScaleDiv2;
			y += LineAA.LineSubpixelScaleDiv2;

			var xh0 = xh1;
			var dx = x - xc;
			var dy = y - yc;
			do
			{
				var d = Common.FastSqrt(dx * dx + dy * dy);
				*p1 = 0;
				if (di.Dist1 <= 0 && di.Dist2 > 0 && d <= w)
				{
					*p1 = Cover(d);
				}
				++p1;
				dx += LineAA.LineSubpixelScale;
				di.IncX();
			} while (++xh1 <= xh2);
			_ren->BlendSolidHSpan(xh0, yh1, (int)(p1 - p0), Color, p0);
		}

		public void Pie(int xc, int yc, int x1, int y1, int x2, int y2)
		{
			var r = ((SubPixelWidth + LineAA.LineSubpixelMask) >> LineAA.LineSubpixelShift);
			if (r < 1) r = 1;
			var interpolator = new BresenhamEllipseInterpolator(r, r);
			var dx = 0;
			var dy = -r;
			var dy0 = dy;
			var dx0 = dx;
			var x = xc >> LineAA.LineSubpixelShift;
			var y = yc >> LineAA.LineSubpixelShift;

			do
			{
				dx += interpolator.DX;
				dy += interpolator.DY;

				if (dy != dy0)
				{
					PieHLine(xc, yc, x1, y1, x2, y2, x - dx0, y + dy0, x + dx0);
					PieHLine(xc, yc, x1, y1, x2, y2, x - dx0, y - dy0, x + dx0);
				}
				dx0 = dx;
				dy0 = dy;
				interpolator.Inc();
			} while (dy < 0);
			PieHLine(xc, yc, x1, y1, x2, y2, x - dx0, y + dy0, x + dx0);
		}

		private void Line0NoClip(ref LineParameters lp)
		{
			while (true)
			{
				if (lp.Length > LineAA.LineMaxLength)
				{
					lp.Divide(out var lp1, out var lp2);
					Line0NoClip(ref lp1);
					lp = lp2;
					continue;
				}

				_li0.Init(ref this, (LineParameters*)Unsafe.AsPointer(ref lp));
				if (_li0.Count == 0) return;
				if (_li0.IsVertical)
				{
					while (_li0.StepVer())
					{
					}
				}
				else
				{
					while (_li0.StepHor())
					{
					}
				}
				break;
			}
		}

		public void Line0(ref LineParameters lp)
		{
			if (_isClipping)
			{
				var x1 = lp.X1;
				var y1 = lp.Y1;
				var x2 = lp.X2;
				var y2 = lp.Y2;
				var flags = ClipLiangBarsky.ClipLineSegment(ref x1, ref y1, ref x2, ref y2, ref _clipBox);
				if ((flags & 4) != 0) return;
				if (flags != 0)
				{
					var lp2 = new LineParameters(x1, y1, x2, y2, Common.RoundToU32(Common.CalcDistance(x1, y1, x2, y2)));
					Line0NoClip(ref lp2);
				}
				else
				{
					Line0NoClip(ref lp);
				}
			}
			else
			{
				Line0NoClip(ref lp);
			}
		}

		private void Line1NoClip(ref LineParameters lp, int sx, int sy)
		{
			while (true)
			{
				if (lp.Length > LineAA.LineMaxLength)
				{
					lp.Divide(out var lp1, out var lp2);
					Line1NoClip(ref lp1, (lp.X1 + sx) >> 1, (lp.Y1 + sy) >> 1);
					Line1NoClip(ref lp2, lp1.X2 + (lp1.Y2 - lp1.Y1), lp1.Y2 - (lp1.X2 - lp1.X1));
					continue;
				}

				LineAA.FixDegenerateBisectrixStart(ref lp, ref sx, ref sy);
				_li1.Init(ref this, (LineParameters*)Unsafe.AsPointer(ref lp), sx, sy);
				if (_li1.IsVertical)
				{
					while (_li1.StepVer())
					{
					}
				}
				else
				{
					while (_li1.StepHor())
					{
					}
				}
				break;
			}
		}

		public void Line1(ref LineParameters lp, int sx, int sy)
		{
			if (_isClipping)
			{
				var x1 = lp.X1;
				var y1 = lp.Y1;
				var x2 = lp.X2;
				var y2 = lp.Y2;
				var flags = ClipLiangBarsky.ClipLineSegment(ref x1, ref y1, ref x2, ref y2, ref _clipBox);
				if ((flags & 4) != 0) return;
				if (flags != 0)
				{
					var lp2 = new LineParameters(x1, y1, x2, y2, Common.RoundToU32(Common.CalcDistance(x1, y1, x2, y2)));
					if ((flags & 1) != 0)
					{
						sx = x1 + (y2 - y1);
						sy = y1 - (x2 - x1);
					}
					else
					{
						while (Math.Abs(sx - lp.X1) + Math.Abs(sy - lp.Y1) > lp2.Length)
						{
							sx = (lp.X1 + sx) >> 1;
							sy = (lp.Y1 + sy) >> 1;
						}
					}
					Line1NoClip(ref lp2, sx, sy);
				}
				else
				{
					Line1NoClip(ref lp, sx, sy);
				}
			}
			else
			{
				Line1NoClip(ref lp, sx, sy);
			}
		}

		private void Line2NoClip(ref LineParameters lp, int ex, int ey)
		{
			while (true)
			{
				if (lp.Length > LineAA.LineMaxLength)
				{
					lp.Divide(out var lp1, out var lp2);
					Line2NoClip(ref lp1, lp1.X2 + (lp1.Y2 - lp1.Y1), lp1.Y2 - (lp1.X2 - lp1.X1));
					Line2NoClip(ref lp2, (lp.X2 + ex) >> 1, (lp.Y2 + ey) >> 1);
					continue;
				}

				LineAA.FixDegenerateBisectrixEnd(ref lp, ref ex, ref ey);
				_li2.Init(ref this, (LineParameters*)Unsafe.AsPointer(ref lp), ex, ey);
				if (_li2.IsVertical)
				{
					while (_li2.StepVer())
					{
					}
				}
				else
				{
					while (_li2.StepHor())
					{
					}
				}
				break;
			}
		}

		public void Line2(ref LineParameters lp, int ex, int ey)
		{
			if (_isClipping)
			{
				var x1 = lp.X1;
				var y1 = lp.Y1;
				var x2 = lp.X2;
				var y2 = lp.Y2;
				var flags = ClipLiangBarsky.ClipLineSegment(ref x1, ref y1, ref x2, ref y2, ref _clipBox);
				if ((flags & 4) != 0) return;
				if (flags != 0)
				{
					var lp2 = new LineParameters(x1, y1, x2, y2, Common.RoundToU32(Common.CalcDistance(x1, y1, x2, y2)));
					if ((flags & 2) != 0)
					{
						ex = x2 + (y2 - y1);
						ey = y2 - (x2 - x1);
					}
					else
					{
						while (Math.Abs(ex - lp.X2) + Math.Abs(ey - lp.Y2) > lp2.Length)
						{
							ex = (lp.X2 + ex) >> 1;
							ey = (lp.Y2 + ey) >> 1;
						}
					}
					Line2NoClip(ref lp2, ex, ey);
				}
				else
				{
					Line2NoClip(ref lp, ex, ey);
				}
			}
			else
			{
				Line2NoClip(ref lp, ex, ey);
			}
		}

		private void Line3NoClip(ref LineParameters lp, int sx, int sy, int ex, int ey)
		{
			while (true)
			{
				if (lp.Length > LineAA.LineMaxLength)
				{
					lp.Divide(out var lp1, out var lp2);
					var mx = lp1.X2 + (lp1.Y2 - lp1.Y1);
					var my = lp1.Y2 - (lp1.X2 - lp1.X1);
					Line3NoClip(ref lp1, (lp.X1 + sx) >> 1, (lp.Y1 + sy) >> 1, mx, my);
					var lp3 = lp;
					lp = lp2;
					sx = mx;
					sy = my;
					ex = (lp3.X2 + ex) >> 1;
					ey = (lp3.Y2 + ey) >> 1;
					continue;
				}

				LineAA.FixDegenerateBisectrixStart(ref lp, ref sx, ref sy);
				LineAA.FixDegenerateBisectrixEnd(ref lp, ref ex, ref ey);
				_li3.Init(ref this, (LineParameters*)Unsafe.AsPointer(ref lp), sx, sy, ex, ey);
				if (_li3.IsVertical)
				{
					while (_li3.StepVer())
					{
					}
				}
				else
				{
					while (_li3.StepHor())
					{
					}
				}
				break;
			}
		}

		public void Line3(ref LineParameters lp, int sx, int sy, int ex, int ey)
		{
			if (_isClipping)
			{
				var x1 = lp.X1;
				var y1 = lp.Y1;
				var x2 = lp.X2;
				var y2 = lp.Y2;
				var flags = ClipLiangBarsky.ClipLineSegment(ref x1, ref y1, ref x2, ref y2, ref _clipBox);
				if ((flags & 4) != 0) return;
				if (flags != 0)
				{
					var lp2 = new LineParameters(x1, y1, x2, y2, Common.RoundToU32(Common.CalcDistance(x1, y1, x2, y2)));
					if ((flags & 1) != 0)
					{
						sx = x1 + (y2 - y1);
						sy = y1 - (x2 - x1);
					}
					else
					{
						while (Math.Abs(sx - lp.X1) + Math.Abs(sy - lp.Y1) > lp2.Length)
						{
							sx = (lp.X1 + sx) >> 1;
							sy = (lp.Y1 + sy) >> 1;
						}
					}
					if ((flags & 2) != 0)
					{
						ex = x2 + (y2 - y1);
						ey = y2 - (x2 - x1);
					}
					else
					{
						while (Math.Abs(ex - lp.X2) + Math.Abs(ey - lp.Y2) > lp2.Length)
						{
							ex = (lp.X2 + ex) >> 1;
							ey = (lp.Y2 + ey) >> 1;
						}
					}
					Line3NoClip(ref lp2, sx, sy, ex, ey);
				}
				else
				{
					Line3NoClip(ref lp, sx, sy, ex, ey);
				}
			}
			else
			{
				Line3NoClip(ref lp, sx, sy, ex, ey);
			}
		}

		private void SemidotHLine(RendererOutlineCmp cmp, int xc1, int yc1, int xc2, int yc2, int x1, int y1, int x2)
		{
			var covers = stackalloc byte[LineInterpolatorAABase<TColor, TRendererBase>.MaxHalfWidthMul2 + 4];
			var p0 = covers;
			var p1 = covers;
			var x = x1 << LineAA.LineSubpixelShift;
			var y = y1 << LineAA.LineSubpixelShift;
			var w = SubPixelWidth;
			var di = new DistanceInterpolator0(xc1, yc1, xc2, yc2, x, y);
			x += LineAA.LineSubpixelScaleDiv2;
			y += LineAA.LineSubpixelScaleDiv2;

			var x0 = x1;
			var dx = x - xc1;
			var dy = y - yc1;
			if (cmp == RendererOutlineCmp.DistStart)
			{
				do
				{
					var d = Common.FastSqrt(dx * dx + dy * dy);
					*p1 = 0;
					if (di.Dist > 0 && d <= w)
					{
						*p1 = Cover(d);
					}
					++p1;
					dx += LineAA.LineSubpixelScale;
					di.IncX();
				} while (++x1 <= x2);
			}
			else
			{
				do
				{
					var d = (int)(Math.Sqrt(dx * dx + dy * dy));
					*p1 = 0;
					if (di.Dist <= 0 && d <= w)
					{
						*p1 = Cover(d);
					}
					++p1;
					dx += LineAA.LineSubpixelScale;
					di.IncX();
				} while (++x1 <= x2);
			}

			_ren->BlendSolidHSpan(x0, y1, (int)(p1 - p0), Color, p0);
		}

		public void Semidot(RendererOutlineCmp cmp, int xc1, int yc1, int xc2, int yc2)
		{
			if (_isClipping && ((xc1 > _clipBox.X2 ? 1 : 0) | ((yc1 > _clipBox.Y2 ? 1 : 0) << 1) | ((xc1 < _clipBox.X1 ? 1 : 0) << 2) | ((yc1 < _clipBox.Y1 ? 1 : 0) << 3)) != 0) return;

			var r = ((SubPixelWidth + LineAA.LineSubpixelMask) >> LineAA.LineSubpixelShift);
			if (r < 1)
			{
				r = 1;
			}
			var interpolator = new BresenhamEllipseInterpolator(r, r);
			var dx = 0;
			var dy = -r;
			var dy0 = dy;
			var dx0 = dx;
			var x = xc1 >> LineAA.LineSubpixelShift;
			var y = yc1 >> LineAA.LineSubpixelShift;

			do
			{
				dx += interpolator.DX;
				dy += interpolator.DY;

				if (dy != dy0)
				{
					SemidotHLine(cmp, xc1, yc1, xc2, yc2, x - dx0, y + dy0, x + dx0);
					SemidotHLine(cmp, xc1, yc1, xc2, yc2, x - dx0, y - dy0, x + dx0);
				}
				dx0 = dx;
				dy0 = dy;
				interpolator.Inc();
			} while (dy < 0);
			SemidotHLine(cmp, xc1, yc1, xc2, yc2, x - dx0, y + dy0, x + dx0);
		}
	}
}