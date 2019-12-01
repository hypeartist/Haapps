using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public struct DDA2LineInterpolator
	{
		private readonly int _cnt;

		public int Lft;
		public int Rem;
		public int Mod;
		public int Y;

		public DDA2LineInterpolator(int y2, int count)
		{
			_cnt = count <= 0 ? 1 : count;
			Lft = y2 / _cnt;
			Rem = y2 % _cnt;
			Mod = Rem;
			Y = 0;
			if (Mod > 0)
			{
				return;
			}

			Mod += count;
			Rem += count;
			Lft--;
		}

		public DDA2LineInterpolator(int y1, int y2, int count)
		{
			_cnt = count <= 0 ? 1 : count;
			Lft = (y2 - y1) / _cnt;
			Rem = (y2 - y1) % _cnt;
			Mod = Rem;
			Y = y1;
			if (Mod <= 0)
			{
				Mod += count;
				Rem += count;
				Lft--;
			}

			Mod -= count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Load(int a, int b)
		{
			Mod = a;
			Y = b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Save(out int a, out int b)
		{
			a = Mod;
			b = Y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Inc()
		{
			Mod += Rem;
			Y += Lft;
			if (Mod <= 0)
			{
				return;
			}

			Mod -= _cnt;
			Y++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Dec()
		{
			if (Mod <= Rem)
			{
				Mod += _cnt;
				Y--;
			}

			Mod -= Rem;
			Y -= Lft;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AdjustForward() => Mod -=_cnt;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AdjustBackward() => Mod += _cnt;
	}
}