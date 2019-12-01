namespace Haapps.Gfx.Agg
{
	public interface IPixelDataAccessorAttacher
	{
		bool Attach<TPixelDataAccessor, TColor>(ref TPixelDataAccessor pixfmt, int x1, int y1, int x2, int y2)
			where TPixelDataAccessor : unmanaged, IPixfmt<TColor>
			where TColor : unmanaged, IColor;
	}
}