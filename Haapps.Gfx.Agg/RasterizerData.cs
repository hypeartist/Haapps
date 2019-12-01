using System;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed class RasterizerData<TCell> : IDisposable
		where TCell : unmanaged, ICell<TCell>
	{
		private RefPodList<TCell> _cells = new RefPodList<TCell>(128);
		private RefPodArray<SortedY> _sortedY = new RefPodArray<SortedY>(128);
		private RefPodArray<PodPtr<TCell>> _sortedCells = new RefPodArray<PodPtr<TCell>>(128);

		public ref RefPodList<TCell> Cells => ref _cells;

		public ref RefPodArray<SortedY> SortedY => ref _sortedY;

		public ref RefPodArray<PodPtr<TCell>> SortedCells => ref _sortedCells;

		public void Dispose()
		{
			_cells.Dispose();
			_sortedY.Dispose();
			_sortedCells.Dispose();
		}
	}
}