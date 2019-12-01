using System;
using System.Runtime.CompilerServices;

namespace Haapps.Utils.PodMemory
{
	public unsafe struct RefPodArray<T> : IDisposable
		where T:unmanaged
	{
		public RefPodArray(int size)
		{
			DataPtr = (T*) 0;
			Size = size;
			Reallocate(size);
		}

		public int Size { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }

		public ref T this[int pos]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref DataPtr[pos];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Reallocate(int size)
		{
			if (DataPtr == null)
			{
				DataPtr = (T*) PodHeap.AllocateRaw<T>(Size = size);
				return;
			}
			PodHeap.Free(DataPtr);
			DataPtr = (T*) PodHeap.AllocateRaw<T>(size);
			Size = size;
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
		public void Dispose() => PodHeap.Free(DataPtr);
	}
}