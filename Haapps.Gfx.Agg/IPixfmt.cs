namespace Haapps.Gfx.Agg
{
	public unsafe interface IPixfmt<TColor> : IPixelDataAccessor
		where TColor : unmanaged, IColor
	{
		int BytesPerPixel { get; }
		byte* GetPixPtr(int x, int y);
		TColor Pixel(int x, int y);
		void CopyPixel(int x, int y, TColor color);
		void BlendPixel(int x, int y, TColor color, byte cover);
		void CopyHLine(int x, int y, int length, TColor color);
		void CopyVLine(int x, int y, int length, TColor color);
		void BlendHLine(int x, int y, int length, TColor color, byte cover);
		void BlendVLine(int x, int y, int length, TColor color, byte cover);
		void BlendSolidHSpan(int x, int y, int length, TColor color, byte* covers);
		void BlendSolidVSpan(int x, int y, int length, TColor color, byte* covers);
		void CopyColorHSpan(int x, int y, int length, TColor* colors);
		void CopyColorVSpan(int x, int y, int length, TColor* colors);
		void BlendColorHSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover);
		void BlendColorVSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover);
	}
}