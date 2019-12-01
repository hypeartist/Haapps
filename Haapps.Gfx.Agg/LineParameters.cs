using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct LineParameters
	{
		[StructLayout(LayoutKind.Explicit, Size = 32)]
		private struct I32x8
		{
		}

		private I32x8 _orthQuadrant;
		private I32x8 _diagQuadrant;

		public int Octant;
		public int Y1;
		public int Y2;
		public int Sx;
		public int Sy;
		public int Length;
		public int Dx;
		public int Dy;
		public int X2;
		public int X1;
		public int Increment;
		public bool IsVertical;

		public LineParameters(int x1, int y1, int x2, int y2, int length) : this()
		{
			var i = (int*)Unsafe.AsPointer(ref _orthQuadrant);
			i[0] = 0;
			i[1] = 0;
			i[2] = 1;
			i[3] = 1;
			i[4] = 3;
			i[5] = 3;
			i[6] = 2;
			i[7] = 2;

			i = (int*)Unsafe.AsPointer(ref _diagQuadrant);
			i[0] = 0;
			i[1] = 1;
			i[2] = 2;
			i[3] = 1;
			i[4] = 0;
			i[5] = 3;
			i[6] = 2;
			i[7] = 3;

			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
			Length = length;
			Dx = Math.Abs(x2 - x1);
			Dy = Math.Abs(y2 - y1);
			Sx = (x2 > x1) ? 1 : -1;
			Sy = (y2 > y1) ? 1 : -1;
			IsVertical = Dy >= Dx;
			Increment = IsVertical ? Sy : Sx;
			Octant = (Sy & 4) | (Sx & 2) | (IsVertical ? 1 : 0);
		}

		public int OrthQuadrant => ((int*)Unsafe.AsPointer(ref _orthQuadrant))[Octant];

		public int DiagQuadrant => ((int*)Unsafe.AsPointer(ref _diagQuadrant))[Octant];

		public bool SameOrthogonalQuadrant(ref LineParameters lp) => OrthQuadrant == ((int*)Unsafe.AsPointer(ref _orthQuadrant))[lp.Octant];

		public bool SameDiagonalQuadrant(ref LineParameters lp) => DiagQuadrant == ((int*)Unsafe.AsPointer(ref _diagQuadrant))[lp.Octant];

		public void Divide(out LineParameters lp1, out LineParameters lp2)
		{
			var xmid = (X1 + X2) >> 1;
			var ymid = (Y1 + Y2) >> 1;
			var len2 = Length >> 1;

			lp1 = this;
			lp2 = this;

			lp1.X2 = xmid;
			lp1.Y2 = ymid;
			lp1.Length = len2;
			lp1.Dx = Math.Abs(lp1.X2 - lp1.X1);
			lp1.Dy = Math.Abs(lp1.Y2 - lp1.Y1);

			lp2.X1 = xmid;
			lp2.Y1 = ymid;
			lp2.Length = len2;
			lp2.Dx = Math.Abs(lp2.X2 - lp2.X1);
			lp2.Dy = Math.Abs(lp2.Y2 - lp2.Y1);
		}

		public static void Bisectrix(ref LineParameters l1, ref LineParameters l2, out int x, out int y)
		{
			var k = l2.Length / (double)l1.Length;
			var tx = l2.X2 - (l2.X1 - l1.X1) * k;
			var ty = l2.Y2 - (l2.Y1 - l1.Y1) * k;

			if ((double)(l2.X2 - l2.X1) * (l2.Y1 - l1.Y1) < (double)(l2.Y2 - l2.Y1) * (l2.X1 - l1.X1) + 100.0)
			{
				tx -= (tx - l2.X1) * 2.0;
				ty -= (ty - l2.Y1) * 2.0;
			}

			var ddx = tx - l2.X1;
			var ddy = ty - l2.Y1;
			if ((int)Math.Sqrt(ddx * ddx + ddy * ddy) < LineAA.LineSubpixelScale)
			{
				x = (l2.X1 + l2.X1 + (l2.Y1 - l1.Y1) + (l2.Y2 - l2.Y1)) >> 1;
				y = (l2.Y1 + l2.Y1 - (l2.X1 - l1.X1) - (l2.X2 - l2.X1)) >> 1;
				return;
			}

			x = Common.RoundToI32(tx);
			y = Common.RoundToI32(ty);
		}

		public static void FixDegenerateBisectrixStart(ref LineParameters lp, ref int x, ref int y)
		{
			var d = Common.RoundToI32((((double)(x - lp.X2) * (lp.Y2 - lp.Y1) - (double)(y - lp.Y2) * (lp.X2 - lp.X1)) / lp.Length));
			if (d >= LineAA.LineSubpixelScaleDiv2) return;
			x = lp.X1 + (lp.Y2 - lp.Y1);
			y = lp.Y1 - (lp.X2 - lp.X1);
		}

		public static void FixDegenerateBisectrixEnd(ref LineParameters lp, ref int x, ref int y)
		{
			var d = Common.RoundToI32((((double)(x - lp.X2) * (lp.Y2 - lp.Y1) - (double)(y - lp.Y2) * (lp.X2 - lp.X1)) / lp.Length));
			if (d >= LineAA.LineSubpixelScaleDiv2) return;
			x = lp.X2 + (lp.Y2 - lp.Y1);
			y = lp.Y2 - (lp.X2 - lp.X1);
		}
	}
}