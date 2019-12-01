using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct ScanlineU : IScanline
	{
		private byte* _covers;
		private Span* _spans;
		private Span* _currentSpan;
		private int _lastX;
		private int _minX;

		public int Y { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => (int) (_currentSpan - _spans) <= 0;
		}

		public bool IsHit => false;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int CalcMaxLength(int minX, int maxX)
		{
			_minX = minX;
			return maxX - minX + 2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Reset(byte* covers, Span* spans)
		{
			_covers = covers;
			_spans = spans;
			_lastX = 0x7FFFFFF0;
			_currentSpan = _spans;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddCell(int x, byte cover)
		{
			x -= _minX;
			_covers[x] = cover;
			if (x == _lastX + 1)
			{
				_currentSpan->Length++;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = x + _minX;
				_currentSpan->Length = 1;
				_currentSpan->Covers = _covers + x;
			}

			_lastX = x;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddCells(int x, int length, byte* covers)
		{
			x -= _minX;
			Unsafe.CopyBlock((_covers + x), covers, (uint) length);
			if (x == _lastX + 1)
			{
				_currentSpan->Length += length;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = x + _minX;
				_currentSpan->Length = length;
				_currentSpan->Covers = _covers + x;
			}

			_lastX = x + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddSpan(int x, int length, byte cover)
		{
			x -= _minX;
			Unsafe.InitBlock((_covers + x), cover, (uint) length);
			if (x == _lastX + 1)
			{
				_currentSpan->Length += length;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = x + _minX;
				_currentSpan->Length = length;
				_currentSpan->Covers = _covers + x;
			}

			_lastX = x + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Finalize(int y) => Y = y;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void ResetSpans()
		{
			_lastX = 0x7FFFFFF0;
			_currentSpan = _spans;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public Span* GetSpans(out int count)
		{
			count = (int) (_currentSpan - _spans);
			return _spans + 1;
		}
	}
}