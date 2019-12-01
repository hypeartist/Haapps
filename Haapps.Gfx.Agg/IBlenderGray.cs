namespace Haapps.Gfx.Agg
{
	public unsafe interface IBlenderGray
	{
		void BlendPixel(byte* p, int cv, int alpha);
		void BlendPixel(byte* p, int cv, int alpha, int cover);
	}

	public interface IBlenderGray8 : IBlenderGray{}
}