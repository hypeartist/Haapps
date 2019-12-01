using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public interface IRasterizerScanline : IRasterizer
	{
		public int MinX { get; }
		public int MinY { get; }
		public int MaxX { get; }
		public int MaxY { get; }
		public byte ApplyGamma(byte cover);
		public void Reset();
		public bool SweepScanline<TScanline>(ref TScanline sl, int misc = 0)
			where TScanline : unmanaged, IScanline;
		public bool RewindScanlines();
	}
}