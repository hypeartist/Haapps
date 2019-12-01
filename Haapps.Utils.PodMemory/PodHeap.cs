using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DEBUG
#endif

namespace Haapps.Utils.PodMemory
{
	public static unsafe class PodHeap
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static void* AllocateRaw<T>(int count) where T : unmanaged
		{
			var size = count * sizeof(T);
			var ptr = PodHeapNative.Allocate(size);
			return ptr.ToPointer();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static void* AllocateRaw(int size)
		{
			var ptr = PodHeapNative.Allocate(size);
			return ptr.ToPointer();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static void Free(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero) return;
			PodHeapNative.Free(ptr);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static void Free(void* ptr)
		{
			if (ptr == (void*) 0) return;
			PodHeapNative.Free((IntPtr)ptr);
		}
	}

	internal static class PodHeapNative
	{
#if DEBUG
		// ReSharper disable once CollectionNeverQueried.Local
		private static readonly Dictionary<IntPtr, bool> History = new Dictionary<IntPtr, bool>();
#endif
#if LINUX
// TODO: Linux-based impl

			public static void Init() => throw new NotImplementedException();

			public static IntPtr Allocate(int size) => throw new NotImplementedException();
	
			public static void Free(IntPtr ptr) => throw new NotImplementedException();

			public static void Destroy() => throw new NotImplementedException();
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static unsafe IntPtr Allocate(int size)
		{
			const int alignment = 16;
			var requestSize = size + alignment;
			var buf = Marshal.AllocHGlobal((IntPtr) requestSize);
			var remainder = (buf.ToInt64()) % alignment;
			var offset = (byte)(alignment - remainder);
			var ret = (byte*)buf + offset;
			*(ret - 1) = offset;
			var p = (IntPtr)ret;
#if DEBUG
			History[p] = false;
#endif
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static unsafe void Free(IntPtr ptr)
		{
#if DEBUG
			History[ptr] = true;
#endif
			int offset = *((byte*)ptr - 1);
			Marshal.FreeHGlobal((IntPtr) ((byte*)ptr - offset));
		}
#endif
	}
}
