namespace Haapps.Gfx.Agg
{
	public struct VertexDistanceCommand
	{
		private const double Dd = 1.0 / Common.VertexDistanceEpsilon;

		public double X;
		public double Y;
		public double Distance;
		public PathCommand Command;

		public VertexDistanceCommand(double x, double y, PathCommand command) : this()
		{
			X = x;
			Y = y;
			Distance = 0;
			Command = command;
		}

		public VertexDistanceCommand(double x, double y, double dist, PathCommand command) : this()
		{
			X = x;
			Y = y;
			Distance = dist;
			Command = command;
		}

		public bool Measure(ref VertexDistanceCommand vd)
		{
			var ret = (Distance = Common.CalcDistance(X, Y, vd.X, vd.Y)) > Common.VertexDistanceEpsilon;
			if (!ret)
			{
				Distance = Dd;
			}

			return ret;
		}
	}
}