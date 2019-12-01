using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public struct RasterizerSlNoClip64<TRasConv> : IRasterizerClipper<TRasConv>
		where TRasConv : unmanaged, IRasterizerConverter64
	{
		private double _x1;
		private double _y1;

		public void ResetClipping()
		{
		}

		public void ClipBox(double x1, double y1, double x2, double y2)
		{
		}

		public void MoveTo(double x1, double y1)
		{
			_x1 = x1;
			_y1 = y1;
		}

		internal void LineTo(ref RasterizerCellsAA<Cell> ras, double x2, double y2)
		{
			ras.Line(_x1, _y1, x2, y2);
			_x1 = x2;
			_y1 = y2;
		}
	}
}