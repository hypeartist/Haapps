namespace Haapps.Gfx.Agg
{
	public unsafe interface IAlphaMask
	{
		byte Pixel(int x, int y);
		byte CombinePixel(int x, int y, byte val);
		void FillHSpan(int x, int y, byte* dst, int numPix);
		void CombineHSpan(int x, int y, byte* dst, int numPix);
		void FillVSpan(int x, int y, byte* dst, int numPix);
		void CombineVSpan(int x, int y, byte* dst, int numPix);
	}
}