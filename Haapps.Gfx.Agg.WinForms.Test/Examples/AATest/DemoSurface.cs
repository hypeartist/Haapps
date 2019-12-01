using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Test.Examples.AATest
{
	using TTPixfmt = PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>;
	using TTRendererBase = RendererBase<Color8, PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>>;
	using TTRasterizerScanline = RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>;
	using TTRendererScanlineAASolid = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>>>;
	using TTSpanGradient = SpanGradient<Color8, Color8, SpanGradientData<Color8>, SpanGradientApplier<Color8, SpanGradientData<Color8>>, TransformAffine, SpanInterpolatorLinear<TransformAffine>, Gradients.X>;
	
	public unsafe partial class DemoSurface : Surface
	{
		private readonly Ellipse _ellipse = new Ellipse();
		private bool _isSpeedTestMode;
		private readonly ExecutionTimer _timer = new ExecutionTimer();
		private readonly RasterizerData<Cell> _rasterizerData = new RasterizerData<Cell>();
		private readonly DashedLine<RendererScanlineAASolid<Color8, TTRendererBase>, TTRasterizerScanline, ScanlineU> _dash = new DashedLine<TTRendererScanlineAASolid, TTRasterizerScanline, ScanlineU>();
		private readonly DashedLine<RendererScanlineAAGradient<Color8, TTRendererBase, SpanGradient<Color8, Color8, SpanGradientData<Color8>, SpanGradientApplier<Color8, SpanGradientData<Color8>>, TransformAffine, SpanInterpolatorLinear<TransformAffine>, Gradients.X>>, TTRasterizerScanline, ScanlineU> _dashWithGradient = new DashedLine<RendererScanlineAAGradient<Color8, TTRendererBase, TTSpanGradient>, TTRasterizerScanline, ScanlineU>();
		private readonly RefPodArrayAdapter<Color8> _colorData = new RefPodArrayAdapter<Color8>(256);

		public DemoSurface()
		{
			InitializeComponent();

			PixelFormat = Pixmap.Format.BGRA32;
		}

		protected override void OnResize(EventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var pixfmt = new TTPixfmt(ref RenderingBuffer);
			var rendererBaseColor = new TTRendererBase(ref pixfmt);

			rendererBaseColor.Clear(new Color8(0, 0, 0));

			var rendererSolidColor = new TTRendererScanlineAASolid(ref rendererBaseColor);
			
			var rasterizer = new TTRasterizerScanline(_rasterizerData);

			var gradientTransform = new TransformAffine();
			var gradientInterpolator = new SpanInterpolatorLinear<TransformAffine>(ref gradientTransform);

			// var tmp = stackalloc Color8[256];
			var colorFunction = new SpanGradientData<Color8>(_colorData);//new PodSpan<Color8>(tmp, 256);
			var gradient = new TTSpanGradient(ref gradientInterpolator, ref colorFunction, 0, 100);

			var gradientRenderer = new RendererScanlineAAGradient<Color8, TTRendererBase, TTSpanGradient>(ref rendererBaseColor, ref gradient);
			
			_dashWithGradient.Init(ref rasterizer, ref gradientRenderer);
			_dash.Init(ref rasterizer, ref rendererSolidColor);

			var spanGouraud = SpanGouraudColor.CreateAndInitialize();
			var rendererGouraud = new RendererScanlineAAGradient<Color8, TTRendererBase, SpanGouraudColor>(ref rendererBaseColor, ref spanGouraud);

			
			if (!_isSpeedTestMode)
			{
				var cx = Width / 2.0;
				var cy = Height / 2.0;

				rendererSolidColor.Color = new Color8(255, 255, 255, 50);
				for (var i = 180; i > 0; i--)
				{
					var n = Common.PiMul2 * i / 180.0;
					_dash.Draw(cx + Math.Min(cx, cy) * Math.Sin(n), cy + Math.Min(cx, cy) * Math.Cos(n), cx, cy, 1.0, (i < 90) ? i : 0.0);
				}

				rendererSolidColor.Color = new Color8(255, 255, 255);

				for (var i = 0; i < 20; i++)
				{
					rendererSolidColor.Color = new Color8(255, 255, 255);

					_ellipse.Init(20.0 + i * (i + 1.0) + 0.5, 20.5, i / 2.0, i / 2.0, 8 + i);
					rasterizer.Reset();
					rasterizer.AddPath(_ellipse);
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, TTRendererScanlineAASolid>(ref rasterizer, ref rendererSolidColor);

					_ellipse.Init(18.0 + i * 4.0 + 0.5, 33.0 + 0.5, i / 20.0, i / 20.0, 8);
					rasterizer.Reset();
					rasterizer.AddPath(_ellipse);
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, TTRendererScanlineAASolid>(ref rasterizer, ref rendererSolidColor);

					_ellipse.Init(18.0 + i * 4.0 + (i - 1.0) / 10.0 + 0.5, 27.0 + (i - 1.0) / 10.0 + 0.5, 0.5, 0.5, 8);
					rasterizer.Reset();
					rasterizer.AddPath(_ellipse);
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, TTRendererScanlineAASolid>(ref rasterizer, ref rendererSolidColor);

					FillColorArray(ref colorFunction, new Color8(255, 255, 255), Color8.FromDoubles(i % 2, (i % 3) * 0.5, (i % 5) * 0.25));

					double x1 = 20 + i * (i + 1);
					var y1 = 40.5;
					double x2 = 20 + i * (i + 1) + (i - 1) * 4;
					var y2 = 100.5;

					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, i, 0);

					x1 = 18 + i * 4;
					y1 = 112.5;
					x2 = 18 + i * 4;
					y2 = 112.5 + i / 6.66666667;
					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, 1.0, 0);

					FillColorArray(ref colorFunction, new Color8(255, 0, 0), new Color8(255, 255, 255));
					x1 = 21.5;
					y1 = 120 + (i - 1) * 3.1;
					x2 = 52.5;
					y2 = 120 + (i - 1) * 3.1;
					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, 1.0, 0);

					FillColorArray(ref colorFunction, new Color8(0, 255, 0), new Color8(255, 255, 255));
					x1 = 52.5;
					y1 = 118 + i * 3;
					x2 = 83.5;
					y2 = 118 + i * 3;
					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, 2.0 - (i - 1) / 10.0, 0);

					FillColorArray(ref colorFunction, new Color8(0, 0, 255), new Color8(255, 255, 255));
					x1 = 83.5;
					y1 = 119 + i * 3;
					x2 = 114.5;
					y2 = 119 + i * 3;
					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, 2.0 - (i - 1) / 10.0, 3.0);

					rendererSolidColor.Color = new Color8(255, 255, 255);

					if (i <= 10)
					{
						_dash.Draw(125.5, 119.5 + (i + 2) * (i / 2.0), 135.5, 119.5 + (i + 2) * (i / 2.0), i, 0.0);
					}

					_dash.Draw(17.5 + i * 4, 192, 18.5 + i * 4, 192, i / 10.0, 0);
					_dash.Draw(17.5 + i * 4 + (i - 1) / 10.0, 186, 18.5 + i * 4 + (i - 1) / 10.0, 186, 1.0, 0);
				}

				for (var i = 1; i <= 13; i++)
				{
					FillColorArray(ref colorFunction, new Color8(255, 255, 255), Color8.FromDoubles(i % 2, (i % 3) * 0.5, (i % 5) * 0.25));
					CalcLinearGradientTransform(Width - 150, Height - 20 - i * (i + 1.5), Width - 20, Height - 20 - i * (i + 1), ref gradientTransform);
					rasterizer.Reset();
					rasterizer.MoveToD(Width - 150, Height - 20 - i * (i + 1.5));
					rasterizer.LineToD(Width - 20, Height - 20 - i * (i + 1));
					rasterizer.LineToD(Width - 20, Height - 20 - i * (i + 2));
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, RendererScanlineAAGradient<Color8, TTRendererBase, TTSpanGradient>>(ref rasterizer, ref gradientRenderer);
				}
			}
			else
			{
				_timer.Start();
				for (var i = 0; i < 20000; i++)
				{
					var r = Common.Rand(20.0) + 1.0;
					var wr = Common.Rand(Width);
					var wh = Common.Rand(Height);
					_ellipse.Init(wr, wh, r / 2.0, r / 2.0, (int) r + 10);
					rasterizer.Reset();
					rasterizer.AddPath(_ellipse);
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, TTRendererScanlineAASolid>(ref rasterizer, ref rendererSolidColor);
					rendererSolidColor.Color = Color8.Random(0.5);
				}
				
				_timer.Stop();
				var t1 = _timer.Milliseconds;

				_timer.Start();
				for (var i = 0; i < 2000; i++)
				{
					var x1 = Common.Rand(Width);
					var y1 = Common.Rand(Height);
					var x2 = x1 + Common.Rand(Width >> 1) - Width * 0.25;
					var y2 = y1 + Common.Rand(Height >> 1) - Height * 0.25;
				
					FillColorArray(ref colorFunction, Color8.Random(0.5), Color8.Random(0.5));
					CalcLinearGradientTransform(x1, y1, x2, y2, ref gradientTransform);
					_dashWithGradient.Draw(x1, y1, x2, y2, 10.0, 0);
				}
				
				_timer.Stop();
				var t2 = _timer.Milliseconds;
				
				_timer.Start();
				for (var i = 0; i < 2000; i++)
				{
					var x1 = Common.Rand(Width);
					var y1 = Common.Rand(Height);
					var x2 = x1 + Common.Rand(Width * 0.4) - Width * 0.2;
					var y2 = y1 + Common.Rand(Height * 0.4) - Height * 0.2;
					var x3 = x1 + Common.Rand(Width * 0.4) - Width * 0.2;
					var y3 = y1 + Common.Rand(Height * 0.4) - Height * 0.2;
				
					spanGouraud.Colors(Color8.Random(0.5), Color8.Random(0.5), Color8.Random(0.5));
					spanGouraud.Triangle(x1, y1, x2, y2, x3, y3, 0.0);
					rasterizer.AddPath(spanGouraud.VertexSourceAdaptor);
					Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, RendererScanlineAAGradient<Color8, TTRendererBase, SpanGouraudColor>>(ref rasterizer, ref rendererGouraud);
				}
				
				_timer.Stop();
				var t3 = _timer.Milliseconds;
				
				MessageBox.Show($"Points={20000 / t1} K/sec, Lines={2000 / t2} K/sec, Triangles={2000 / t3} K/sec");
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			_isSpeedTestMode = true;
			ForceRedraw();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static void CalcLinearGradientTransform(double x1, double y1, double x2, double y2, ref TransformAffine mtx, double gradientD2 = 100.0)
		{
			var dx = x2 - x1;
			var dy = y2 - y1;
			mtx.Reset();
			mtx *= TransformAffine.AffineScaling(Math.Sqrt(dx * dx + dy * dy) / gradientD2);
			var a2 = Math.Atan2(dy, dx);
			mtx *= TransformAffine.AffineRotation(a2);
			mtx *= TransformAffine.AffineTranslation(x1 + 0.5, y1 + 0.5);
			mtx.Invert();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private static void FillColorArray(ref SpanGradientData<Color8> array, Color8 begin, Color8 end)
		{
			for (var i = 0.0; i < 256.0; ++i)
			{
				array[(int) i] = begin.Gradient(end, i / 255.0);
			}
		}
	}

	public sealed unsafe class DashedLine<TRendererScanline, TRasterizerScanline, TScanline>
		where TRendererScanline : unmanaged, IRendererScanline
		where TRasterizerScanline : unmanaged, IRasterizerScanline
		where TScanline : unmanaged, IScanline
	{
		private TRasterizerScanline* _rasterizer;
		private TRendererScanline* _renderer;
		private readonly SimpleVertexSource _src = new SimpleVertexSource();
		private readonly ConvDash<NullMarkers, SimpleVertexSource> _dash;
		private readonly ConvStroke<NullMarkers, SimpleVertexSource> _stroke;
		private readonly ConvStroke<NullMarkers, ConvDash<NullMarkers, SimpleVertexSource>> _dashStroke;

		public DashedLine()
		{
			_dash = new ConvDash<NullMarkers, SimpleVertexSource>(_src);
			_stroke = new ConvStroke<NullMarkers, SimpleVertexSource>(_src);
			_dashStroke = new ConvStroke<NullMarkers, ConvDash<NullMarkers, SimpleVertexSource>>(_dash);
		}

		public void Init(ref TRasterizerScanline rasterizer, ref TRendererScanline renderer)
		{
			_rasterizer = (TRasterizerScanline*) Unsafe.AsPointer(ref rasterizer);
			_renderer = (TRendererScanline*) Unsafe.AsPointer(ref renderer);
		}

		public void Draw(double x1, double y1, double x2, double y2, double lineWidth, double dashLength)
		{
			_src.Init(x1 + 0.5, y1 + 0.5, x2 + 0.5, y2 + 0.5);
			_rasterizer->Reset();
			if (dashLength > 0.0)
			{
				_dash.RemoveAllDashes();
				_dash.AddDash(dashLength, dashLength);
				_dashStroke.Width = lineWidth;
				_dashStroke.LineCap = LineCap.Round;
				_rasterizer->AddPath(_dashStroke);
			}
			else
			{
				_stroke.Width = lineWidth;
				_stroke.LineCap = LineCap.Round;
				_rasterizer->AddPath(_stroke);
			}

			Renderer.RenderScanlines<TRasterizerScanline, TScanline, TRendererScanline>(ref *_rasterizer, ref *_renderer);
		}

		public sealed class SimpleVertexSource : VertexSourceAbstract
		{
			private int _count;
			private readonly double[] _x = new double[8];
			private readonly double[] _y = new double[8];
			private readonly PathCommand[] _cmd = new PathCommand[8];

			public SimpleVertexSource()
			{
				_cmd[0] = PathCommand.Stop;
			}

			public SimpleVertexSource(double x1, double y1, double x2, double y2)
			{
				Init(x1, y1, x2, y2);
			}

			public SimpleVertexSource(double x1, double y1, double x2, double y2, double x3, double y3)
			{
				Init(x1, y1, x2, y2, x3, y3);
			}

			public int NumberOfVertices { get; private set; }

			public void Init(double x1, double y1, double x2, double y2)
			{
				NumberOfVertices = 2;
				_count = 0;
				_x[0] = x1;
				_y[0] = y1;
				_x[1] = x2;
				_y[1] = y2;
				_cmd[0] = PathCommand.MoveTo;
				_cmd[1] = PathCommand.LineTo;
				_cmd[2] = PathCommand.Stop;
			}

			public void Init(double x1, double y1, double x2, double y2, double x3, double y3)
			{
				NumberOfVertices = 3;
				_count = 0;
				_x[0] = x1;
				_y[0] = y1;
				_x[1] = x2;
				_y[1] = y2;
				_x[2] = x3;
				_y[2] = y3;
				_x[3] = _y[3] = _x[4] = _y[4] = 0.0;
				_cmd[0] = PathCommand.MoveTo;
				_cmd[1] = PathCommand.LineTo;
				_cmd[2] = PathCommand.LineTo;
				_cmd[3] = PathCommand.EndPoly | (PathCommand) PathFlags.Close;
				_cmd[4] = PathCommand.Stop;
			}

			public override void Rewind(int pathId = 0)
			{
				_count = 0;
			}

			public override PathCommand Vertex(ref double x, ref double y)
			{
				x = _x[_count];
				y = _y[_count];
				return _cmd[_count++];
			}

			public override void Dispose()
			{
			}
		}
	}
}
