using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanGradientApplier<TColor, TSpanGradientDataProvider> : ISpanGradientApplier<TColor, TColor, TSpanGradientDataProvider>
		where TColor : unmanaged, IColor
		where TSpanGradientDataProvider : unmanaged, ISpanGradientDataProvider<TColor>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Apply(TColor* span, ref TSpanGradientDataProvider dataProvider, int d) => *span = dataProvider[d];
	}
}