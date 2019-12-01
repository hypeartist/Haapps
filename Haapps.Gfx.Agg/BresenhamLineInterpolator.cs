using System;

namespace Haapps.Gfx.Agg
{
	public struct BresenhamLineInterpolator
	{
		public const int SubpixelShift = 8;
		public const int SubpixelScale = 1 << SubpixelShift;

		public static int LineLr(int v) { return v >> SubpixelShift; }

		private DDA2LineInterpolator _interpolator;

		public BresenhamLineInterpolator(int x1, int y1, int x2, int y2):this()
		{
			X1 = LineLr(x1);
			Y1 = LineLr(y1);
			var x2Lr = LineLr(x2);
			var y2Lr = LineLr(y2);
			IsVer = Math.Abs(x2Lr - X1) < Math.Abs(y2Lr - Y1);
			Length = IsVer ? Math.Abs(y2Lr - Y1) : Math.Abs(x2Lr - X1);
			Inc = IsVer ? ((y2 > y1) ? 1 : -1) : ((x2 > x1) ? 1 : -1);
			_interpolator = new DDA2LineInterpolator(IsVer ? x1 : y1, IsVer ? x2 : y2, Length);
		}

		public void HStep()
		{
			_interpolator.Inc();
			X1 += Inc;
		}

		public void VStep()
		{
			_interpolator.Inc();
			Y1 += Inc;
		}
				
		public bool IsVer { get; private set; }

		public int Length { get; private set; }

		public int Inc { get; private set; }

		public int X1 { get; private set; }

		public int Y1 { get; private set; }

		public int X2 => LineLr(_interpolator.Y);

		public int Y2 => LineLr(_interpolator.Y);

		public int X2Hr => _interpolator.Y;

		public int Y2Hr => _interpolator.Y;
	}
}