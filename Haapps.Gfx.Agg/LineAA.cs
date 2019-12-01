namespace Haapps.Gfx.Agg
{
	public static class LineAA
	{
		public const int LineSubpixelShift = 8;
		public const int LineSubpixelShiftMul2 = LineSubpixelShift << 1;
		public const int LineSubpixelScale = 1 << LineSubpixelShift;
		public const int LineSubpixelScaleDiv2 = LineSubpixelScale >> 1;
		public const int LineSubpixelScaleMul2 = LineSubpixelScale << 1;
		public const int LineSubpixelMask = LineSubpixelScale - 1;
		public const int LineMaxCoord = (1 << 28) - 1;
		public const int LineMaxLength = 1 << (LineSubpixelShift + 10);

		public const int LineMrSubpixelShift = 4;
		public const int LineMrSubpixelScale = 1;
		public const int LineMrSubpixelMask = LineMrSubpixelScale - 1;

		public static int LineMr(int x) => x >> (LineSubpixelShift - LineMrSubpixelShift);

		public static int LineHr(int x) => x << (LineSubpixelShift - LineMrSubpixelShift);

		public static int LineDblHr(int x) => x << LineSubpixelShift;

		public static int LineCoordSatConv(double x)
		{
			x *= LineSubpixelScale;
			if (x < -LineMaxCoord) return -LineMaxCoord;
			return x > LineMaxCoord ? LineMaxCoord : Common.RoundToI32(x);
		}

		public static void FixDegenerateBisectrixStart(ref LineParameters lp, ref int x, ref int y)
		{
			var d = Common.RoundToI32(((double)(x - lp.X2) * (lp.Y2 - lp.Y1) - (double)(y - lp.Y2) * (lp.X2 - lp.X1)) / lp.Length);
			if (d >= LineSubpixelScale >> 1) return;
			x = lp.X1 + (lp.Y2 - lp.Y1);
			y = lp.Y1 - (lp.X2 - lp.X1);
		}

		public static void FixDegenerateBisectrixEnd(ref LineParameters lp, ref int x, ref int y)
		{
			var d = Common.RoundToI32(((double)(x - lp.X2) * (lp.Y2 - lp.Y1) - (double)(y - lp.Y2) * (lp.X2 - lp.X1)) / lp.Length);
			if (d >= LineSubpixelScale >> 1) return;
			x = lp.X2 + (lp.Y2 - lp.Y1);
			y = lp.Y2 - (lp.X2 - lp.X1);
		}
	}
}