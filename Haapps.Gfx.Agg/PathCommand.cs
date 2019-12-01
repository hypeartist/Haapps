using System;

namespace Haapps.Gfx.Agg
{
	[Flags]
	public enum PathCommand : byte
	{
		Stop = 0,
		MoveTo = 1,
		LineTo = 2,
		Curve3 = 3,
		Curve4 = 4,
		CurveN = 5,
		Catrom = 6,
		Spline = 7,
		EndPoly = 0x0F,
		Mask = 0x0F
	}
}