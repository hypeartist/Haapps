using System;
using System.Runtime.CompilerServices;

namespace Haapps.Utils.PodMemory
{
	public sealed unsafe class PodList<T> : IDisposable
		where T : unmanaged
	{
		public readonly int BlockSize;
		
		private T* _data;

		public PodList(int blockSize, bool preAllocate = true)
		{
			BlockSize = blockSize;
			_data = default;
			Count = Size = 0;
			if (!preAllocate) return;
			_data = (T*) PodHeap.AllocateRaw<T>(blockSize);
			DataPtr = _data;
			Size = blockSize;
		}

		public int Count { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }

		public int Size { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }
		
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Add(T element)
		{
			if (Count + 1 > Size)
			{
				var tmp = (T*) PodHeap.AllocateRaw<T>(Size = Count + BlockSize);
				Unsafe.CopyBlock(tmp, _data, (uint) (Count * sizeof(T)));
				PodHeap.Free(_data);
				_data = tmp;
				DataPtr = _data;
			}

			_data[Count++] = element;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Add(ref T element)
		{
			if (Count + 1 > Size)
			{
				var tmp = (T*) PodHeap.AllocateRaw<T>(Size = Count + BlockSize);
				Unsafe.CopyBlock(tmp, _data, (uint) (Count * sizeof(T)));
				PodHeap.Free(_data);
				_data = tmp;
				DataPtr = _data;
			}

			_data[Count++] = element;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Clear() => Count = 0;

		public ref T this[int pos]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref _data[pos];
		}

		public void RemoveLast() => --Count;

		public T* DataPtr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] 
			private set;
		}

#if DEBUG
		public System.Collections.Generic.List<T> PlainList
		{
			get
			{
				var list = new System.Collections.Generic.List<T>();
				for (var i = 0; i < Count; i++)
				{
					list.Add(this[i]);
				}

				return list;
			}
		}
#endif
		public void Dispose()
		{
			Count = Size = 0;
			PodHeap.Free(_data);
		}
	}
}