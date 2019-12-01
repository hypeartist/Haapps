using System.Runtime.CompilerServices;

namespace Haapps.Utils.PodMemory
{
	public unsafe struct PodSpan<T>
		where T:unmanaged
	{
		public PodSpan(T* data, int size)
		{
			DataPtr = data;
			Size = size;
		}

		public int Size { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }

		public ref T this[int pos]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref DataPtr[(uint)pos];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Zero() => Unsafe.InitBlock(DataPtr, 0, (uint) (Size * sizeof(T)));

		public T* DataPtr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get;
			private set;
		}

#if DEBUG
		public System.Collections.Generic.List<T> PlainList
		{
			get
			{
				var list = new System.Collections.Generic.List<T>();
				for (var i = 0; i < Size; i++)
				{
					list.Add(this[i]);
				}

				return list;
			}
		}
#endif
	}
}