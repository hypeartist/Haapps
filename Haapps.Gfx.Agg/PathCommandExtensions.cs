namespace Haapps.Gfx.Agg
{
	public static class PathCommandExtensions
	{
		public static bool Vertex(this PathCommand c) => c >= PathCommand.MoveTo && c < PathCommand.EndPoly;

		public static bool Drawing(this PathCommand c) => c >= PathCommand.LineTo && c < PathCommand.EndPoly;

		public static bool Stop(this PathCommand c) => c == PathCommand.Stop;

		public static bool MoveTo(this PathCommand c) => c == PathCommand.MoveTo;

		public static bool LineTo(this PathCommand c) => c == PathCommand.LineTo;

		public static bool Curve(this PathCommand c) => c == PathCommand.Curve3 || c == PathCommand.Curve4;

		public static bool Curve3(this PathCommand c) => c == PathCommand.Curve3;

		public static bool Curve4(this PathCommand c) => c == PathCommand.Curve4;

		public static bool EndPoly(this PathCommand c) => (c & PathCommand.Mask) == PathCommand.EndPoly;

		public static bool Closed(this PathCommand c) => ((int) c & ~((int) PathFlags.Cw | (int) PathFlags.Ccw)) == ((int) PathCommand.EndPoly | (int) PathFlags.Close);

		public static bool NextPoly(this PathCommand c) => Stop(c) || MoveTo(c) || EndPoly(c);

		public static bool Oriented(int c) => (c & (int) (PathFlags.Cw | PathFlags.Ccw)) != 0;

		public static bool Cw(int c) => (c & (int) PathFlags.Cw) != 0;

		public static bool Ccw(int c) => (c & (int) PathFlags.Ccw) != 0;

		public static int CloseFlag(this PathCommand c) => (int) c & (int) PathFlags.Close;

		public static int GetOrientation(this PathCommand c) => (int) c & (int) (PathFlags.Cw | PathFlags.Ccw);

		public static int ClearOrientation(this PathCommand c) => (int) c & ~(int) (PathFlags.Cw | PathFlags.Ccw);

		public static int SetOrientation(this PathCommand c, PathFlags o) => ClearOrientation(c) | (int) o;

		public static int GetCloseFlag(this PathCommand c) => (int) c & (int) PathFlags.Close;
	}
}