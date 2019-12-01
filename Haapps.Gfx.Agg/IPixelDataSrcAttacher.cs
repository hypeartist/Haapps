namespace Haapps.Gfx.Agg
{
	public interface IPixelDataSrcAttacher<TPixelDataSrc>
		where TPixelDataSrc : unmanaged
	{
		void Attach(ref TPixelDataSrc dataSrc);
	}
}