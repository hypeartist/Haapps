using System;

namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	[Flags]
	public enum WindowFlags
	{
		Resize = 1,
		HwBuffer = 2,
		KeepAspectRatio = 4,
		ProcessAllKeys = 8,
		Maximized = 16
	}
}