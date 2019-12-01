namespace Haapps.Gfx.Agg
{
	public struct RasConvSat32 : IRasterizerConverter32
	{
		private const int PolyMaxCoord = (1 << 30) - 1;
		private const int PolySubpixelShift = 8;
		private const int PolySubpixelScale = 1 << PolySubpixelShift;

		public int MulDiv(double a, double b, double c)
		{
			var v = (a*b/c);
			if (v < -PolyMaxCoord) return -PolyMaxCoord;
			return v > PolyMaxCoord ? PolyMaxCoord : Common.RoundToI32(v);
		}

		public int Xi(int v) => v;

		public int Yi(int v) => v;

		public int Upscale(double v)
		{
			v *= PolySubpixelScale;
			if (v < -PolyMaxCoord) return -PolyMaxCoord;
			return v > PolyMaxCoord ? PolyMaxCoord : Common.RoundToI32(v);
		}

		public int Downscale(int v) => v;
	}
}