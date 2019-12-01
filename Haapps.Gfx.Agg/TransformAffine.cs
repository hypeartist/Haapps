using System;

namespace Haapps.Gfx.Agg
{
	public unsafe struct TransformAffine : ITransform
	{
		public static TransformAffine Default = new TransformAffine()
		{
			SY = 1.0,
			SX = 1.0
		};

		public TransformAffine(double sx, double shy, double shx, double sy, double tx, double ty)
		{
			SX = sx;
			SHX = shx;
			SHY = shy;
			SY = sy;
			TX = tx;
			TY = ty;
		}

		public double SX { get; private set; }

		public double SHX { get; private set; }

		public double SHY { get; private set; }

		public double SY { get; private set; }

		public double TX { get; private set; }

		public double TY { get; private set; }

		public static TransformAffine operator *(TransformAffine m1, TransformAffine m2) => m1.Multiply(m2);

		public static TransformAffine operator /(TransformAffine m1, TransformAffine m2) => m1.MultiplyInv(m2);

		public static TransformAffine operator ~(TransformAffine m) => new TransformAffine().LoadFrom(m).Invert();

		public static TransformAffine AffineRotation(double a) => new TransformAffine(Math.Cos(a), Math.Sin(a), -Math.Sin(a), Math.Cos(a), 0.0, 0.0);

		public static TransformAffine AffineScaling(double s) => new TransformAffine(s, 0.0, 0.0, s, 0.0, 0.0);

		public static TransformAffine AffineScaling(double x, double y) => new TransformAffine(x, 0.0, 0.0, y, 0.0, 0.0);

		public static TransformAffine AffineTranslation(double x, double y) => new TransformAffine(1.0, 0.0, 0.0, 1.0, x, y);

		public static TransformAffine AffineSkewing(double x, double y) => new TransformAffine(1.0, Math.Tan(y), Math.Tan(x), 1.0, 0.0, 0.0);

		public TransformAffine Reset()
		{
			SX = SY = 1.0;
			SHY = SHX = TX = TY = 0.0;
			return this;
		}

		public TransformAffine MultiplyInv(TransformAffine m)
		{
			var t = m;
			t.Invert();
			return LoadFrom(t.Multiply(this));
		}

		public TransformAffine Multiply(TransformAffine m)
		{
			var t0 = SX * m.SX + SHY * m.SHX;
			var t2 = SHX * m.SX + SY * m.SHX;
			var t4 = TX * m.SX + TY * m.SHX + m.TX;
			SHY = SX * m.SHY + SHY * m.SY;
			SY = SHX * m.SHY + SY * m.SY;
			TY = TX * m.SHY + TY * m.SY + m.TY;
			SX = t0;
			SHX = t2;
			TX = t4;
			return this;
		}

		public TransformAffine LoadFrom(TransformAffine m)
		{
			SX = m.SX;
			SHX = m.SHX;
			SHY = m.SHY;
			SY = m.SY;
			TX = m.TX;
			TY = m.TY;
			return this;
		}

		public TransformAffine Invert()
		{
			var d = DeterminantReciprocal();

			var t0 = SY * d;
			SY = SX * d;
			SHY = -SHY * d;
			SHX = -SHX * d;

			var t4 = -TX * t0 - TY * SHX;
			TY = -TX * SHY - TY * SY;

			SX = t0;
			TX = t4;
			return this;
		}

		public double DeterminantReciprocal() => 1.0 / (SX * SY - SHY * SHX);

		public TransformAffine ParlToParl(double* src, double* dst)
		{
			SX = src[2] - src[0];
			SHY = src[3] - src[1];
			SHX = src[4] - src[0];
			SY = src[5] - src[1];
			TX = src[0];
			TY = src[1];
			Invert();
			return Multiply(new TransformAffine(dst[2] - dst[0], dst[3] - dst[1], dst[4] - dst[0], dst[5] - dst[1], dst[0], dst[1]));
		}

		public TransformAffine ParlToRect(double* p, double x1, double y1, double x2, double y2)
		{
			var dst = stackalloc double[]{x1, y1, x2, y1, x2, y2};
			return ParlToParl(p, dst);
		}

		public void Transform(ref double x, ref double y)
		{
			var tmp = x;
			x = tmp * SX + y * SHX + TX;
			y = tmp * SHY + y * SY + TY;
		}

		public void InverseTransform(ref double x, ref double y)
		{
			var d = DeterminantReciprocal();
			var a = (x - TX) * d;
			var b = (y - TY) * d;
			x = a * SY - b * SHX;
			y = b * SX - a * SHY;
		}

		public double Scale()
		{
			var x = 0.707106781 * SX + 0.707106781 * SHX;
			var y = 0.707106781 * SHY + 0.707106781 * SY;
			return Math.Sqrt(x * x + y * y);
		}
	}
}