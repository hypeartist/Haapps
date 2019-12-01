namespace Haapps.Gfx.Agg
{
	public unsafe interface IRendererBase<TColor>
		where TColor : unmanaged, IColor
	{
		int Width { get; }
		int Height { get; }
		int MaxX { get; }
		int MaxY { get; }
		int MinX { get; }
		int MinY { get; }
		Rectangle32 ClipBox { get; }
		void BlendPixel(int x, int y, TColor color, byte cover);
		void CopyBar(int x1, int y1, int x2, int y2, TColor c);
		void BlendBar(int x1, int y1, int x2, int y2, TColor c, byte cover);
		void CopyHLine(int x1, int y, int x2, TColor color);
		void CopyVLine(int x, int y1, int y2, TColor color);
		void BlendHLine(int x1, int y, int x2, TColor color, byte cover);
		void BlendVLine(int x, int y1, int y2, TColor color, byte cover);
		void BlendSolidHSpan(int x, int y, int length, TColor color, byte* covers);
		void BlendSolidVSpan(int x, int y, int length, TColor color, byte* covers);
		void CopyColorHSpan(int x, int y, int length, TColor* colors);
		void CopyColorVSpan(int x, int y, int length, TColor* colors);
		void BlendColorHSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover);
		void BlendColorVSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover);
		void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor color, int dx, int dy, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor;
		void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor* colorLUT, int dx, int dy, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor;
		void BlendFrom<TPixfmtAlphaBlendColor2>(TPixfmtAlphaBlendColor2 src, int dx, int dy, byte cover)
			where TPixfmtAlphaBlendColor2 : unmanaged, IPixfmt<TColor>;
	}
}