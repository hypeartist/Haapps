namespace Haapps.Gfx.Agg
{
	public unsafe interface IMaskFunction<TOrderColor>
		where TOrderColor : struct, IOrderColor
	{
		byte Calculate(byte* p);
	}
}