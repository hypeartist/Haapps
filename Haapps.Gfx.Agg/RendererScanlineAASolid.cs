using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RendererScanlineAASolid<TColor, TRendererBase> : IRendererScanline<TColor>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private readonly TRendererBase* _renderer;

		public RendererScanlineAASolid(ref TRendererBase renderer) : this() => _renderer = (TRendererBase*) Unsafe.AsPointer(ref renderer);

		public TColor Color { get; set; }

		public void Prepare()
		{
		}

		public void Render<TScanline>(ref TScanline scanline) 
			where TScanline : unmanaged, IScanline => RenderScanline(ref scanline, ref *_renderer, Color);

		private static void RenderScanline<TScanline>(ref TScanline scanline, ref TRendererBase renderer, TColor color)
			where TScanline : unmanaged, IScanline
		{
			var y = scanline.Y;
			var spans = scanline.GetSpans(out var count);

			for (var i = 0; i < count; i++)
			{
				var span = spans[i];
				var x = span.X;
				var length = span.Length;
				if (length > 0)
				{
					renderer.BlendSolidHSpan(x, y, length, color, span.Covers);
				}
				else
				{
					renderer.BlendHLine(x, y, x - length - 1, color, *span.Covers);
				}
			}
		}

		public static void RenderScanlines<TRasterizerScanline, TScanline>(ref TRasterizerScanline rasterizer, ref TRendererBase renderer, TColor color)
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

			while (rasterizer.SweepScanline(ref scanline))
			{
				var y = scanline.Y;
				var spans = scanline.GetSpans(out var count);

				for (var i = 0; i < count; i++)
				{
					var span = spans[i];
					var x = span.X;
					var length = span.Length;
					if (length > 0)
					{
						renderer.BlendSolidHSpan(x, y, length, color, span.Covers);
					}
					else
					{
						renderer.BlendHLine(x, y, x - length - 1, color, *span.Covers);
					}
				}
			}
		}
	}
}