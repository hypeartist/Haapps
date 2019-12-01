using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public interface IRasterizerClipper32<TRasConv> : IRasterizerClipper<TRasConv>
		where TRasConv : unmanaged, IRasterizerConverter32
	{
		void ResetClipping();
		void ClipBox(int x1, int y1, int x2, int y2);
		void MoveTo(int x1, int y1);
		internal void LineTo(ref RasterizerCellsAA<Cell> ras, int x2, int y2);
	}
}