using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanGradientAlphaApplier<TColor, TSpanGradientDataProvider> : ISpanGradientApplier<TColor, byte, TSpanGradientDataProvider>
		where TColor : unmanaged, IColor
		where TSpanGradientDataProvider : unmanaged, ISpanGradientDataProvider<byte>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Apply(TColor* span, ref TSpanGradientDataProvider dataProvider, int d) => span->A = dataProvider[d];
	}
}