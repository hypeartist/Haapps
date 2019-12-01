namespace Haapps.Gfx.Agg
{
	public unsafe interface IBlenderColor<TColorOrder>
		where TColorOrder : unmanaged, IOrderColor
	{
		void BlendPixel(byte* p, int r, int g, int b, int a);
		void BlendPixel(byte* p, int r, int g, int b, int a, int cover);
	}
}