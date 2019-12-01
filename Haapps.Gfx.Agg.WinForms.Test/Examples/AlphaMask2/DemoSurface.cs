using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Test.Examples.AlphaMask2
{
	using TTPixfmt = PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>;
	using TTPixfmtGray = PixfmtAlphaBlendGray8<RenderingBuffer, Bgr, MaskParams.Default<Bgr>, BlenderGray8>;
	using TTPixfmtAdapter = PixfmtAlphaMaskAdaptor<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>, AlphaMaskNoClipU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>>;
	using TTRendererBase = RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>;
	using TTRendererBaseAdapter = RendererBase<Color8, PixfmtAlphaMaskAdaptor<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>, AlphaMaskNoClipU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>>>;
	using TTRasterizerScanline = RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>;
	using TTRendererScanlineAASolid = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>>;
	using TTRendererScanlineAASolidGray = RendererScanlineAASolid<Gray8, RendererBase<Gray8, PixfmtAlphaBlendGray8<RenderingBuffer, Bgr, MaskParams.Default<Bgr>, BlenderGray8>>>;
	using TTRendererScanlineAASolidAdapter = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaMaskAdaptor<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>, AlphaMaskNoClipU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>>>>;
	using TTSpanGradient = SpanGradient<Color8, Color8, GradientLinearColor, SpanGradientApplier<Color8, GradientLinearColor>, TransformAffine, SpanInterpolatorLinear<TransformAffine>, Gradients.Circle>;

	public unsafe partial class DemoSurface : Surface
	{
		private readonly ExecutionTimer _timer = new ExecutionTimer();
		private double _angle;
		private double _scale = 1;
		private double _skewX;
		private double _skewY;
		private readonly int _numPaths;
		private readonly PodArray<Color8> _colors = new PodArray<Color8>(256);
		private readonly PodArray<int> _idxs = new PodArray<int>(100);
		private readonly double _baseDx;
		private readonly double _baseDy;
		private TransformAffine _matrix;
		private TransformAffine _gradientMatrix;
		private readonly PathStorage _path = new PathStorage();
		private readonly ConvTransform<PathStorage, TransformAffine> _pathTransform;
		private readonly RasterizerData<Cell> _rasterizerData = new RasterizerData<Cell>();
		private readonly RasterizerOutlineData _rasterizerOutlineData = new RasterizerOutlineData();
		private readonly LineProfileAAData _lineProfileAaData = new LineProfileAAData();
		private readonly Ellipse _ellipse = new Ellipse();
		private RenderingBuffer _alphaMaskRenderingBuffer;
		private AlphaMaskNoClipU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>> _alphaMask;
		private TTRasterizerScanline _rasterizer;
		private readonly PodArray<byte> _alphaMaskBufferData = new PodArray<byte>();
		// private readonly StackArrayAdapter<Color8> _colorData = new StackArrayAdapter<Color8>(256);

		public DemoSurface()
		{
			InitializeComponent();

			PixelFormat = Pixmap.Format.BGR24;

			_rasterizer = new TTRasterizerScanline(_rasterizerData);

			_numPaths = DemoData.Lion.Make(_path, _colors, _idxs);
			_pathTransform = new ConvTransform<PathStorage, TransformAffine>(_path, ref _matrix);

			_path.Bounds(_idxs, 0, _numPaths, out double x1, out var y1, out var x2, out var y2);
			_baseDx = (x2 - x1)/2.0;
			_baseDy = (y2 - y1)/2.0;
		}

		protected override void OnResize(EventArgs e)
		{
			GenerateAlphaMask(Width, Height);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var pixfmt = new TTPixfmt(ref RenderingBuffer);
			var rendererBaseColor = new TTRendererBase(ref pixfmt);
			
			rendererBaseColor.Clear(new Color8(255, 255, 255));

			_matrix.Reset();
			_matrix *= TransformAffine.AffineTranslation(-_baseDx, -_baseDy);
			_matrix *= TransformAffine.AffineScaling(_scale, _scale);
			_matrix *= TransformAffine.AffineRotation(_angle + Common.Pi);
			_matrix *= TransformAffine.AffineSkewing(_skewX/1000.0, _skewY/1000.0);
			_matrix *= TransformAffine.AffineTranslation(Width/2.0, Height/2.0);
			
			var pixfmtAlphaMaskAdaptor = new TTPixfmtAdapter(ref pixfmt, ref _alphaMask);
			var rendererBaseAdaptor = new TTRendererBaseAdapter(ref pixfmtAlphaMaskAdaptor);
			var rendererSolidAdaptor = new TTRendererScanlineAASolidAdapter(ref rendererBaseAdaptor) { Color = Color8.FromDoubles(0.5, 0.0, 0, 0.5) };

			Renderer.RenderAllPaths<Color8, TTRasterizerScanline, ScanlineU, TTRendererScanlineAASolidAdapter>(ref _rasterizer, ref rendererSolidAdaptor, _pathTransform, _colors, _idxs, _numPaths);

			var rendererMarkersColor = new RendererMarkers<Color8, TTRendererBaseAdapter>(ref rendererBaseAdaptor);
			
			var lineProfile = new LineProfileAA(5.0, _lineProfileAaData);
			var rendererOutline = new RendererOutlineAA<Color8, TTRendererBaseAdapter>(ref rendererBaseAdaptor, ref lineProfile);
			var rasterizerOutline = new RasterizerOutlineAA<Color8, TTRendererBaseAdapter>(ref rendererOutline, _rasterizerOutlineData) { RoundCap = true };
			
			var spanInterpolator = new SpanInterpolatorLinear<TransformAffine>(ref _gradientMatrix);
			// var tmp1 = stackalloc Color8[256];
			var colorFunction = new GradientLinearColor();//new PodSpan<Color8>(tmp1, 256);
			var gradient = new TTSpanGradient(ref spanInterpolator, ref colorFunction, 0, 10);
			
			var rendererGradientColor = new RendererScanlineAAGradient<Color8, TTRendererBaseAdapter, TTSpanGradient>(ref rendererBaseAdaptor, ref gradient);
			
			for (var i = 0; i < 50; i++)
			{
				rendererMarkersColor.LineColor = new Color8((byte)Common.Rand(127), (byte)Common.Rand(127), (byte)Common.Rand(127), (byte)(Common.Rand(127) + 127));
				rendererMarkersColor.FillColor = new Color8((byte)Common.Rand(127), (byte)Common.Rand(127), (byte)Common.Rand(127), (byte)(Common.Rand(127) + 127));
			
				rendererMarkersColor.Line(Coord(Common.Rand(Width)), Coord(Common.Rand(Height)), Coord(Common.Rand(Width)), Coord(Common.Rand(Height)));
				rendererMarkersColor.Marker((int)Common.Rand(Width), (int)Common.Rand(Height), (int)Common.Rand(5, 10), (Markers)Common.Rand(17));
			}
			
			for (var i = 0; i < 50; i++)
			{
				rendererOutline.Color = new Color8((byte)Common.Rand(127), (byte)Common.Rand(127), (byte)Common.Rand(127), (byte)(Common.Rand(127) + 127));
				rasterizerOutline.MoveToD(Common.Rand(Width), Common.Rand(Height));
				rasterizerOutline.LineToD(Common.Rand(Width), Common.Rand(Height));
			
				rasterizerOutline.Render(false);
			}
			
			for (var i = 0; i < 1; i++)
			{
				var x = Common.Rand(Width);
				var y = Common.Rand(Height);
				var rr = Common.Rand(10) + 5;
				_gradientMatrix.Reset();
				_gradientMatrix *= TransformAffine.AffineScaling(rr / 10.0);
				_gradientMatrix *= TransformAffine.AffineTranslation(x, y);
				_gradientMatrix.Invert();
				// GenerateColors(ref colorFunction, new Color8(255, 255, 255, 0), new Color8(127,127,127)/*new Color8((byte)Common.Rand(127), (byte)Common.Rand(127), (byte)Common.Rand(127))*/);
				colorFunction.Colors(new Color8(255, 255, 255, 0), new Color8((byte)Common.Rand(127), (byte)Common.Rand(127), (byte)Common.Rand(127)));
				_ellipse.Init(x, y, rr, rr, 32);
				_rasterizer.AddPath(_ellipse);
				Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, RendererScanlineAAGradient<Color8, TTRendererBaseAdapter, TTSpanGradient>>(ref _rasterizer, ref rendererGradientColor);
			}
			
			_timer.Stop();
			var t = _timer.Milliseconds;
			
			using var text = new GSVText();
			text.Size(7.5);
			text.StartPoint(5, Height - 15.0);
			text.SetContent($"Render time: {t:0.00} ms");
			using var textStroke = new ConvStroke<NullMarkers,GSVText>(text) { Width = 1 };
			_rasterizer.AddPath(textStroke);
			TTRendererScanlineAASolid.RenderScanlines<TTRasterizerScanline, ScanlineU>(ref _rasterizer, ref rendererBaseColor, new Color8(0, 0, 0));
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			var x = e.X;
			var y = e.Y;

			if (e.Button == MouseButtons.Left)
			{
				var width = Width;
				var height = Height;
				Transform(width, height, x, y);
				ForceRedraw();
			}

			if (e.Button != MouseButtons.Right) return;
			_skewX = x;
			_skewY = y;
			ForceRedraw();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			OnMouseDown(e);
		}

		private static void GenerateColors(ref PodSpan<Color8> colors, Color8 _c1, Color8 _c2)
		{
			for (var i = 0; i < (uint)colors.Size; i++)
			{
				colors[i] = new Color8((byte)((((_c2.R - _c1.R) * i) + (_c1.R << 8)) >> 8), (byte)((((_c2.G - _c1.G) * i) + (_c1.G << 8)) >> 8), (byte)((((_c2.B - _c1.B) * i) + (_c1.B << 8)) >> 8), (byte)((((_c2.A - _c1.A) * i) + (_c1.A << 8)) >> 8));
			}
		}

		private static int Coord(double c) => Common.RoundToI32(c * BresenhamLineInterpolator.SubpixelScale);

		private void Transform(double width, double height, double x, double y)
		{
			x -= width / 2;
			y -= height / 2;
			_angle = Math.Atan2(y, x);
			_scale = Math.Sqrt(y * y + x * x) / 100.0;
		}

		private void GenerateAlphaMask(int cx, int cy)
		{
			_alphaMaskBufferData.Reallocate(cx * cy);
			_alphaMaskRenderingBuffer = new RenderingBuffer(_alphaMaskBufferData, cx, cy, cx);
			_alphaMask = new AlphaMaskNoClipU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>(ref _alphaMaskRenderingBuffer);
			
			var pixfmtGray = new TTPixfmtGray(ref _alphaMaskRenderingBuffer);
			var rendererBaseGray = new RendererBase<Gray8, TTPixfmtGray>(ref pixfmtGray);
			var rendererSolidGray = new TTRendererScanlineAASolidGray(ref rendererBaseGray);
			
			rendererBaseGray.Clear(new Gray8(0));
			
			for (var i = 0; i < 25; i++)
			{
				_ellipse.Init(Common.Rand(cx), Common.Rand(cy), Common.Rand(20, (double)cx / 5), Common.Rand(20, (double)cy / 5), 100);
				_rasterizer.AddPath(_ellipse);
				rendererSolidGray.Color = new Gray8((byte)(Common.Rand(255)), (byte)(Common.Rand(255)));
				Renderer.RenderScanlines<TTRasterizerScanline, ScanlineP, TTRendererScanlineAASolidGray>(ref _rasterizer, ref rendererSolidGray);
			}
		}
	}

	public struct GradientLinearColor : ISpanGradientDataProvider<Color8>
	{
		private Color8 _c1;
		private Color8 _c2;

		public GradientLinearColor(Color8 c1, Color8 c2)
		{
			_c1 = c1;
			_c2 = c2;
		}

		public void Colors(Color8 c1, Color8 c2)
		{
			_c1 = c1;
			_c2 = c2;
		}

		public int Size => 256;
		
		public Color8 this[int v]
		{
			get
			{
				var c = new Color8
				{
					R = (byte)((((_c2.R - _c1.R) * v) + (_c1.R << 8)) >> 8),
					G = (byte)((((_c2.G - _c1.G) * v) + (_c1.G << 8)) >> 8),
					B = (byte)((((_c2.B - _c1.B) * v) + (_c1.B << 8)) >> 8),
					A = (byte)((((_c2.A - _c1.A) * v) + (_c1.A << 8)) >> 8)
				};
				return c;
			}
			set{}
		}
	}
}
