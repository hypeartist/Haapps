using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererScanlineAAGradient<TColor, TRendererBase, TSpanGenerator> : IRendererScanline
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
		where TSpanGenerator : unmanaged, ISpanGenerator<TColor>
	{
		private readonly TSpanGenerator* _generator;
		private readonly TRendererBase* _renderer;

		public RendererScanlineAAGradient(ref TRendererBase renderer, ref TSpanGenerator generator)
		{
			_renderer = (TRendererBase*) Unsafe.AsPointer(ref renderer);
			_generator = (TSpanGenerator*) Unsafe.AsPointer(ref generator);
		}

		public readonly void Prepare() => _generator->Prepare();

		public readonly void Render<TScanline>(ref TScanline scanline) 
			where TScanline : unmanaged, IScanline => RenderScanline(ref scanline, ref *_renderer, ref *_generator);

		public static void RenderScanline<TScanline>(ref TScanline scanline, ref TRendererBase renderer, ref TSpanGenerator generator)
			where TScanline : unmanaged, IScanline
		{
			var y = scanline.Y;
			var spans = scanline.GetSpans(out var count);
			for (var i = 0; i < count; i++)
			{
				var span = spans[i];
				var x = span.X;
				var length = span.Length;
				{
					var colors = stackalloc TColor[length];
					generator.Generate(colors, x, y, length);
					renderer.BlendColorHSpan(x, y, length, colors, length < 0 ? null : span.Covers, *span.Covers);
				}
			}
		}
		
		public static void RenderScanlines<TRasterizerScanline, TScanline>(ref TRasterizerScanline rasterizer, ref TRendererBase renderer, ref TSpanGenerator generator)
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
		{
			if (!rasterizer.RewindScanlines())
			{
				return;
			}

			TScanline scanline = default;
			var maxLength = scanline.CalcMaxLength(rasterizer.MinX, rasterizer.MaxX);
			var slSpans = stackalloc Span[maxLength];
			var slCovers = stackalloc byte[maxLength];
			scanline.Reset(slCovers, slSpans);

			generator.Prepare();
			while (rasterizer.SweepScanline(ref scanline))
			{
				RenderScanline(ref scanline, ref renderer, ref generator);
			}
		}
	}
}