namespace Haapps.Gfx.Agg
{
	public unsafe interface IBlenderColor24<TOrderColor> : IBlenderColor<TOrderColor>
		where TOrderColor : unmanaged, IOrderColor24
	{
		
		void BlendPixel4(byte* pDst, Color8 pSrc, byte cover);
		void BlendPixel4(byte* pDst, Color8 pSrc, byte* covers);
		void BlendPixel4(byte* pDst, Color8* pSrc, byte cover);
	}
}