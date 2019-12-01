namespace Haapps.Gfx.Agg
{
	public struct Rectangle32
	{
		public int X1;
		public int Y1;
		public int X2;
		public int Y2;

		public Rectangle32(int x1, int y1, int x2, int y2):this()
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		public void Init(int x1, int y1, int x2, int y2)
		{
			X1 = x1;
			Y1 = y2;
			X2 = x2;
			Y2 = y2;
		}

		public Rectangle32 Normalize()
		{
			int t;
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

		public bool Clip(ref Rectangle32 r)
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

		public bool HitTest(int x, int y) => (x >= X1 && x <= X2 && y >= Y1 && y <= Y2);

		public override string ToString() => $"X={X1}, Y={Y1}, Width={X2 - X1}, Height={Y2 - Y1}";
	}
}
