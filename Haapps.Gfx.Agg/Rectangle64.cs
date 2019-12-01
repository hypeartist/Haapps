namespace Haapps.Gfx.Agg
{
	public struct Rectangle64
	{
		public double X1;
		public double Y1;
		public double X2;
		public double Y2;

		public Rectangle64(double x1, double y1, double x2, double y2) : this()
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		public void Init(double x1, double y1, double x2, double y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		public Rectangle64 Normalize()
		{
			double t;
			if (X1 > X2)
			{
				t = X1;
				X1 = X2;
				X2 = t;
			}
			if (Y1 <= Y2) return this;
			t = Y1;
			Y1 = Y2;
			Y2 = t;
			return this;
		}

		public bool Clip(ref Rectangle64 r)
		{
			if (X2 > r.X2)
			{
				X2 = r.X2;
			}
			if (Y2 > r.Y2)
			{
				Y2 = r.Y2;
			}
			if (X1 < r.X1)
			{
				X1 = r.X1;
			}
			if (Y1 < r.Y1)
			{
				Y1 = r.Y1;
			}
			return X1 <= X2 && Y1 <= Y2;
		}

		public bool IsValid => X1 <= X2 && Y1 <= Y2;

		public bool HitTest(double x, double y) => (x >= X1 && x <= X2 && y >= Y1 && y <= Y2);

		public override string ToString() => $"X={X1:F}, Y={Y1:F}, Width={X2 - X1:F}, Height={Y2 - Y1:F}";
	}
}
