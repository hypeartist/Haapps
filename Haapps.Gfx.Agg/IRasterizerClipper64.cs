using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public interface IRasterizerClipper64<TRasConv> : IRasterizerClipper<TRasConv>
		where TRasConv : unmanaged, IRasterizerConverter64
	{
		void ResetClipping();
		void ClipBox(double x1, double y1, double x2, double y2);
		void MoveTo(double x1, double y1);
		internal void LineTo(ref RasterizerCellsAA<Cell> ras, double x2, double y2);
	}
}