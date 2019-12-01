using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public struct DDALineInterpolator
	{
		private readonly int _fractionShift;
		private int _yShift;
		private int _y;
		private int _inc;
		private int _dy;

		public DDALineInterpolator(int fractionShift, int yShift = 0) : this()
		{
			_fractionShift = fractionShift;
			_yShift = yShift;
		}

		public DDALineInterpolator(int fractionShift, int y1, int y2, int count, int yShift = 0)
		{
			_fractionShift = fractionShift;
			_y = y1;
			_yShift = yShift;
			_inc = ((y2 - y1) << fractionShift) / count;
			_dy = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Init(int y1, int y2, int count, int yShift = 0)
		{
			_y = y1;
			_yShift = yShift;
			_inc = ((y2 - y1) << _fractionShift) / count;
			_dy = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Inc() => _dy += _inc;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Dec() => _dy -= _inc;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Add(int n) => _dy += _inc * n;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Sub(int n) => _dy -= _inc * n;

		public readonly int Y
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]	
			get => _y + (_dy >> (_fractionShift - _yShift));
		}
	}
}