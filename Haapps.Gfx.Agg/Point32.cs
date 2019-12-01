namespace Haapps.Gfx.Agg
{
	public struct Point32
	{
		public int X;
		public int Y;

		public Point32(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return string.Format("X={0}, Y={1}", X, Y);
		}
	}
}
