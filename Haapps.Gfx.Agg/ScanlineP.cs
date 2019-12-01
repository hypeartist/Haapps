using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct ScanlineP : IScanline
	{
		private byte* _covers;
		private Span* _spans;
		private byte* _coverPtr;
		private Span* _currentSpan;
		private int _lastX;

		public int Y { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public bool IsEmpty
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => (int) (_currentSpan - _spans) <= 0;
		}

		public bool IsHit => false;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int CalcMaxLength(int minX, int maxX) => maxX - minX + 3;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Reset(byte* covers, Span* spans)
		{
			_covers = covers;
			_spans = spans;
			_lastX = 0x7FFFFFF0;
			_coverPtr = covers;
			_currentSpan = _spans;
			_currentSpan->Length = default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddCell(int x, byte cover)
		{
			*_coverPtr = cover;
			if (x == _lastX + 1 && _currentSpan->Length > 0)
			{
				_currentSpan->Length++;
			}
			else
			{
				_currentSpan++;
				_currentSpan->Covers = _coverPtr;
				_currentSpan->X = x;
				_currentSpan->Length = 1;
			}

			_lastX = x;
			_coverPtr++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddCells(int x, int length, byte* covers)
		{
			Unsafe.CopyBlock(_coverPtr, covers, (uint) length);
			if (x == _lastX + 1 && _currentSpan->Length > 0)
			{
				_currentSpan->Length += length;
			}
			else
			{
				_currentSpan++;
				_currentSpan->Covers = _coverPtr;
				_currentSpan->X = x;
				_currentSpan->Length = length;
			}

			_coverPtr += length;
			_lastX = x + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddSpan(int x, int length, byte cover)
		{
			if (x == _lastX + 1 && _currentSpan->Length < 0 && cover == *_currentSpan->Covers)
			{
				_currentSpan->Length -= length;
			}
			else
			{
				*_coverPtr = cover;
				_currentSpan++;
				_currentSpan->Covers = _coverPtr++;
				_currentSpan->X = x;
				_currentSpan->Length = -length;
			}

			_lastX = x + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Finalize(int y) => Y = y;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void ResetSpans()
		{
			_lastX = 0x7FFFFFF0;
			_coverPtr = _covers;
			_currentSpan = _spans;
			_currentSpan->Length = default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public Span* GetSpans(out int count)
		{
			count = (int) (_currentSpan - _spans);
			return _spans + 1;
		}
	}
}