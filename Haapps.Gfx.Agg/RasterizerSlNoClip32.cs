using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public struct RasterizerSlNoClip32<TRasConv> : IRasterizerClipper32<TRasConv>
		where TRasConv : unmanaged, IRasterizerConverter32
	{
		private int _x1;
		private int _y1;

		public void ResetClipping()
		{
		}

		public void ClipBox(int x1, int y1, int x2, int y2)
		{
		}

		public void MoveTo(int x1, int y1)
		{
			_x1 = x1;
			_y1 = y1;
		}

		void IRasterizerClipper32<TRasConv>.LineTo(ref RasterizerCellsAA<Cell> ras, int x2, int y2)
		{
			ras.Line(_x1, _y1, x2, y2);
			_x1 = x2;
			_y1 = y2;
		}
	}
}