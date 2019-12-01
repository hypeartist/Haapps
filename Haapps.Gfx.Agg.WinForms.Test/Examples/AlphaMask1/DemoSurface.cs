using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Test.Examples.AlphaMask1
{
	using TTPixfmt = PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>;
	using TTPixfmtGray = PixfmtAlphaBlendGray8<RenderingBuffer, Bgr, MaskParams.Default<Bgr>, BlenderGray8>;
	using TTRendererBase = RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>;
	using TTRasterizerScanline = RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>;
	using TTRendererScanlineAASolid = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaBlend24<RenderingBuffer, Bgr, BlenderColor24<Bgr>>>>;
	using TTRendererScanlineAASolidGray = RendererScanlineAASolid<Gray8, RendererBase<Gray8, PixfmtAlphaBlendGray8<RenderingBuffer, Bgr, MaskParams.Default<Bgr>, BlenderGray8>>>;

	public unsafe partial class DemoSurface : Surface
	{
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
		private readonly PathStorage _path = new PathStorage();
		private readonly ConvTransform<PathStorage, TransformAffine> _pathTransform;
		private readonly RasterizerData<Cell> _rasterizerData = new RasterizerData<Cell>();
		private readonly Ellipse _ellipse = new Ellipse();
		private RenderingBuffer _alphaMaskRenderingBuffer;
		private AlphaMaskU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>> _alphaMask;
		private TTRasterizerScanline _rasterizer;
		private readonly PodArray<byte> _alphaMaskBufferData = new PodArray<byte>();

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
			
			var rendererSolidColor = new TTRendererScanlineAASolid(ref rendererBaseColor);

			rendererBaseColor.Clear(new Color8(255, 255, 255));

			var scanlineUM = new ScanlineUM<AlphaMaskU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>>(ref _alphaMask);

			_matrix.Reset();
			_matrix *= TransformAffine.AffineTranslation(-_baseDx, -_baseDy);
			_matrix *= TransformAffine.AffineScaling(_scale, _scale);
			_matrix *= TransformAffine.AffineRotation(_angle + Common.Pi);
			_matrix *= TransformAffine.AffineSkewing(_skewX/1000.0, _skewY/1000.0);
			_matrix *= TransformAffine.AffineTranslation(Width/2.0, Height/2.0);
			
			Renderer.RenderAllPaths<Color8, TTRasterizerScanline, ScanlineUM<AlphaMaskU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>>, TTRendererScanlineAASolid>(ref _rasterizer, ref scanlineUM, ref rendererSolidColor, _pathTransform, _colors, _idxs, _numPaths);
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
			_alphaMask = new AlphaMaskU8<RenderingBuffer, Bgr, OneComponentMaskU8<Bgr>, MaskParams.Default<Bgr>>(ref _alphaMaskRenderingBuffer);
			
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
}
