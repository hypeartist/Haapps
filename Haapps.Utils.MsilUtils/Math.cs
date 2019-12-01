using System.Runtime.CompilerServices;

namespace Haapps.Utils.MsilUtils
{
	public static class Math
	{
		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Add<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Sub<T>(T v1, T v2) where T:unmanaged;
		
		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Mul<T>(T v1, T v2) where T:unmanaged;
		
		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Div<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Shr<T>(T v1, int v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Shl<T>(T v1, int v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T And<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Or<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Xor<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern T Not<T>(T v) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern bool Eq<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern bool Gt<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern bool Gte<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern bool Lt<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern bool Lte<T>(T v1, T v2) where T:unmanaged;

		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern float AsF32<T>(T v) where T:unmanaged;
				
		[MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static extern double AsF64<T>(T v) where T:unmanaged;
	}
}
