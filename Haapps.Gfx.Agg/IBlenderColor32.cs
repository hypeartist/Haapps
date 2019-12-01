namespace Haapps.Gfx.Agg
{
	public unsafe interface IBlenderColor32<TOrderColor> : IBlenderColor<TOrderColor>
		where TOrderColor : unmanaged, IOrderColor32
	{
		void BlendPixel4(ref byte* pDst, ref int length, Color8 color, byte cover);
		void BlendPixel4(ref byte* pDst, ref int length, Color8 color, ref byte* covers);
	}
}