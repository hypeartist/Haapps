namespace Haapps.Gfx.Agg
{
	public sealed class NullMarkers : MarkersGeneratorAbstract
	{
		public override void RemoveAll()
		{
		}

		public override void AddVertex(double x, double y, PathCommand cmd)
		{
		}

		public override void Rewind(int pathId = 0)
		{
		}

		public override PathCommand Vertex(ref double x, ref double y) => PathCommand.Stop;

		public override void Dispose()
		{
		}
	}
}