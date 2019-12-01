namespace Haapps.Gfx.Agg
{
	public struct RasConvMulByThree64 : IRasterizerConverter64
	{
		private const int PolySubpixelShift = 8;
		private const int PolySubpixelScale = 1 << PolySubpixelShift;

		public double MulDiv(double a, double b, double c) => a * b / c;

		public int Xi(double v) => Common.RoundToI32(v * PolySubpixelScale * 3);

		public int Yi(double v) => Common.RoundToI32(v * PolySubpixelScale);

		public double Upscale(double v) => v;

		public double Downscale(int v) => (double)v / PolySubpixelScale;
	}
}