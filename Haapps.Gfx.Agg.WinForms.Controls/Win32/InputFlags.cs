using System;

namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	[Flags]
	public enum InputFlags
	{
		MouseLeft = 1,
		MouseRight = 2,
		Shift = 4,
		Ctrl = 8
	}
}