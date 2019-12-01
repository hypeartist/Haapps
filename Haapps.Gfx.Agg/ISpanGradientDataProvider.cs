namespace Haapps.Gfx.Agg
{
	public interface ISpanGradientDataProvider<out TDataItem>
		where TDataItem : unmanaged
	{
		int Size { get; }
		TDataItem this[int pos] { get; }
	}
}