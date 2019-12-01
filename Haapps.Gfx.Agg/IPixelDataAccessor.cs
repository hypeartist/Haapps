namespace Haapps.Gfx.Agg
{
	public unsafe interface IPixelDataAccessor
	{
		int Width { get; }
		int Height { get; }
		int Stride { get; }
		byte* GetRowPtr(int y);
		byte* GetRowPtr(int x, int y, int length);
		RowInfo GetRowInfo(int y);
	}
}