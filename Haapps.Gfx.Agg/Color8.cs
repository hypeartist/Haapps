namespace Haapps.Gfx.Agg
{
	public struct Color8 : IColor
	{
		public const int BaseShift = 8;
		public const int BaseScale = 1 << BaseShift;
		public const int BaseMask = BaseScale - 1;
		public const int BaseMsb = 1 << (BaseShift - 1);

		public static Color8 Random(double alphaDiv = 1) => new Color8((byte) Common.Rand(255), (byte) Common.Rand(255), (byte) Common.Rand(255), (byte) (Common.Rand(255) / alphaDiv));

		public static Color8 FromDoubles(double r, double g, double b, double a = 1)
		{
			var c = new Color8
			{
				R = (byte) Common.RoundToU32(r * BaseMask),
				G = (byte) Common.RoundToU32(g * BaseMask),
				B = (byte) Common.RoundToU32(b * BaseMask),
				A = (byte) Common.RoundToU32(a * BaseMask)
			};
			return c;
		}

		public static Color8 PackedRgb(int v) => new Color8((byte) ((v >> 16) & 0xFF), (byte) ((v >> 8) & 0xFF), (byte) (v & 0xFF));

		public Color8(byte r, byte g, byte b, byte a = 255)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public byte R;
		public byte G;
		public byte B;
		public byte A { get; set; }

		public readonly Color8 Gradient(Color8 c, double k)
		{
			Color8 ret = default;
			var ik = Common.RoundToU32(k * BaseScale);
			ret.R = (byte) (R + (((c.R - R) * ik) >> BaseShift));
			ret.G = (byte) (G + (((c.G - G) * ik) >> BaseShift));
			ret.B = (byte) (B + (((c.B - B) * ik) >> BaseShift));
			ret.A = (byte) (A + (((c.A - A) * ik) >> BaseShift));
			return ret;
		}
#if DEBUG
		public override string ToString() => $"R: {R}, G: {G}, B: {B}, A: {A}";
#endif
	}
}