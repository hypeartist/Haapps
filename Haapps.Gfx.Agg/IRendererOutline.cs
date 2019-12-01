namespace Haapps.Gfx.Agg
{
	public interface IRendererOutline
	{
		bool AccurateJoinOnly { get; }
		void Pie(int xc, int yc, int x1, int y1, int x2, int y2);
		void Line0(ref LineParameters lp);
		void Line1(ref LineParameters lp, int x, int y);
		void Line2(ref LineParameters lp, int x, int y);
		void Line3(ref LineParameters lp, int sx, int sy, int ex, int ey);
		void Semidot(RendererOutlineCmp cmp, int xc1, int yc1, int xc2, int yc2);
	}
}