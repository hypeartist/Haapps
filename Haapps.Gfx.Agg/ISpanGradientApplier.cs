using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe interface ISpanGradientApplier<TColor, TDataItem, TSpanGradientDataProvider>
		where TColor : unmanaged, IColor
		where TDataItem : unmanaged
		where TSpanGradientDataProvider : unmanaged, ISpanGradientDataProvider<TDataItem>
	{
		void Apply(TColor* span, ref TSpanGradientDataProvider dataProvider, int d);
	}
}