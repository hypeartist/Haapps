namespace Haapps.Gfx.Agg
{
	public struct RasConv64 : IRasterizerConverter64
	{
		public double MulDiv(double a, double b, double c) => a * b / c;

		public int Xi(double v) => (int) (v * Common.PolySubpixelScale);

		public int Yi(double v) => (int) (v * Common.PolySubpixelScale);

		public double Upscale(double v) => v;

		public double Downscale(int v) => v / (double) Common.PolySubpixelScale;
	}
}