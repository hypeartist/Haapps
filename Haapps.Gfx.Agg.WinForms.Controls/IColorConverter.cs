namespace Haapps.Gfx.Agg.WinForms.Controls
{
	public unsafe interface IColorConverter
	{
		void Convert(byte* dst, byte* src, int width);
	}
}