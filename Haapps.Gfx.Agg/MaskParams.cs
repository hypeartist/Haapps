using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public static class MaskParams
	{
		public struct Default<TOrder> : IMaskParams<TOrder>
			where TOrder : struct, IOrderColor24
		{
			public int Step
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => 1;
			}

			public int Offset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => 0;
			}
		}

		public struct R<TOrder> : IMaskParams<TOrder>
			where TOrder : struct, IOrderColor24
		{
			public int Step
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => 3;
			}

			public int Offset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => default(TOrder).R;
			}
		}

		public struct G<TOrder> : IMaskParams<TOrder>
			where TOrder : struct, IOrderColor24
		{
			public int Step
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => 3;
			}

			public int Offset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => default(TOrder).G;
			}
		}

		public struct B<TOrder> : IMaskParams<TOrder>
			where TOrder : struct, IOrderColor24
		{
			public int Step
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => 3;
			}

			public int Offset
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
				get => default(TOrder).B;
			}
		}
	}
}