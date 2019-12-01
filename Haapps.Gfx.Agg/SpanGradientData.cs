using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanGradientData<T> : ISpanGradientDataProvider<T>
		where T:unmanaged
	{
		private readonly RefPodArray<T>* _data;

		public SpanGradientData(RefPodArrayAdapter<T> data) => _data = (RefPodArray<T>*) Unsafe.AsPointer(ref data.Array);

		public int Size => _data->Size;

		public T this[int pos]
		{
			get => (*_data)[pos];
			set => (*_data)[pos] = value;
		}
	}
}