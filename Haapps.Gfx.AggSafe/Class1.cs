using System;

namespace Haapps.Gfx.AggSafe
{
	public interface IPixelDataAccessor
	{
		int BytesPerPixel { get; }
		int Width { get; }
		int Height { get; }
		int Stride { get; }
		ref byte GetRowPtr(int y);
		ref byte GetRowPtr(int x, int y, int length);
		ref byte GetPixPtr(int x, int y);
		RowInfo GetRowInfo(int y);
	}
}
