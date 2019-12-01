namespace Haapps.Gfx.Agg
{
	public interface IRendererScanline
	{
		void Render<TScanline>(ref TScanline scanline)
			where TScanline : unmanaged, IScanline;
		void Prepare();
	}

	public interface IRendererScanline<TColor> : IRendererScanline
		where TColor : unmanaged, IColor
	{
		TColor Color { get; set; }
	}
}