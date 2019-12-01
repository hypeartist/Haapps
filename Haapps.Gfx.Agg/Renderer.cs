namespace Haapps.Gfx.Agg
{
	public static unsafe class Renderer
	{
		
		public static void RenderScanlines<TRasterizerScanline, TScanline, TRendererScanline>(ref TRasterizerScanline rasterizer, ref TRendererScanline renderer)
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
			where TRendererScanline : unmanaged, IRendererScanline
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

			renderer.Prepare();
			while (rasterizer.SweepScanline(ref scanline))
			{
				renderer.Render(ref scanline);
			}
		}

		public static void RenderScanlines<TRasterizerScanline, TScanline, TRendererScanline>(ref TRasterizerScanline rasterizer, ref TScanline scanline, ref TRendererScanline renderer)
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
			where TRendererScanline : unmanaged, IRendererScanline
		{
			if (!rasterizer.RewindScanlines())
			{
				return;
			}

			var maxLength = scanline.CalcMaxLength(rasterizer.MinX, rasterizer.MaxX);
			var slSpans = stackalloc Span[maxLength];
			var slCovers = stackalloc byte[maxLength];
			scanline.Reset(slCovers, slSpans);

			renderer.Prepare();
			while (rasterizer.SweepScanline(ref scanline))
			{
				renderer.Render(ref scanline);
			}
		}

		public static void RenderScanlines<TColor, TRasterizerScanline, TScanline, TRendererBase, TSpanGenerator>(ref TRasterizerScanline rasterizer, ref TRendererBase renderer, ref TSpanGenerator spanGenerator)
			where TColor : unmanaged, IColor
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
			where TRendererBase : unmanaged, IRendererBase<TColor>
			where TSpanGenerator : unmanaged, ISpanGenerator<TColor>
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
		
			spanGenerator.Prepare();
			while (rasterizer.SweepScanline(ref scanline))
			{
				RendererScanlineAAGradient<TColor, TRendererBase, TSpanGenerator>.RenderScanline(ref scanline, ref renderer, ref spanGenerator);
			}
		}

		public static void RenderAllPaths<TColor, TRasterizerScanline, TScanline, TRendererScanline>(ref TRasterizerScanline ras, ref TRendererScanline r, VertexSourceAbstract vs, TColor* cs, int* pathIds, int numPaths)
			where TColor : unmanaged, IColor
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
			where TRendererScanline : unmanaged, IRendererScanline<TColor>
		{
			for (var i = 0; i < numPaths; i++)
			{
				ras.Reset();
				ras.AddPath(vs, pathIds[i]);
				r.Color = cs[i];
				RenderScanlines<TRasterizerScanline, TScanline, TRendererScanline>(ref ras, ref r);
			}
		}

		public static void RenderAllPaths<TColor, TRasterizerScanline, TScanline, TRendererScanline>(ref TRasterizerScanline ras, ref TScanline scanline, ref TRendererScanline r, VertexSourceAbstract vs, TColor* cs, int* pathIds, int numPaths)
			where TColor : unmanaged, IColor
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TScanline : unmanaged, IScanline
			where TRendererScanline : unmanaged, IRendererScanline<TColor>
		{
			for (var i = 0; i < numPaths; i++)
			{
				ras.Reset();
				ras.AddPath(vs, pathIds[i]);
				r.Color = cs[i];
				RenderScanlines(ref ras, ref scanline, ref r);
			}
		}
	}
}