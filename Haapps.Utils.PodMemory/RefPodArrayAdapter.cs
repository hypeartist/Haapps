using System;

namespace Haapps.Utils.PodMemory
{
	public sealed class RefPodArrayAdapter<T> : IDisposable
		where T : unmanaged
	{
		private RefPodArray<T> _array;

		public RefPodArrayAdapter(int size) => _array.Reallocate(size);

		public ref RefPodArray<T> Array => ref _array;

		public void Dispose() => _array.Dispose();
	}
}