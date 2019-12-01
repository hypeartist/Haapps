using System;

namespace Haapps.Gfx.Agg
{
	[Flags]
	public enum PathFlags : byte
	{
		None = 0,
		Ccw = 0x10,
		Cw = 0x20,
		Close = 0x40,
		Mask = 0xF0
	}
}