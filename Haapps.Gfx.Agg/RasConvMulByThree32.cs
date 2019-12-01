namespace Haapps.Gfx.Agg
{
	public struct RasConvMulByThree32 : IRasterizerConverter32
	{
		private const int PolySubpixelShift = 8;
		private const int PolySubpixelScale = 1 << PolySubpixelShift;

		public int MulDiv(double a, double b, double c) => Common.RoundToI32(a * b / c);

		public int Xi(int v) => v * 3;

		public int Yi(int v) => v;

		public int Upscale(double v) => Common.RoundToI32(v * PolySubpixelScale);

		public int Downscale(int v) => v;
	}
}