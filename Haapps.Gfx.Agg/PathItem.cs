namespace Haapps.Gfx.Agg
{
	public struct PathItem
	{
		public static PathItem Stop = new PathItem(0, 0, PathCommand.Stop);

		public static readonly PathItem EndPolyCloseCcw = new PathItem(0, 0, PathCommand.EndPoly | (PathCommand) (PathFlags.Close | PathFlags.Ccw));

		public static readonly PathItem EndPolyCloseCw = new PathItem(0, 0, PathCommand.EndPoly | (PathCommand) (PathFlags.Close | PathFlags.Cw));
		
		public static readonly PathItem EndPolyClose = new PathItem(0, 0, PathCommand.EndPoly | (PathCommand) (PathFlags.Close));

		public double X;
		public double Y;
		public PathCommand Command;

		public PathItem(double x, double y, PathCommand command)
		{
			X = x;
			Y = y;
			Command = command;
		}

		public readonly PathCommand Decompose(out double x, out double y)
		{
			x = X;
			y = Y;
			return Command;
		}
#if DEBUG
		public override string ToString()
		{
			return $"{X:F2}, {Y:F2} => {Command} ({(int)Command})";
		}
#endif
	}
}