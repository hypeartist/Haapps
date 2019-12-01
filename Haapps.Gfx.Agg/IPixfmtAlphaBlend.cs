namespace Haapps.Gfx.Agg
{
	public unsafe interface IPixfmtAlphaBlend<TColor>
		where TColor : unmanaged, IColor
	{
		void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor color, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor;
		void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor* colorLUT, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor;
		void BlendFrom<TPixfmtAlphaBlendColor>(TPixfmtAlphaBlendColor src, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlendColor : unmanaged, IPixfmt<TColor>;
	}
}