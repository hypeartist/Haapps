using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
// ReSharper disable StaticMemberInGenericType

namespace Haapps.Gfx.Agg
{
	public readonly unsafe struct BlenderColor24<TOrderColor> : IBlenderColor24<TOrderColor>
		where TOrderColor : unmanaged, IOrderColor24
	{
		private static readonly TOrderColor Order = default;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]		
		public void BlendPixel(byte* p, int r, int g, int b, int a)
		{
			var pdr = p[Order.R];
			var pdg = p[Order.G];
			var pdb = p[Order.B];

			p[Order.R] = (byte) (pdr + (((r - pdr) * a) >> Color8.BaseShift));
			p[Order.G] = (byte) (pdg + (((g - pdg) * a) >> Color8.BaseShift));
			p[Order.B] = (byte) (pdb + (((b - pdb) * a) >> Color8.BaseShift));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel(byte* p, int r, int g, int b, int a, int cover)
		{
			var pdr = p[Order.R];
			var pdg = p[Order.G];
			var pdb = p[Order.B];
			
			p[Order.R] = (byte) (pdr + (((r - pdr) * a) >> Color8.BaseShift));
			p[Order.G] = (byte) (pdg + (((g - pdg) * a) >> Color8.BaseShift));
			p[Order.B] = (byte) (pdb + (((b - pdb) * a) >> Color8.BaseShift));
		}

		private static readonly Vector128<byte> DstShuffleMaskInitial = Vector128.Create(0xFF050403FF020100, 0xFF0B0A09FF080706).AsByte();
		private static readonly Vector128<byte> DstShuffleMaskRed = Vector128.Create(((ulong)((Order.R + 4) | 0xFFFFFF00) << 32) | (ulong)(Order.R | 0xFFFFFF00), ((ulong)((Order.R + 12) | 0xFFFFFF00) << 32) | (ulong)((Order.R + 8) | 0xFFFFFF00)).AsByte();
		private static readonly Vector128<byte> DstShuffleMaskGreen = Vector128.Create(((ulong)((Order.G + 4) | 0xFFFFFF00) << 32) | (ulong)(Order.G | 0xFFFFFF00), ((ulong)((Order.G + 12) | 0xFFFFFF00) << 32) | (ulong)((Order.G + 8) | 0xFFFFFF00)).AsByte();
		private static readonly Vector128<byte> DstShuffleMaskBlue = Vector128.Create(((ulong)((Order.B + 4) | 0xFFFFFF00) << 32) | (ulong)(Order.B | 0xFFFFFF00), ((ulong)((Order.B + 12) | 0xFFFFFF00) << 32) | (ulong)((Order.B + 8) | 0xFFFFFF00)).AsByte();

		private static readonly Vector128<byte> SrcShuffleMaskInitial = Vector128.Create(0x0302010003020100, 0x0302010003020100).AsByte();
		private static readonly Vector128<byte> SrcShuffleMaskRed = Vector128.Create(0xFFFFFF04FFFFFF00, 0xFFFFFF0CFFFFFF08).AsByte();
		private static readonly Vector128<byte> SrcShuffleMaskGreen = Vector128.Create(0xFFFFFF05FFFFFF01, 0xFFFFFF0DFFFFFF09).AsByte();
		private static readonly Vector128<byte> SrcShuffleMaskBlue = Vector128.Create(0xFFFFFF06FFFFFF02, 0xFFFFFF0EFFFFFF0A).AsByte();
		private static readonly Vector128<byte> SrcShuffleMaskAlpha = Vector128.Create(0xFFFFFF07FFFFFF03, 0xFFFFFF0FFFFFFF0B).AsByte();

		private static readonly Vector128<byte> ConstMinusOne = Sse2.CompareEqual(Vector128<byte>.Zero, Vector128<byte>.Zero);
		private static readonly Vector128<byte> ConstOne = Sse2.Subtract(Vector128<byte>.Zero, ConstMinusOne);

		private static readonly Vector128<int> CvrMask = Sse2.ShiftRightLogical(ConstMinusOne.AsInt32(), 24);
		private static readonly Vector128<byte> CvrMask2 = Vector128.Create(0xFFFFFF01FFFFFF00, 0xFFFFFF03FFFFFF02).AsByte();

		private static readonly Vector128<byte> ResShuffleMask = Vector128.Create(0x0908060504020100, 0xFFFFFFFF0E0D0C0A).AsByte();

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel4(byte* pDst, Color8 pSrc, byte cover)
		{
			var dst = Ssse3.Shuffle(Sse2.LoadVector128(pDst), DstShuffleMaskInitial);
			var src = Ssse3.Shuffle(Sse2.LoadVector128((byte*)&pSrc), SrcShuffleMaskInitial);
			var cvr = Sse2.And(Ssse3.Shuffle(Sse2.LoadVector128(&cover), Vector128<byte>.Zero).AsInt32(), CvrMask);

			var dstRed = Ssse3.Shuffle(dst, DstShuffleMaskRed).AsInt32();
			var dstGreen = Ssse3.Shuffle(dst, DstShuffleMaskGreen).AsInt32();
			var dstBlue = Ssse3.Shuffle(dst, DstShuffleMaskBlue).AsInt32();

			var srcRed = Ssse3.Shuffle(src, SrcShuffleMaskRed).AsInt32();
			var srcGreen = Ssse3.Shuffle(src, SrcShuffleMaskGreen).AsInt32();
			var srcBlue = Ssse3.Shuffle(src, SrcShuffleMaskBlue).AsInt32();
			var srcAlpha = Ssse3.Shuffle(src, SrcShuffleMaskAlpha).AsInt32();

			srcAlpha = Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Add(cvr, Sse2.And(ConstOne.AsInt32(), CvrMask)), srcAlpha), 8);

			var resRed = Sse2.Add(dstRed, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcRed, dstRed), srcAlpha).AsInt32(), Color8.BaseShift));
			var resGreen = Sse2.Add(dstGreen, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcGreen, dstGreen), srcAlpha).AsInt32(), Color8.BaseShift));
			var resBlue = Sse2.Add(dstBlue, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcBlue, dstBlue), srcAlpha).AsInt32(), Color8.BaseShift));

			var res = Ssse3.Shuffle(Sse2.Or(Sse2.Or(Sse2.ShiftLeftLogical(resRed, (byte)(Order.R << 3)), Sse2.ShiftLeftLogical(resGreen, (byte)(Order.G << 3))), Sse2.ShiftLeftLogical(resBlue, (byte)(Order.B << 3))).AsByte(), ResShuffleMask);

			Sse2.MaskMove(res, Sse2.CompareGreaterThan(ResShuffleMask.AsSByte(), ConstMinusOne.AsSByte()).AsByte(), pDst);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel4(byte* pDst, Color8 pSrc, byte* covers)
		{
			var dst = Ssse3.Shuffle(Sse2.LoadVector128(pDst), DstShuffleMaskInitial);
			var src = Ssse3.Shuffle(Sse2.LoadVector128((byte*)&pSrc), SrcShuffleMaskInitial);
			var cvr = Ssse3.Shuffle(Sse2.LoadVector128(covers), CvrMask2).AsInt32();

			var dstRed = Ssse3.Shuffle(dst, DstShuffleMaskRed).AsInt32();
			var dstGreen = Ssse3.Shuffle(dst, DstShuffleMaskGreen).AsInt32();
			var dstBlue = Ssse3.Shuffle(dst, DstShuffleMaskBlue).AsInt32();

			var srcRed = Ssse3.Shuffle(src, SrcShuffleMaskRed).AsInt32();
			var srcGreen = Ssse3.Shuffle(src, SrcShuffleMaskGreen).AsInt32();
			var srcBlue = Ssse3.Shuffle(src, SrcShuffleMaskBlue).AsInt32();
			var srcAlpha = Ssse3.Shuffle(src, SrcShuffleMaskAlpha).AsInt32();

			srcAlpha = Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Add(cvr, Sse2.And(ConstOne.AsInt32(), CvrMask)), srcAlpha), 8);

			var resRed = Sse2.Add(dstRed, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcRed, dstRed), srcAlpha).AsInt32(), Color8.BaseShift));
			var resGreen = Sse2.Add(dstGreen, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcGreen, dstGreen), srcAlpha).AsInt32(), Color8.BaseShift));
			var resBlue = Sse2.Add(dstBlue, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcBlue, dstBlue), srcAlpha).AsInt32(), Color8.BaseShift));

			var res = Ssse3.Shuffle(Sse2.Or(Sse2.Or(Sse2.ShiftLeftLogical(resRed, (byte)(Order.R << 3)), Sse2.ShiftLeftLogical(resGreen, (byte)(Order.G << 3))), Sse2.ShiftLeftLogical(resBlue, (byte)(Order.B << 3))).AsByte(), ResShuffleMask);

			Sse2.MaskMove(res, Sse2.CompareGreaterThan(ResShuffleMask.AsSByte(), ConstMinusOne.AsSByte()).AsByte(), pDst);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel4(byte* pDst, Color8* pSrc, byte cover)
		{
			var dst = Ssse3.Shuffle(Sse2.LoadVector128(pDst), DstShuffleMaskInitial);
			var src = Sse2.LoadVector128((byte*)&pSrc);
			var cvr = Sse2.And(Ssse3.Shuffle(Sse2.LoadVector128(&cover), Vector128<byte>.Zero).AsInt32(), CvrMask);

			var dstRed = Ssse3.Shuffle(dst, DstShuffleMaskRed).AsInt32();
			var dstGreen = Ssse3.Shuffle(dst, DstShuffleMaskGreen).AsInt32();
			var dstBlue = Ssse3.Shuffle(dst, DstShuffleMaskBlue).AsInt32();

			var srcRed = Ssse3.Shuffle(src, SrcShuffleMaskRed).AsInt32();
			var srcGreen = Ssse3.Shuffle(src, SrcShuffleMaskGreen).AsInt32();
			var srcBlue = Ssse3.Shuffle(src, SrcShuffleMaskBlue).AsInt32();
			var srcAlpha = Ssse3.Shuffle(src, SrcShuffleMaskAlpha).AsInt32();

			srcAlpha = Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Add(cvr, Sse2.And(ConstOne.AsInt32(), CvrMask)), srcAlpha), 8);

			var resRed = Sse2.Add(dstRed, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcRed, dstRed), srcAlpha).AsInt32(), Color8.BaseShift));
			var resGreen = Sse2.Add(dstGreen, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcGreen, dstGreen), srcAlpha).AsInt32(), Color8.BaseShift));
			var resBlue = Sse2.Add(dstBlue, Sse2.ShiftRightArithmetic(Sse41.MultiplyLow(Sse2.Subtract(srcBlue, dstBlue), srcAlpha).AsInt32(), Color8.BaseShift));

			var res = Ssse3.Shuffle(Sse2.Or(Sse2.Or(Sse2.ShiftLeftLogical(resRed, (byte)(Order.R << 3)), Sse2.ShiftLeftLogical(resGreen, (byte)(Order.G << 3))), Sse2.ShiftLeftLogical(resBlue, (byte)(Order.B << 3))).AsByte(), ResShuffleMask);

			Sse2.Store(pDst, res);
		}
	}
}