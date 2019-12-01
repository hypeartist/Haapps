using System;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Windows.Forms;

namespace Haapps.Gfx.Agg.WinForms.Test
{
	internal static unsafe class Program
	{
		// struct Rgb : IOrderColor24
		// {
		// 	public int R => 0;
		//
		// 	public int G => 1;
		//
		// 	public int B => 2;
		// }
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			// var p = stackalloc byte[16] {100, 101, 102, 203, 110, 111, 112, 213, 120, 121, 122, 223, 130, 131, 132, 233};
			// var p2 = stackalloc byte[16] {100, 101, 102, 203, 110, 111, 112, 213, 120, 121, 122, 223, 130, 131, 132, 233};
			// var c = stackalloc Color8[4] {new Color8(127, 45, 97, 254), new Color8(234, 32, 100, 253), new Color8(76, 145, 33, 252), new Color8(98, 65, 200, 251)};
			// var v = stackalloc byte[4] {127, 128, 129, 130};
			//
			// Do(p, c);
			//
			// var b = new BlenderColor24<Rgb>();
			// var cc = c;
			//
			// b.BlendPixel(p2, cc->R, cc->G, cc->B, cc->A);
			// // Debug.WriteLine($"1: {p2[0]}, {p2[1]}, {p2[2]}, {p2[3]}");
			// p2 += 3;
			// cc++;
			// b.BlendPixel(p2, cc->R, cc->G, cc->B, cc->A);
			// // Debug.WriteLine($"2: {p2[0]}, {p2[1]}, {p2[2]}, {p2[3]}");
			// p2 += 3;
			// cc++;
			// b.BlendPixel(p2, cc->R, cc->G, cc->B, cc->A);
			// // Debug.WriteLine($"res=({p2[0]},{p2[1]},{p2[2]},{p2[3]})");
			// p2 += 3;
			// cc++;
			// b.BlendPixel(p2, cc->R, cc->G, cc->B, cc->A);
			// // Debug.WriteLine($"4: {p2[0]}, {p2[1]}, {p2[2]}, {p2[3]}");
			//
			// var zeroU16 = Vector128<ushort>.Zero;
			// var maskAlpha = Vector128.Create(0x00FFFFFF).AsByte();
			//
			// var dst = Sse2.LoadAlignedVector128(p);// R1G1B1A1 R2G2B2A2 R3G3B3A3 R4G4B4A4
			// var dstAlpha =  Sse2.And(dst.AsUInt32(), Vector128.Create(0xFF000000)).AsByte();
			// var dstRedBlue = Sse2.And(dst.AsUInt32(), Vector128.Create(0x00FF00FFu));// R1xB1x R2xB2x R3xB3x R4xB4x
			// var dstRedBlueUnpk12 = Sse2.UnpackLow(dstRedBlue.AsUInt16(), zeroU16).AsInt32();// R1xxx B1xxx R2xxx B2xxx
			// var dstRedBlueUnpk34 = Sse2.UnpackHigh(dstRedBlue.AsUInt16(), zeroU16).AsInt32();// R3xxx B3xxx R4xxx B4xxx
			// var dstGreenAlpha = Sse2.ShiftRightLogical128BitLane(Sse2.And(dst.AsUInt32(), Vector128.Create(0xFF00FF00)), 1);// G1xA1x G2xA2x G3xA3x G4xA4x
			// var dstGreenAlphaUnpk12 = Sse2.UnpackLow(dstGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G1xxx A1xxx G2xxx A2xxx
			// var dstGreenAlphaUnpk34 = Sse2.UnpackHigh(dstGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G3xxx A3xxx G4xxx A4xxx
			//
			// var src = Sse2.LoadAlignedVector128((byte*)c);
			// var srcAlpha =  Sse2.ShiftRightLogical128BitLane(Sse2.And(src.AsUInt32(), Vector128.Create(0xFF000000)), 3).AsInt32();
			// srcAlpha = Sse2.Or(srcAlpha, Sse2.ShiftLeftLogical128BitLane(srcAlpha, 2));
			// var srcAlphaUnpk12 = Sse2.UnpackLow(srcAlpha.AsUInt16(), zeroU16).AsInt32();
			// var srcAlphaUnpk34 = Sse2.UnpackHigh(srcAlpha.AsUInt16(), zeroU16).AsInt32();
			// var srcRedBlue = Sse2.And(src.AsUInt32(), Vector128.Create(0x00FF00FFu));// R1xB1x R2xB2x R3xB3x R4xB4x
			// var srcRedBlueUnpk12 = Sse2.UnpackLow(srcRedBlue.AsUInt16(), zeroU16).AsInt32();// R1xxx B1xxx R2xxx B2xxx
			// var srcRedBlueUnpk34 = Sse2.UnpackHigh(srcRedBlue.AsUInt16(), zeroU16).AsInt32();// R3xxx B3xxx R4xxx B4xxx
			// var srcGreenAlpha = Sse2.ShiftRightLogical128BitLane(Sse2.And(src.AsUInt32(), Vector128.Create(0xFF00FF00)), 1);// G1xA1x G2xA2x G3xA3x G4xA4x
			// var srcGreenAlphaUnpk12 = Sse2.UnpackLow(srcGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G1xxx A1xxx G2xxx A2xxx
			// var srcGreenAlphaUnpk34 = Sse2.UnpackHigh(srcGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G3xxx A3xxx G4xxx A4xxx
			//
			// var subRedBlue12 = Sse2.Subtract(srcRedBlueUnpk12, dstRedBlueUnpk12);
			// var subRedBlue34 = Sse2.Subtract(srcRedBlueUnpk34, dstRedBlueUnpk34);
			// var tmpRedBlue12 = Sse41.MultiplyLow(subRedBlue12, srcAlphaUnpk12);
			// tmpRedBlue12 = Sse2.ShiftRightArithmetic(tmpRedBlue12, 8);
			// tmpRedBlue12 = Sse2.Add(tmpRedBlue12, dstRedBlueUnpk12);
			// var tmpRedBlue34 = Sse41.MultiplyLow(subRedBlue34, srcAlphaUnpk34);
			// tmpRedBlue34 = Sse2.ShiftRightArithmetic(tmpRedBlue34, 8);
			// tmpRedBlue34 = Sse2.Add(tmpRedBlue34, dstRedBlueUnpk34);
			//
			//
			// var subGreenAlpha12 = Sse2.Subtract(srcGreenAlphaUnpk12, dstGreenAlphaUnpk12);
			// var subGreenAlpha34 = Sse2.Subtract(srcGreenAlphaUnpk34, dstGreenAlphaUnpk34);
			// var tmpGreenAlpha12 = Sse41.MultiplyLow(subGreenAlpha12, srcAlphaUnpk12);
			// tmpGreenAlpha12 = Sse2.ShiftRightArithmetic(tmpGreenAlpha12, 8);
			// tmpGreenAlpha12 = Sse2.Add(tmpGreenAlpha12, dstGreenAlphaUnpk12);
			// var tmpGreenAlpha34 = Sse41.MultiplyLow(subGreenAlpha34, srcAlphaUnpk34);
			// tmpGreenAlpha34 = Sse2.ShiftRightArithmetic(tmpGreenAlpha34, 8);
			// tmpGreenAlpha34 = Sse2.Add(tmpGreenAlpha34, dstGreenAlphaUnpk34);
			//
			// var pckRedBlue = Sse2.PackSignedSaturate(tmpRedBlue12, tmpRedBlue34).AsByte();
			// var pckGreenAlpha = Sse2.PackSignedSaturate(tmpGreenAlpha12, tmpGreenAlpha34).AsByte();
			// pckGreenAlpha = Sse2.ShiftLeftLogical128BitLane(pckGreenAlpha, 1);
			//
			// var res = Sse2.Or(Sse2.And(Sse2.Or(pckRedBlue, pckGreenAlpha), maskAlpha), dstAlpha);
			// Sse2.StoreAlignedNonTemporal(p, res);

			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TestForm());
		}
		//
		// public static void Do(byte* p, Color8* c)
		// {
		// 	var zeroU16 = Vector128<ushort>.Zero;
		// 	var maskAlpha = Vector128.Create(0x00FFFFFF).AsByte();
		//
		// 	var dst = Sse2.LoadAlignedVector128(p);// R1G1B1A1 R2G2B2A2 R3G3B3A3 R4G4B4A4
		// 	var dstAlpha = Sse2.And(dst.AsUInt32(), Vector128.Create(0xFF000000)).AsByte();
		// 	var dstRedBlue = Sse2.And(dst.AsUInt32(), Vector128.Create(0x00FF00FFu));// R1xB1x R2xB2x R3xB3x R4xB4x
		// 	var dstRedBlueUnpk12 = Sse2.UnpackLow(dstRedBlue.AsUInt16(), zeroU16).AsInt32();// R1xxx B1xxx R2xxx B2xxx
		// 	var dstRedBlueUnpk34 = Sse2.UnpackHigh(dstRedBlue.AsUInt16(), zeroU16).AsInt32();// R3xxx B3xxx R4xxx B4xxx
		// 	var dstGreenAlpha = Sse2.ShiftRightLogical128BitLane(Sse2.And(dst.AsUInt32(), Vector128.Create(0xFF00FF00)), 1);// G1xA1x G2xA2x G3xA3x G4xA4x
		// 	var dstGreenAlphaUnpk12 = Sse2.UnpackLow(dstGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G1xxx A1xxx G2xxx A2xxx
		// 	var dstGreenAlphaUnpk34 = Sse2.UnpackHigh(dstGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G3xxx A3xxx G4xxx A4xxx
		//
		// 	var src = Sse2.LoadAlignedVector128((byte*)c);
		// 	var srcAlpha = Sse2.ShiftRightLogical128BitLane(Sse2.And(src.AsUInt32(), Vector128.Create(0xFF000000)), 3).AsInt32();
		// 	srcAlpha = Sse2.Or(srcAlpha, Sse2.ShiftLeftLogical128BitLane(srcAlpha, 2));
		// 	var srcAlphaUnpk12 = Sse2.UnpackLow(srcAlpha.AsUInt16(), zeroU16).AsInt32();
		// 	var srcAlphaUnpk34 = Sse2.UnpackHigh(srcAlpha.AsUInt16(), zeroU16).AsInt32();
		// 	var srcRedBlue = Sse2.And(src.AsUInt32(), Vector128.Create(0x00FF00FFu));// R1xB1x R2xB2x R3xB3x R4xB4x
		// 	var srcRedBlueUnpk12 = Sse2.UnpackLow(srcRedBlue.AsUInt16(), zeroU16).AsInt32();// R1xxx B1xxx R2xxx B2xxx
		// 	var srcRedBlueUnpk34 = Sse2.UnpackHigh(srcRedBlue.AsUInt16(), zeroU16).AsInt32();// R3xxx B3xxx R4xxx B4xxx
		// 	var srcGreenAlpha = Sse2.ShiftRightLogical128BitLane(Sse2.And(src.AsUInt32(), Vector128.Create(0xFF00FF00)), 1);// G1xA1x G2xA2x G3xA3x G4xA4x
		// 	var srcGreenAlphaUnpk12 = Sse2.UnpackLow(srcGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G1xxx A1xxx G2xxx A2xxx
		// 	var srcGreenAlphaUnpk34 = Sse2.UnpackHigh(srcGreenAlpha.AsUInt16(), zeroU16).AsInt32();// G3xxx A3xxx G4xxx A4xxx
		//
		// 	var subRedBlue12 = Sse2.Subtract(srcRedBlueUnpk12, dstRedBlueUnpk12);
		// 	var subRedBlue34 = Sse2.Subtract(srcRedBlueUnpk34, dstRedBlueUnpk34);
		// 	var tmpRedBlue12 = Sse41.MultiplyLow(subRedBlue12, srcAlphaUnpk12);
		// 	tmpRedBlue12 = Sse2.ShiftRightArithmetic(tmpRedBlue12, 8);
		// 	tmpRedBlue12 = Sse2.Add(tmpRedBlue12, dstRedBlueUnpk12);
		// 	var tmpRedBlue34 = Sse41.MultiplyLow(subRedBlue34, srcAlphaUnpk34);
		// 	tmpRedBlue34 = Sse2.ShiftRightArithmetic(tmpRedBlue34, 8);
		// 	tmpRedBlue34 = Sse2.Add(tmpRedBlue34, dstRedBlueUnpk34);
		//
		//
		// 	var subGreenAlpha12 = Sse2.Subtract(srcGreenAlphaUnpk12, dstGreenAlphaUnpk12);
		// 	var subGreenAlpha34 = Sse2.Subtract(srcGreenAlphaUnpk34, dstGreenAlphaUnpk34);
		// 	var tmpGreenAlpha12 = Sse41.MultiplyLow(subGreenAlpha12, srcAlphaUnpk12);
		// 	tmpGreenAlpha12 = Sse2.ShiftRightArithmetic(tmpGreenAlpha12, 8);
		// 	tmpGreenAlpha12 = Sse2.Add(tmpGreenAlpha12, dstGreenAlphaUnpk12);
		// 	var tmpGreenAlpha34 = Sse41.MultiplyLow(subGreenAlpha34, srcAlphaUnpk34);
		// 	tmpGreenAlpha34 = Sse2.ShiftRightArithmetic(tmpGreenAlpha34, 8);
		// 	tmpGreenAlpha34 = Sse2.Add(tmpGreenAlpha34, dstGreenAlphaUnpk34);
		//
		// 	var pckRedBlue = Sse2.PackSignedSaturate(tmpRedBlue12, tmpRedBlue34).AsByte();
		// 	var pckGreenAlpha = Sse2.PackSignedSaturate(tmpGreenAlpha12, tmpGreenAlpha34).AsByte();
		// 	pckGreenAlpha = Sse2.ShiftLeftLogical128BitLane(pckGreenAlpha, 1);
		//
		// 	var res = Sse2.Or(Sse2.And(Sse2.Or(pckRedBlue, pckGreenAlpha), maskAlpha), dstAlpha);
		// 	Sse2.StoreAlignedNonTemporal(p, res);
		// }
	}
}
