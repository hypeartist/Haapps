namespace Haapps.Gfx.Agg
{
	public sealed class ConvStroke<TMarkersGenerator, TVertexSource> : ConvAdaptorVcgenAbstract<VcgenStroke, TMarkersGenerator, TVertexSource>
		where TMarkersGenerator : MarkersGeneratorAbstract, new()
		where TVertexSource : VertexSourceAbstract
	{
		public ConvStroke(TVertexSource source) : base(source)
		{
		}

		public double Shorten
		{
			get => Generator.Shorten;

			set => Generator.Shorten = value;
		}

		public double MiterLimit
		{
			get => Generator.MiterLimit;

			set => Generator.MiterLimit = value;
		}

		public double InnerMiterLimit
		{
			get => Generator.InnerMiterLimit;

			set => Generator.InnerMiterLimit = value;
		}

		public double ApproximationScale
		{
			get => Generator.ApproximationScale;

			set => Generator.ApproximationScale = value;
		}

		public LineJoin LineJoin
		{
			get => Generator.LineJoin;

			set => Generator.LineJoin = value;
		}

		public LineCap LineCap
		{
			get => Generator.LineCap;

			set => Generator.LineCap = value;
		}

		public InnerJoin InnerJoin
		{
			get => Generator.InnerJoin;

			set => Generator.InnerJoin = value;
		}

		public double Width
		{
			get => Generator.Width;

			set => Generator.Width = value;
		}

		public double MiterLimitTheta
		{
			set => Generator.MiterLimitTheta = value;
		}
	}
}