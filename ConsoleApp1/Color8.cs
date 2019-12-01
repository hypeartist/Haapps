using System.Runtime.InteropServices;

namespace ConsoleApp1
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Color8
	{
		public const int BaseShift = 8;
		public const int BaseScale = 1 << BaseShift;
		public const int BaseMask = BaseScale - 1;
		public const int BaseMsb = 1 << (BaseShift - 1);

		public byte R;
		public byte G;
		public byte B;
		private byte _a;

		public byte A { get => _a; set => _a = value; }
		
		public Color8(byte r, byte g, byte b, byte a = 255) : this()
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}
	}
}