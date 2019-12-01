using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct OneComponentMaskU8<TOrder> : IMaskFunction<TOrder>
		where TOrder : struct, IOrderColor
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public byte Calculate(byte* p) => *p;
	}
}