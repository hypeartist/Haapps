namespace Haapps.Gfx.Agg
{
	public abstract class VertexGeneratorAbstract : VertexSourceAbstract
	{
		public abstract void AddVertex(double x, double y, PathCommand cmd);
		public abstract void RemoveAll();
	}
}