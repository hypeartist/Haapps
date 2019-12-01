using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct AlphaMaskNoClipU8<TRenderingBuffer, TOrderColor, TMaskFunction, TMaskParams> : IAlphaMask
		where TRenderingBuffer : unmanaged, IRenderingBuffer
		where TOrderColor : struct, IOrderColor24
		where TMaskFunction : struct, IMaskFunction<TOrderColor>
		where TMaskParams : struct, IMaskParams<TOrderColor>
	{
		private static TMaskFunction _maskFunc = default;
		private static readonly TMaskParams Params = default;
		private readonly TRenderingBuffer* _buffer;

		public AlphaMaskNoClipU8(ref TRenderingBuffer buffer) => _buffer = (TRenderingBuffer*) Unsafe.AsPointer(ref buffer);

		public readonly byte Pixel(int x, int y) => _maskFunc.Calculate(_buffer->GetRowPtr(y) + x * Params.Step + Params.Offset);

		public readonly byte CombinePixel(int x, int y, byte val) => (byte) ((Common.CoverFull + val * _maskFunc.Calculate(_buffer->GetRowPtr(y) + x * Params.Step + Params.Offset)) >> Common.CoverShift);

		public readonly void FillHSpan(int x, int y, byte* dst, int numPix)
		{
			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*dst++ = _maskFunc.Calculate(mask);
				mask += Params.Step;
			} while (--numPix != 0);
		}

		public readonly void CombineHSpan(int x, int y, byte* dst, int numPix)
		{
			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*dst = (byte) ((Common.CoverFull + *dst * _maskFunc.Calculate(mask)) >> Common.CoverShift);
				++dst;
				mask += Params.Step;
			} while (--numPix != 0);
		}

		public readonly void FillVSpan(int x, int y, byte* dst, int numPix)
		{
			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*dst++ = _maskFunc.Calculate(mask);
				mask += _buffer->Stride;
			} while (--numPix != 0);
		}

		public readonly void CombineVSpan(int x, int y, byte* dst, int numPix)
		{
			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*dst = (byte) ((Common.CoverFull + *dst * _maskFunc.Calculate(mask)) >> Common.CoverShift);
				++dst;
				mask += _buffer->Stride;
			} while (--numPix != 0);
		}
	}
}