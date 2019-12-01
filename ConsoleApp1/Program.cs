using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ConsoleApp1
{
	unsafe class Program
	{
		
		const byte r = 0;
		const byte g = 1;
		const byte b = 2;
		const byte a = 3;
			
		private static readonly Bgra Order = default;

		private static byte orderR = (byte) Order.R;
		private static byte orderG = (byte) Order.G;
		private static byte orderB = (byte) Order.B;
		private static byte orderA = (byte) Order.A;

		// ReSharper disable once ShiftExpressionZeroLeftOperand
		private static Vector128<byte> mskSrcShfl = Vector128.Create(r << (orderR << 3) | g << (orderG << 3) | b << (orderB << 3) | a << (orderA << 3)).AsByte();
		private static Vector128<int> mskSingleComp = Vector128.Create(0x000000FF);
			
		private static Vector128<int> constOne = Sse2.Subtract(Vector128<int>.Zero, Sse2.CompareEqual(Vector128<int>.Zero, Vector128<int>.Zero));
		private static Vector128<int> constBaseShift = Vector128.Create(Color8.BaseMask);

		static void Main(string[] args)
		{
			Debug.WriteLine($"Sse2: {Sse2.IsSupported} \nSsse3: {Ssse3.IsSupported} \nSse41: {Sse41.IsSupported} \nSse42: {Sse42.IsSupported} \nAvx: {Avx.IsSupported} \nAvx2: {Avx2.IsSupported}\n");
			// var p = stackalloc byte[16] { 100, 101, 102, 203, 110, 111, 112, 213, 120, 121, 122, 223, 130, 131, 132, 233 };
			
			var p4_24 = stackalloc byte[12] { 100, 101, 102, 110, 111, 112, 120, 121, 122, 130, 131, 132};
			var p4_32 = stackalloc byte[16] { 102, 101, 100, 250, 112, 111, 110, 251, 122, 121, 120, 252, 132, 131, 130, 253};
			var p4_24_sw = stackalloc byte[12] { 100, 101, 102, 110, 111, 112, 120, 121, 122, 130, 131, 132};
			var p4_32_sw = stackalloc byte[16] { 100, 101, 102, 250, 110, 111, 112, 251, 120, 121, 122, 252, 130, 131, 132, 253 };
			var c4 = stackalloc Color8[4] { new Color8(127, 45, 97, 254), new Color8(234, 32, 100, 253), new Color8(76, 145, 33, 252), new Color8(98, 65, 200, 251) };
			
			var v = stackalloc byte[4] { 127, 128, 129, 130 };
			//
			

			var b = new BlenderColor32();
			BlendPixel4_32_sw(b, p4_32_sw, *c4, *v);
			BlendPixel4_32(b, p4_32, 4, *c4, v);

			Console.WriteLine("Hello World!");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlendPixel4_24_sw(BlenderColor24 b, byte* pDst, Color8 color, byte cover)
		{
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 3;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 3;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 3;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlendPixel4_24(BlenderColor24 b, byte* pDst, Color8 color, byte cover)
		{
			b.BlendPixel4(pDst, color, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlendPixel4_32_sw(BlenderColor32 b, byte* pDst, Color8 color, byte cover)
		{
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 4;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 4;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
			pDst += 4;
			b.BlendPixel(pDst, color.R, color.G, color.B, color.A, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlendPixel4_32(BlenderColor32 bl, byte* pDst, int length, Color8 color, byte cover)
		{
			bl.BlendPixel4(ref pDst, ref length, color, cover);

			// var vSrc = Ssse3.Shuffle(Vector128.Create(*(int*) &color).AsByte(), mskSrcShfl);
			// var vCvr = Sse2.And(Vector128.Create(cover).AsInt32(), mskSingleComp.AsInt32());
			//
			// var vSrcA = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderA << 3)), mskSingleComp.AsInt32());
			// vSrcA = Sse2.ShiftRightLogical(Sse41.MultiplyLow(Sse2.Add(vCvr, constOne).AsInt32(), vSrcA), Color8.BaseShift);
			//
			// var stop = length & 3;
			// while (length >= stop)
			// {
			// 	var vDst = Sse2.LoadVector128(pDst);
			//
			// 	var vSrcR = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderR << 3)), mskSingleComp.AsInt32());
			// 	var vDstR = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), r << 3), mskSingleComp.AsInt32());
			// 	var vResR = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcR, vDstR), vSrcA), Sse2.ShiftLeftLogical(vDstR, Color8.BaseShift)), Color8.BaseShift);
			//
			// 	var vSrcG = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderG << 3)), mskSingleComp.AsInt32());
			// 	var vDstG = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), g << 3), mskSingleComp.AsInt32());
			// 	var vResG = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcG, vDstG), vSrcA), Sse2.ShiftLeftLogical(vDstG, Color8.BaseShift)), Color8.BaseShift);
			//
			// 	var vSrcB = Sse2.And(Sse2.ShiftRightLogical(vSrc.AsInt32(), (byte)(orderB << 3)), mskSingleComp.AsInt32());
			// 	var vDstB = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), b << 3), mskSingleComp.AsInt32());
			// 	var vResB = Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(Sse2.Subtract(vSrcB, vDstB), vSrcA), Sse2.ShiftLeftLogical(vDstB, Color8.BaseShift)), Color8.BaseShift);
			//
			// 	var vDstA = Sse2.And(Sse2.ShiftRightLogical(vDst.AsInt32(), a << 3), mskSingleComp.AsInt32());
			// 	var vResA = Sse2.Subtract(Sse2.Add(vSrcA, vDstA), Sse2.ShiftRightLogical(Sse2.Add(Sse41.MultiplyLow(vSrcA, vDstA), constBaseShift), Color8.BaseShift));
			//
			// 	var rr = Sse2.ShiftLeftLogical(vResR, (byte)(orderR << 3)).AsByte();
			// 	var gg = Sse2.ShiftLeftLogical(vResG, (byte)(orderG << 3)).AsByte();
			// 	var bb = Sse2.ShiftLeftLogical(vResB, (byte)(orderB << 3)).AsByte();
			// 	var aa = Sse2.ShiftLeftLogical(vResA, (byte)(orderA << 3)).AsByte();
			// 	
			// 	Sse2.Store(pDst, Sse2.Or(rr, Sse2.Or(gg, Sse2.Or(bb, aa))));
			// 	length -= 4;
			// }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BlendPixel4_32(BlenderColor32 bl, byte* pDst, int length, Color8 color, byte* covers)
		{
			bl.BlendPixel4(ref pDst, ref length, color, covers);
		}
	}
}
