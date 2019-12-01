namespace Haapps.Gfx.Agg
{
	public interface IRasterizerConverter64 : IRasterizerConverter
	{
		double MulDiv(double a, double b, double c);
		int Xi(double v);
		int Yi(double v);
		double Upscale(double v);
		double Downscale(int v);
	}
}