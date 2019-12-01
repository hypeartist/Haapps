namespace Haapps.Gfx.Agg
{
	public interface IDistanceInterpolator
	{
		void IncX(int dy);
		void DecX(int dy);
		void IncY(int dy);
		void DecY(int dy);
		int Dist { get; }
	}
}