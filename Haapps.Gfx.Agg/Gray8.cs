namespace Haapps.Gfx.Agg
{
	public struct Gray8 : IColor
	{
		private const int CoverShift = 8;
		private const int CoverSize = 1 << CoverShift;
		private const int CoverMask = CoverSize - 1;

		public const int BaseShift = 8;
		public const int BaseScale = 1 << BaseShift;
		public const int BaseMask = BaseScale - 1;

		public byte V;
		public byte A { get; set; }

		public static Gray8 Empty = new Gray8(0, 0);

		public Gray8(byte v, byte alpha = BaseMask) : this()
		{
			V = v;
			A = alpha;
		}

		public Gray8(Gray8 c) : this()
		{
			V = c.V;
			A = c.A;
		}

		public Gray8(Color8 c) : this()
		{
			V = (byte) ((c.R * 77 + c.G * 150 + c.B * 29) >> 8);
			A = c.A;
		}

		public Gray8(Color8 c, byte a) : this()
		{
			V = (byte) ((c.R * 77 + c.G * 150 + c.B * 29) >> 8);
			A = a;
		}

		public Gray8(double r, double g, double b, double a = BaseMask) : this()
		{
			V = (byte) (int) (0.5 + (0.299 * r + 0.587 * g + 0.114 * b) * BaseMask);
			A = (byte) (int) (0.5 + a * BaseMask);
		}

		public void Clear() => V = A = 0;

		public double Opacity
		{
			get => A / (double) BaseMask;
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}

				if (value > 1.0)
				{
					value = 1.0;
				}

				A = (byte) (int) (value * BaseMask + 0.5);
			}
		}

		public Gray8 Premultiply()
		{
			switch (A)
			{
				case BaseMask:
					return this;
				case 0:
					V = 0;
					return this;
			}

			V = (byte) ((V * A) >> BaseShift);
			return this;
		}

		public Gray8 Premultiply(byte a)
		{
			if (A == BaseMask && a >= BaseMask)
			{
				return this;
			}

			if (A == 0 || a == 0)
			{
				V = A = 0;
				return this;
			}

			var v = V * a / A;
			V = (byte) (v > a ? a : v);
			A = a;
			return this;
		}

		public Gray8 Demultiply()
		{
			switch (A)
			{
				case BaseMask:
					return this;
				case 0:
					V = 0;
					return this;
			}

			var v = V * BaseMask / A;
			V = (byte) (v > BaseMask ? BaseMask : v);
			return this;
		}

		public readonly Gray8 Gradient(Gray8 c, double k)
		{
			var ret = new Gray8();
			var ik = (int) (k * BaseScale);
			ret.V = (byte) (V + (((c.V - V) * ik) >> BaseShift));
			ret.A = (byte) (A + (((c.A - A) * ik) >> BaseShift));
			return ret;
		}

		public void Add(Gray8 c, byte cover)
		{
			int cv, ca;
			if (cover == CoverMask)
			{
				if (c.A == BaseMask)
				{
					this = c;
				}
				else
				{
					cv = V + c.V;
					V = (byte) (cv > BaseMask ? BaseMask : cv);
					ca = A + c.A;
					A = (byte) (ca > BaseMask ? BaseMask : ca);
				}
			}
			else
			{
				cv = V + ((c.V * cover + (CoverMask >> 1)) >> CoverShift);
				ca = A + ((c.A * cover + (CoverMask >> 1)) >> CoverShift);
				V = (byte) (cv > BaseMask ? BaseMask : cv);
				A = (byte) (ca > BaseMask ? BaseMask : ca);
			}
		}

		public static implicit operator Gray8(Color8 c) => new Gray8(c);
	}
}