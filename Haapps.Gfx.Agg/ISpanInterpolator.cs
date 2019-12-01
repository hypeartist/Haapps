namespace Haapps.Gfx.Agg
{
	public interface ISpanInterpolator<TTransform>
		where TTransform : unmanaged, ITransform
	{
		ref TTransform Transform { get; }
		void Begin(double x, double y, int length);
		void Inc();
		void Coordinates(out int x, out int y);
	}
}