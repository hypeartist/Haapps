using System.Runtime.CompilerServices;

namespace Haapps.Utils.MsilUtils
{
	public static class Misc
	{
		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern unsafe void* AsPointer<T>(ref T v) where T:struct;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T2 Cast<T1, T2>(T1 v) where T1:unmanaged where T2:unmanaged;
		//
		// [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		// public static extern void CopyBlock<T>(ref T destination, in T source, uint byteCount);
		//
		// [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		// public static extern void InitBlock<T>(ref T startAddress, byte value, uint byteCount);


	}
}