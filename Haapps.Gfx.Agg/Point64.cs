namespace Haapps.Gfx.Agg
{
	public struct Point64
	{
		public double X;
		public double Y;

		public Point64(double x, double y) : this()
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return string.Format("X={0:F}, Y={1:F}", X, Y);
		}
	}
}