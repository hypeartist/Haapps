namespace Haapps.Gfx.Agg
{
	public interface IRasterizerConverter32 : IRasterizerConverter
	{
		int MulDiv(double a, double b, double c);
		int Xi(int v);
		int Yi(int v);
		int Upscale(double v);
		int Downscale(int v);
	}
}