namespace Haapps.Gfx.Agg
{
	public unsafe interface ISpanGenerator<TColor>
		where TColor : unmanaged, IColor
	{
		void Prepare();
		void Generate(TColor* span, int x, int y, int length);
	}
}