using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ConsoleApp1
{
	public readonly unsafe struct BlenderColor32
	{
		private static readonly Bgra Order = default;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel(byte* p, int r, int g, int b, int a)
		{
			var pdr = p[Order.R];
			var pdg = p[Order.G];
			var pdb = p[Order.B];
			var pda = p[Order.A];

			p[Order.R] = (byte)((a * (r - pdr) + (pdr << Color8.BaseShift)) >> Color8.BaseShift);
			p[Order.G] = (byte)((a * (g - pdg) + (pdg << Color8.BaseShift)) >> Color8.BaseShift);
			p[Order.B] = (byte)((a * (b - pdb) + (pdb << Color8.BaseShift)) >> Color8.BaseShift);
			p[Order.A] = (byte)(a + pda - ((a * pda + Color8.BaseMask) >> Color8.BaseShift));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel(byte* p, int r, int g, int b, int a, int cover)
		{
			a = (a * (cover + 1)) >> 8;
			BlendPixel(p, r, g, b, a);
		}

		const byte r = 0;
		const byte g = 1;
		const byte b = 2;
		const byte a = 3;
			
		private static byte orderR = (byte) Order.R;
		private static byte orderG = (byte) Order.G;
		private static byte orderB = (byte) Order.B;
		private static byte orderA = (byte) Order.A;

		// ReSharper disable once ShiftExpressionZeroLeftOperand
		private static Vector128<byte> mskSrcShfl = Vector128.Create(r << (orderR << 3) | g << (orderG << 3) | b << (orderB << 3) | a << (orderA << 3)).AsByte();
		private static Vector128<int> mskSingleComp = Vector128.Create(0x000000FF);
		private static Vector128<int> constOne = Sse2.Subtract(Vector128<int>.Zero, Sse2.CompareEqual(Vector128<int>.Zero, Vector128<int>.Zero));
		private static Vector128<int> constBaseShift = Vector128.Create(Color8.BaseMask);
		private static Vector128<byte> mskCvrShfl = Vector128.Create(0xFFFFFF01FFFFFF00, 0xFFFFFF03FFFFFF02).AsByte();

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel4(ref byte* pDst, ref int length, Color8 color, byte cover)
		{
			var vSrc = Ssse3.Shuffle(Vector128.Create(*(int*) &color).AsByte(), mskSrcShfl);
			var vCvr = Sse2.And(Vector128.Create(cover).AsInt32(), mskSingleComp.AsInt32());

			var vSrcA = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderA << 3)), mskSingleComp.AsInt32());
			vSrcA = Sse2.ShiftRightLogical(Sse41.MultiplyLow(Sse2.Add(vCvr, constOne).AsInt32(), vSrcA), Color8.BaseShift);

			var stop = length & 3;
			while (length > stop)
			{
				var vDst = Sse2.LoadVector128(pDst);

				var vSrcR = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderR << 3)), mskSingleComp.AsInt32());
				var vDstR = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), r << 3), mskSingleComp.AsInt32());
				var vResR = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcR, vDstR), vSrcA), Sse2.ShiftLeftLogical(vDstR, Color8.BaseShift)), Color8.BaseShift);

				var vSrcG = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderG << 3)), mskSingleComp.AsInt32());
				var vDstG = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), g << 3), mskSingleComp.AsInt32());
				var vResG = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcG, vDstG), vSrcA), Sse2.ShiftLeftLogical(vDstG, Color8.BaseShift)), Color8.BaseShift);

				var vSrcB = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderB << 3)), mskSingleComp.AsInt32());
				var vDstB = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), b << 3), mskSingleComp.AsInt32());
				var vResB = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcB, vDstB), vSrcA), Sse2.ShiftLeftLogical(vDstB, Color8.BaseShift)), Color8.BaseShift);

				var vDstA = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), a << 3), mskSingleComp.AsInt32());
				var vResA = Sse2.Subtract(Sse2.Add(vSrcA, vDstA), Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(vSrcA, vDstA), constBaseShift), Color8.BaseShift));

				var rr = Sse2.ShiftLeftLogical(vResR, (byte)(orderR << 3)).AsByte();
				var gg = Sse2.ShiftLeftLogical(vResG, (byte)(orderG << 3)).AsByte();
				var bb = Sse2.ShiftLeftLogical(vResB, (byte)(orderB << 3)).AsByte();
				var aa = Sse2.ShiftLeftLogical(vResA, (byte)(orderA << 3)).AsByte();
				
				Sse2.Store(pDst, Sse2.Or(rr, Sse2.Or(gg, Sse2.Or(bb, aa))));
				length -= 4;
				pDst += 16;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel4(ref byte* pDst, ref int length, Color8 color, byte* covers)
		{
			var vSrc = Ssse3.Shuffle(Vector128.Create(*(int*) &color).AsByte(), mskSrcShfl);

			var stop = length & 3;
			while (length > stop)
			{
				var vDst = Sse2.LoadVector128(pDst);

				var vCvr = Ssse3.Shuffle(Sse2.LoadVector128(covers), mskCvrShfl).AsInt32();

				var vSrcA = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderA << 3)), mskSingleComp.AsInt32());
				vSrcA = Sse2.ShiftRightLogical(Sse41.MultiplyLow(Sse2.Add(vCvr, constOne).AsInt32(), vSrcA), Color8.BaseShift);

				var vSrcR = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderR << 3)), mskSingleComp.AsInt32());
				var vDstR = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), r << 3), mskSingleComp.AsInt32());
				var vResR = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcR, vDstR), vSrcA), Sse2.ShiftLeftLogical(vDstR, Color8.BaseShift)), Color8.BaseShift);

				var vSrcG = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderG << 3)), mskSingleComp.AsInt32());
				var vDstG = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), g << 3), mskSingleComp.AsInt32());
				var vResG = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcG, vDstG), vSrcA), Sse2.ShiftLeftLogical(vDstG, Color8.BaseShift)), Color8.BaseShift);

				var vSrcB = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderB << 3)), mskSingleComp.AsInt32());
				var vDstB = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), b << 3), mskSingleComp.AsInt32());
				var vResB = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcB, vDstB), vSrcA), Sse2.ShiftLeftLogical(vDstB, Color8.BaseShift)), Color8.BaseShift);

				var vDstA = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), a << 3), mskSingleComp.AsInt32());
				var vResA = Sse2.Subtract(Sse2.Add(vSrcA, vDstA), Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(vSrcA, vDstA), constBaseShift), Color8.BaseShift));

				var rr = Sse2.ShiftLeftLogical(vResR, (byte)(orderR << 3)).AsByte();
				var gg = Sse2.ShiftLeftLogical(vResG, (byte)(orderG << 3)).AsByte();
				var bb = Sse2.ShiftLeftLogical(vResB, (byte)(orderB << 3)).AsByte();
				var aa = Sse2.ShiftLeftLogical(vResA, (byte)(orderA << 3)).AsByte();
				
				Sse2.Store(pDst, Sse2.Or(rr, Sse2.Or(gg, Sse2.Or(bb, aa))));
				length -= 4;
				pDst += 16;
			}
		}
	}
}