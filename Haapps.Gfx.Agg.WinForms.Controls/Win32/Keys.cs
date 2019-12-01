namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	public sealed class Keys
	{
		private static readonly KeyCode[] Mapping = new KeyCode[256];

		public static readonly Keys Map = new Keys();

		private Keys() { }

		static Keys()
		{
			Mapping[(int)NativeMethods.KeyStates.VK_PAUSE] = KeyCode.Pause;
			Mapping[(int)NativeMethods.KeyStates.VK_CLEAR] = KeyCode.Clear;

			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD0] = KeyCode.Kp0;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD1] = KeyCode.Kp1;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD2] = KeyCode.Kp2;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD3] = KeyCode.Kp3;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD4] = KeyCode.Kp4;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD5] = KeyCode.Kp5;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD6] = KeyCode.Kp6;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD7] = KeyCode.Kp7;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD8] = KeyCode.Kp8;
			Mapping[(int)NativeMethods.KeyStates.VK_NUMPAD9] = KeyCode.Kp9;
			Mapping[(int)NativeMethods.KeyStates.VK_DECIMAL] = KeyCode.KpPeriod;
			Mapping[(int)NativeMethods.KeyStates.VK_DIVIDE] = KeyCode.KpDivide;
			Mapping[(int)NativeMethods.KeyStates.VK_MULTIPLY] = KeyCode.KpMultiply;
			Mapping[(int)NativeMethods.KeyStates.VK_SUBTRACT] = KeyCode.KpMinus;
			Mapping[(int)NativeMethods.KeyStates.VK_ADD] = KeyCode.KpPlus;
			Mapping[(int)NativeMethods.KeyStates.VK_RETURN] = KeyCode.Return;
			Mapping[(int)NativeMethods.KeyStates.VK_SPACE] = KeyCode.Space;

			Mapping[(int)NativeMethods.KeyStates.VK_UP] = KeyCode.Up;
			Mapping[(int)NativeMethods.KeyStates.VK_DOWN] = KeyCode.Down;
			Mapping[(int)NativeMethods.KeyStates.VK_RIGHT] = KeyCode.Right;
			Mapping[(int)NativeMethods.KeyStates.VK_LEFT] = KeyCode.Left;
			Mapping[(int)NativeMethods.KeyStates.VK_INSERT] = KeyCode.Insert;
			Mapping[(int)NativeMethods.KeyStates.VK_DELETE] = KeyCode.Delete;
			Mapping[(int)NativeMethods.KeyStates.VK_HOME] = KeyCode.Home;
			Mapping[(int)NativeMethods.KeyStates.VK_END] = KeyCode.End;
			Mapping[(int)NativeMethods.KeyStates.VK_PRIOR] = KeyCode.PageUp;
			Mapping[(int)NativeMethods.KeyStates.VK_NEXT] = KeyCode.PageDown;

			Mapping[(int)NativeMethods.KeyStates.VK_F1] = KeyCode.F1;
			Mapping[(int)NativeMethods.KeyStates.VK_F2] = KeyCode.F2;
			Mapping[(int)NativeMethods.KeyStates.VK_F3] = KeyCode.F3;
			Mapping[(int)NativeMethods.KeyStates.VK_F4] = KeyCode.F4;
			Mapping[(int)NativeMethods.KeyStates.VK_F5] = KeyCode.F5;
			Mapping[(int)NativeMethods.KeyStates.VK_F6] = KeyCode.F6;
			Mapping[(int)NativeMethods.KeyStates.VK_F7] = KeyCode.F7;
			Mapping[(int)NativeMethods.KeyStates.VK_F8] = KeyCode.F8;
			Mapping[(int)NativeMethods.KeyStates.VK_F9] = KeyCode.F9;
			Mapping[(int)NativeMethods.KeyStates.VK_F10] = KeyCode.F10;
			Mapping[(int)NativeMethods.KeyStates.VK_F11] = KeyCode.F11;
			Mapping[(int)NativeMethods.KeyStates.VK_F12] = KeyCode.F12;
			Mapping[(int)NativeMethods.KeyStates.VK_F13] = KeyCode.F13;
			Mapping[(int)NativeMethods.KeyStates.VK_F14] = KeyCode.F14;
			Mapping[(int)NativeMethods.KeyStates.VK_F15] = KeyCode.F15;

			Mapping[(int)NativeMethods.KeyStates.VK_NUMLOCK] = KeyCode.NumLock;
			Mapping[(int)NativeMethods.KeyStates.VK_CAPITAL] = KeyCode.CapsLock;
			Mapping[(int)NativeMethods.KeyStates.VK_SCROLL] = KeyCode.ScrollLock;
		}

		public KeyCode this[int code] => Mapping[code];
	}
}