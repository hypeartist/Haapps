using System;

namespace Haapps.Gfx.Agg
{
	public struct LineAAVertex
	{
		public int X;
		public int Y;
		public int Length;

		public LineAAVertex(int x, int y)
		{
			X = x;
			Y = y;
			Length = 0;
		}

		public LineAAVertex(int x, int y, int length)
		{
			X = x;
			Y = y;
			Length = length;
		}

		public bool Measure(ref LineAAVertex val)
		{
			double dx = val.X - X;
			double dy = val.Y - Y;
			return (Length = Common.RoundToU32(Math.Sqrt(dx * dx + dy * dy))) > LineAA.LineSubpixelScale + LineAA.LineSubpixelScaleDiv2;
		}
	}
}