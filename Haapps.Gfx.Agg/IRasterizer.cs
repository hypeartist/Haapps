using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public interface IRasterizer
	{
		void AddPath(VertexSourceAbstract vs, int pathId = 0);
		void AddVertex(double x, double y, PathCommand cmd);
		void MoveTo(int x, int y);
		void LineTo(int x, int y);
		void MoveToD(double x, double y);
		void LineToD(double x, double y);
	}
}