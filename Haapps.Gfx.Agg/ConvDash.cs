namespace Haapps.Gfx.Agg
{
	public sealed class ConvDash<TMarkersGenerator, TVertexSource> : ConvAdaptorVcgenAbstract<VcgenDash, TMarkersGenerator, TVertexSource>
		where TMarkersGenerator : MarkersGeneratorAbstract, new()
		where TVertexSource : VertexSourceAbstract
	{
		public ConvDash(TVertexSource source) : base(source)
		{
		}

		public double Shorten
		{
			get => Generator.Shorten;

			set => Generator.Shorten = value;
		}

		public void AddDash(double dashLen, double gapLen) => Generator.AddDash(dashLen, gapLen);

		public void DashStart(double ds) => Generator.DashStart(ds);

		public void RemoveAllDashes() => Generator.RemoveAllDashes();
	}
}