using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public struct RasConv32 : IRasterizerConverter32
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int MulDiv(double a, double b, double c) => (int) (a * b / c);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int Xi(int v) => v;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int Yi(int v) => v;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int Upscale(double v) => (int) (v * Common.PolySubpixelScale);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public int Downscale(int v) => v;
	}
}