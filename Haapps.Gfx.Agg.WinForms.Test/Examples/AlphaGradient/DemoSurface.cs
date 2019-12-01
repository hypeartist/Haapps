using System;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Test.Examples.AlphaGradient
{
	using TTPixfmt = PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>;
	using TTRendererBase = RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>;
	using TTRasterizerScanline = RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>;
	using TTRendererScanlineAASolid = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>>;
	using TTSpanGradient = SpanGradient<Color8, Color8, SpanGradientData<Color8>, SpanGradientApplier<Color8, SpanGradientData<Color8>>, TransformAffine, SpanInterpolatorLinear<TransformAffine>, Gradients.Circle>;
	using TTSpanGradientAlpha = SpanGradient<Color8, byte, SpanGradientData<byte>, SpanGradientAlphaApplier<Color8, SpanGradientData<byte>>, TransformAffine, SpanInterpolatorLinear<TransformAffine>, Gradients.XY>;

	public unsafe partial class DemoSurface : Surface
	{
		private readonly PodArray<Point64> _xy = new PodArray<Point64>(6);
		private readonly PodArray<double> _parallelogram = new PodArray<double>(6);
		private double _dx;
		private double _dy;
		private int _index;
		private readonly Ellipse _ellipse = new Ellipse();
		private readonly RasterizerData<Cell> _rasterizerData = new RasterizerData<Cell>();
		private readonly VcgenStroke _stroke = new VcgenStroke();
		private readonly RefPodArrayAdapter<Color8> _colorData = new RefPodArrayAdapter<Color8>(256);
		private readonly RefPodArrayAdapter<byte> _alphaData = new RefPodArrayAdapter<byte>(256);

		public DemoSurface()
		{
			InitializeComponent();

			PixelFormat = Pixmap.Format.BGR24;

			_xy[0].X = 257 + 200;
			_xy[0].Y = 60 + 200;
			_xy[1].X = 369 + 200;
			_xy[1].Y = 170 + 200;
			_xy[2].X = 143 + 200;
			_xy[2].Y = 310 + 200;
		}

		protected override void OnResize(EventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var pixfmt = new TTPixfmt(ref RenderingBuffer);
			var rendererBaseColor = new TTRendererBase(ref pixfmt);

			rendererBaseColor.Clear(new Color8(255, 255, 255));

			var _gradientMatrix = new TransformAffine();
			var _alphaMatrix = new TransformAffine();

			var gradientInterpolator = new SpanInterpolatorLinear<TransformAffine>(ref _gradientMatrix);
			var alphaInterpolator = new SpanInterpolatorLinear<TransformAffine>(ref _alphaMatrix);

			// var tmp1 = stackalloc Color8[256];
			var colorFunction = new SpanGradientData<Color8>(_colorData);//new PodSpan<Color8>(tmp1, 256);
			var gradient = new TTSpanGradient(ref gradientInterpolator, ref colorFunction, 0, 150);
			// var tmp2 = stackalloc byte[256];
			var alphaFunction = new SpanGradientData<byte>(_alphaData);//new PodSpan<byte>(tmp2, 256);
			var gradientAlpha = new TTSpanGradientAlpha(ref alphaInterpolator, ref alphaFunction, 0, 100);

			var spanConverter = new SpanConverter<Color8, TTSpanGradient, TTSpanGradientAlpha>(ref gradient, ref gradientAlpha);

			var rasterizer = new TTRasterizerScanline(_rasterizerData);
			
			for (var i = 0; i < 100; i++)
			{
				_ellipse.Init(Common.Rand(Width), Common.Rand(Height), Common.Rand(6, 60), Common.Rand(6, 60), 50);
				rasterizer.AddPath(_ellipse);
				TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref rasterizer, ref rendererBaseColor, Color8.Random(2.5));
			}

			_parallelogram[0] = _xy[0].X;
			_parallelogram[1] = _xy[0].Y;
			_parallelogram[2] = _xy[1].X;
			_parallelogram[3] = _xy[1].Y;
			_parallelogram[4] = _xy[2].X;
			_parallelogram[5] = _xy[2].Y;

			_gradientMatrix.Reset();
			_gradientMatrix *= TransformAffine.AffineScaling(0.75, 1.2);
			_gradientMatrix *= TransformAffine.AffineRotation(-Common.Pi / 3.0);
			_gradientMatrix *= TransformAffine.AffineTranslation((double)Width / 2, (double)Height / 2);
			_gradientMatrix.Invert();

			_alphaMatrix.ParlToRect(_parallelogram, -100, -100, 100, 100);
			FillAlphaArray(ref colorFunction, Color8.FromDoubles(0, 0.19, 0.19), Color8.FromDoubles(0.7, 0.7, 0.19), Color8.FromDoubles(0.31, 0, 0));

			for (var i = 0; i < 256; i++)
			{
				alphaFunction[i] = (byte)((/*_splAlpha.Value*/(i / 255.0)) * Color8.BaseMask);
			}

			_ellipse.Init((double)Width / 2, (double)Height / 2, 150, 150, 100);
			rasterizer.AddPath(_ellipse);

			Renderer.RenderScanlines<Color8, TTRasterizerScanline, ScanlineU, TTRendererBase, SpanConverter<Color8, TTSpanGradient, TTSpanGradientAlpha>>(ref rasterizer, ref rendererBaseColor, ref spanConverter);

			var color = Color8.FromDoubles(0, 0.4, 0.4, 0.31);

			_ellipse.Init(_xy[0].X, _xy[0].Y, 5, 5, 20);
			rasterizer.AddPath(_ellipse);
			TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref rasterizer, ref rendererBaseColor, color);

			_ellipse.Init(_xy[1].X, _xy[1].Y, 5, 5, 20);
			rasterizer.AddPath(_ellipse);
			TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref rasterizer, ref rendererBaseColor, color);

			_ellipse.Init(_xy[2].X, _xy[2].Y, 5, 5, 20);
			rasterizer.AddPath(_ellipse);
			TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref rasterizer, ref rendererBaseColor, color);

			_stroke.RemoveAll();
			_stroke.AddVertex(_xy[0].X, _xy[0].Y, PathCommand.MoveTo);
			_stroke.AddVertex(_xy[1].X, _xy[1].Y, PathCommand.LineTo);
			_stroke.AddVertex(_xy[2].X, _xy[2].Y, PathCommand.LineTo);
			_stroke.AddVertex(_xy[0].X + _xy[2].X - _xy[1].X, _xy[0].Y + _xy[2].Y - _xy[1].Y, PathCommand.LineTo);
			_stroke.AddVertex(0, 0, PathCommand.EndPoly | (PathCommand)PathFlags.Close);
			rasterizer.AddPath(_stroke);
			TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref rasterizer, ref rendererBaseColor, new Color8(0, 0, 0));
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left) return;
			var x = e.X;
			var y = e.Y;
			int i;
			for (i = 0; i < 3; i++)
			{
				if (!(Math.Sqrt((x - _xy[i].X) * (x - _xy[i].X) + (y - _xy[i].Y) * (y - _xy[i].Y)) < 10.0)) continue;
				_dx = x - _xy[i].X;
				_dy = y - _xy[i].Y;
				_index = i;
				break;
			}
			if (i != 3) return;
			if (!Common.PointInTriangle(_xy[0].X, _xy[0].Y, _xy[1].X, _xy[1].Y, _xy[2].X, _xy[2].Y, x, y)) return;
			_dx = x - _xy[0].X;
			_dy = y - _xy[0].Y;
			_index = 3;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var x = e.X;
			var y = e.Y;
			if (e.Button == MouseButtons.Left)
			{
				if (_index == 3)
				{
					var dx = x - _dx;
					var dy = y - _dy;
					_xy[1].X -= _xy[0].X - dx;
					_xy[1].Y -= _xy[0].Y - dy;
					_xy[2].X -= _xy[0].X - dx;
					_xy[2].Y -= _xy[0].Y - dy;
					_xy[0].X = dx;
					_xy[0].Y = dy;
					ForceRedraw();
					return;
				}

				if (_index < 0) return;
				_xy[_index].X = x - _dx;
				_xy[_index].Y = y - _dy;
				ForceRedraw();
			}
			else
			{
				OnMouseUp(e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_index = -1;
		}

		private static void FillAlphaArray(ref SpanGradientData<Color8> array, Color8 begin, Color8 middle, Color8 end)
		{
			int i;
			for (i = 0; i < 128; ++i)
			{
				array[i] = begin.Gradient(middle, i / 128.0);
			}
			for (; i < 256; ++i)
			{
				array[i] = middle.Gradient(end, (i - 128) / 128.0);
			}
		}
	}
}
