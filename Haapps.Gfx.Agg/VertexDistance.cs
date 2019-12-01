namespace Haapps.Gfx.Agg
{
	public struct VertexDistance
	{
		private const double Dd = 1.0 / Common.VertexDistanceEpsilon;

		public double X;
		public double Y;
		public double Distance;

		public VertexDistance(double x, double y) : this()
		{
			X = x;
			Y = y;
			Distance = 0;
		}

		public VertexDistance(double x, double y, double distance) : this()
		{
			X = x;
			Y = y;
			Distance = distance;
		}

		public bool Measure(ref VertexDistance vd)
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