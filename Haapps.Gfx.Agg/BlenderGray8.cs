using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public readonly unsafe struct BlenderGray8 : IBlenderGray8
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel(byte* p, int v, int a) => *p = (byte) (((v - *p) * a + (*p << Gray8.BaseShift)) >> Gray8.BaseShift);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void BlendPixel(byte* p, int v, int a, int cover) => BlendPixel(p, v, a);
	}
}