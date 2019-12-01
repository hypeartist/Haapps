using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanConverter<TColor, TSpanGeneratorSrc, TSpanGeneratorDst> : ISpanGenerator<TColor>
		where TColor : unmanaged, IColor
		where TSpanGeneratorSrc : unmanaged, ISpanGenerator<TColor>
		where TSpanGeneratorDst : unmanaged, ISpanGenerator<TColor>
	{
		private readonly TSpanGeneratorSrc* _src;
		private readonly TSpanGeneratorDst* _dst;

		public SpanConverter(ref TSpanGeneratorSrc src, ref TSpanGeneratorDst dst)
		{
			_src = (TSpanGeneratorSrc*) Unsafe.AsPointer(ref src);
			_dst = (TSpanGeneratorDst*) Unsafe.AsPointer(ref dst);
		}

		public void Prepare()
		{
			_src->Prepare();
			_dst->Prepare();
		}

		public void Generate(TColor* span, int x, int y, int length)
		{
			_src->Generate(span, x, y, length);
			_dst->Generate(span, x, y, length);
		}
	}
}