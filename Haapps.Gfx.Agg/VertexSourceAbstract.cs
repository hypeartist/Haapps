using System;

namespace Haapps.Gfx.Agg
{
	public abstract unsafe class VertexSourceAbstract : IDisposable
	{
		public abstract void Rewind(int pathId = 0);
		public abstract PathCommand Vertex(ref double x, ref double y);
		public abstract void Dispose();
		
		public bool Bounds(int* paths, int start, int count, out int X1, out int Y1, out int X2, out int Y2)
		{
			double x = 0;
			double y = 0;
			var first = true;

			X1 = 1;
			Y1 = 1;
			X2 = 0;
			Y2 = 0;

			for (var i = 0; i < count; i++)
			{
				Rewind(paths[start + i]);
				PathCommand cmd;
				while (!(cmd = Vertex(ref x, ref y)).Stop())
				{
					if (!cmd.Vertex()) continue;
					if (first)
					{
						X1 = (int) x;
						Y1 = (int) y;
						X2 = (int) x;
						Y2 = (int) y;
						first = false;
					}
					else
					{
						if (x < X1)
						{
							X1 = (int) x;
						}

						if (y < Y1)
						{
							Y1 = (int) y;
						}

						if (x > X2)
						{
							X2 = (int) x;
						}

						if (y > Y2)
						{
							Y2 = (int) y;
						}
					}
				}
			}

			return X1 <= X2 && Y1 <= Y2;
		}

		public bool Bounds(int pathId, out int X1, out int Y1, out int X2, out int Y2)
		{
			double x = 0;
			double y = 0;
			var first = true;

			X1 = 1;
			Y1 = 1;
			X2 = 0;
			Y2 = 0;

			Rewind(pathId);
			PathCommand cmd;
			while (!(cmd = Vertex(ref x, ref y)).Stop())
			{
				if (!cmd.Vertex()) continue;
				if (first)
				{
					X1 = (int) x;
					Y1 = (int) y;
					X2 = (int) x;
					Y2 = (int) y;
					first = false;
				}
				else
				{
					if (x < X1)
					{
						X1 = (int) x;
					}

					if (y < Y1)
					{
						Y1 = (int) y;
					}

					if (x > X2)
					{
						X2 = (int) x;
					}

					if (y > Y2)
					{
						Y2 = (int) y;
					}
				}
			}

			return X1 <= X2 && Y1 <= Y2;
		}

		public bool Bounds(int* paths, int start, int count, out double X1, out double Y1, out double X2, out double Y2)
		{
			double x = 0;
			double y = 0;
			var first = true;

			X1 = 1;
			Y1 = 1;
			X2 = 0;
			Y2 = 0;

			for (var i = 0; i < count; i++)
			{
				Rewind(paths[start + i]);
				PathCommand cmd;
				while (!(cmd = Vertex(ref x, ref y)).Stop())
				{
					if (!cmd.Vertex()) continue;
					if (first)
					{
						X1 = x;
						Y1 = y;
						X2 = x;
						Y2 = y;
						first = false;
					}
					else
					{
						if (x < X1)
						{
							X1 = x;
						}

						if (y < Y1)
						{
							Y1 = y;
						}

						if (x > X2)
						{
							X2 = x;
						}

						if (y > Y2)
						{
							Y2 = y;
						}
					}
				}
			}

			return X1 <= X2 && Y1 <= Y2;
		}

		public bool Bounds(int pathId, out double X1, out double Y1, out double X2, out double Y2)
		{
			double x = 0;
			double y = 0;
			var first = true;

			X1 = 1;
			Y1 = 1;
			X2 = 0;
			Y2 = 0;

			Rewind(pathId);
			PathCommand cmd;
			while (!(cmd = Vertex(ref x, ref y)).Stop())
			{
				if (!cmd.Vertex()) continue;
				if (first)
				{
					X1 = x;
					Y1 = y;
					X2 = x;
					Y2 = y;
					first = false;
				}
				else
				{
					if (x < X1)
					{
						X1 = x;
					}

					if (y < Y1)
					{
						Y1 = y;
					}

					if (x > X2)
					{
						X2 = x;
					}

					if (y > Y2)
					{
						Y2 = y;
					}
				}
			}

			return X1 <= X2 && Y1 <= Y2;
		}
	}
}