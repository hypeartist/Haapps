namespace Haapps.Gfx.Agg
{
	public interface IMaskParams<TOrderColor>
		where TOrderColor : struct, IOrderColor
	{
		int Step { get; }
		int Offset { get; }
	}
}