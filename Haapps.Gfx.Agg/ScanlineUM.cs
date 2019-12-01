using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct ScanlineUM<TAlphaMask> : IScanline
		where TAlphaMask : unmanaged, IAlphaMask
	{
		private readonly TAlphaMask* _alphaMask;
		private byte* _covers;
		private Span* _spans;
		private Span* _currentSpan;
		private int _lastX;
		private int _minX;

		public ScanlineUM(ref TAlphaMask mask) : this() => _alphaMask = (TAlphaMask*) Unsafe.AsPointer(ref mask);

		public int Y { 	[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public bool IsEmpty => (int) (_currentSpan - _spans) <= 0;

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
			var xx = x - _minX;
			_covers[xx] = cover;
			if (xx == _lastX + 1)
			{
				_currentSpan->Length++;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = xx + _minX;
				_currentSpan->Length = 1;
				_currentSpan->Covers = _covers + xx;
			}

			_lastX = xx;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddCells(int x, int length, byte* covers)
		{
			var xx = x - _minX;
			Unsafe.CopyBlock((_covers + xx), covers, (uint) length);
			if (xx == _lastX + 1)
			{
				_currentSpan->Length += length;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = xx + _minX;
				_currentSpan->Length = length;
				_currentSpan->Covers = _covers + xx;
			}

			_lastX = xx + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void AddSpan(int x, int length, byte cover)
		{
			var xx = x - _minX;
			Unsafe.InitBlock((_covers + xx), cover, (uint) length);
			if (xx == _lastX + 1)
			{
				_currentSpan->Length += length;
			}
			else
			{
				_currentSpan++;
				_currentSpan->X = xx + _minX;
				_currentSpan->Length = length;
				_currentSpan->Covers = _covers + xx;
			}

			_lastX = xx + length - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void ResetSpans()
		{
			_lastX = 0x7FFFFFF0;
			_currentSpan = _spans;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Finalize(int spanY)
		{
			Y = spanY;
			if (_alphaMask == null)
			{
				return;
			}

			var span = _spans + 1;
			var count = (int) (_currentSpan - _spans);
			do
			{
				_alphaMask->CombineHSpan(span->X, Y, span->Covers, span->Length);
				++span;
			} while (--count != 0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public Span* GetSpans(out int count)
		{
			count = (int) (_currentSpan - _spans);
			return _spans + 1;
		}
	}
}