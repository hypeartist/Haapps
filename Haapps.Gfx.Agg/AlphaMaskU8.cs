using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct AlphaMaskU8<TRenderingBuffer, TOrderColor, TMaskFunction, TMaskParams> : IAlphaMask
		where TRenderingBuffer : unmanaged, IRenderingBuffer
		where TOrderColor : struct, IOrderColor24
		where TMaskFunction : struct, IMaskFunction<TOrderColor>
		where TMaskParams : struct, IMaskParams<TOrderColor>
	{
		private static TMaskFunction _maskFunc = default;
		private static readonly TMaskParams Params = default;
		private readonly TRenderingBuffer* _buffer;

		public AlphaMaskU8(ref TRenderingBuffer buffer) => _buffer = (TRenderingBuffer*) Unsafe.AsPointer(ref buffer);

		public readonly byte Pixel(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < _buffer->Width && y < _buffer->Height)
			{
				return _maskFunc.Calculate(_buffer->GetRowPtr(y) + x * Params.Step + Params.Offset);
			}

			return 0;
		}

		public readonly byte CombinePixel(int x, int y, byte val)
		{
			if (x >= 0 && y >= 0 && x < _buffer->Width && y < _buffer->Height)
			{
				return (byte) ((Common.CoverFull + val * _maskFunc.Calculate(_buffer->GetRowPtr(y) + x * Params.Step + Params.Offset)) >> Common.CoverShift);
			}

			return 0;
		}

		public readonly void FillHSpan(int x, int y, byte* dst, int numPix)
		{
			var xmax = _buffer->Width - 1;
			var ymax = _buffer->Height - 1;

			var count = numPix;
			var covers = dst;

			if (y < 0 || y > ymax)
			{
				Unsafe.InitBlock(dst, 0, (uint) numPix);
				return;
			}

			if (x < 0)
			{
				count += x;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock(covers, 0, (uint) -x);
				covers -= x;
				x = 0;
			}

			if (x + count > xmax)
			{
				var rest = x + count - xmax - 1;
				count -= rest;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock((covers + count), 0, (uint) rest);
			}

			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*covers++ = _maskFunc.Calculate(mask);
				mask += Params.Step;
			} while (--count != 0);
		}

		public readonly void CombineHSpan(int x, int y, byte* dst, int numPix)
		{
			var xmax = _buffer->Width - 1;
			var ymax = _buffer->Height - 1;

			var count = numPix;
			var covers = dst;

			if (y < 0 || y > ymax)
			{
				Unsafe.InitBlock(dst, 0, (uint) numPix);
				return;
			}

			if (x < 0)
			{
				count += x;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock(covers, 0, (uint) -x);
				covers -= x;
				x = 0;
			}

			if (x + count > xmax)
			{
				var rest = x + count - xmax - 1;
				count -= rest;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock((covers + count), 0, (uint) rest);
			}

			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*covers = (byte) ((Common.CoverFull + *covers * _maskFunc.Calculate(mask)) >> Common.CoverShift);
				++covers;
				mask += Params.Step;
			} while (--count != 0);
		}

		public readonly void FillVSpan(int x, int y, byte* dst, int numPix)
		{
			var xmax = _buffer->Width - 1;
			var ymax = _buffer->Height - 1;

			var count = numPix;
			var covers = dst;

			if (x < 0 || x > xmax)
			{
				Unsafe.InitBlock(dst, 0, (uint) numPix);
				return;
			}

			if (y < 0)
			{
				count += y;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock(covers, 0, (uint) -y);
				covers -= y;
				y = 0;
			}

			if (y + count > ymax)
			{
				var rest = y + count - ymax - 1;
				count -= rest;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock((covers + count), 0, (uint) rest);
			}

			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*covers++ = _maskFunc.Calculate(mask);
				mask += _buffer->Stride;
			} while (--count != 0);
		}

		public readonly void CombineVSpan(int x, int y, byte* dst, int numPix)
		{
			var xmax = _buffer->Width - 1;
			var ymax = _buffer->Height - 1;

			var count = numPix;
			var covers = dst;

			if (x < 0 || x > xmax)
			{
				Unsafe.InitBlock(dst, 0, (uint) numPix);
				return;
			}

			if (y < 0)
			{
				count += y;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock(covers, 0, (uint) -y);
				covers -= y;
				y = 0;
			}

			if (y + count > ymax)
			{
				var rest = y + count - ymax - 1;
				count -= rest;
				if (count <= 0)
				{
					Unsafe.InitBlock(dst, 0, (uint) numPix);
					return;
				}

				Unsafe.InitBlock((covers + count), 0, (uint) rest);
			}

			var mask = _buffer->GetRowPtr(y) + (uint)(x * Params.Step + Params.Offset);
			do
			{
				*covers = (byte) ((Common.CoverFull + *covers * _maskFunc.Calculate(mask)) >> Common.CoverShift);
				++covers;
				mask += _buffer->Stride;
			} while (--count != 0);
		}
	}
}