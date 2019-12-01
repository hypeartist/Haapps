using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Test.Examples.AADemo
{
	using TTPixfmt = PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>;
	using TTRendererBaseColor = RendererBase<Color8, PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>>;
	using TTRasterizerScanline = RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>;
	using TTRendererScanlineAASolidColor = RendererScanlineAASolid<Color8, RendererBase<Color8, PixfmtAlphaBlend32<RenderingBuffer, Bgra, BlenderColor32<Bgra>>>>;

	public partial class DemoSurface : Surface
	{
		private readonly PodArray<Point64> _xy = new PodArray<Point64>(6);
		private double _dx;
		private double _dy;
		private int _idx;

		private readonly PathStorage _path = new PathStorage();
		private readonly ConvStroke<NullMarkers, PathStorage> _pathStroke;
		private readonly RasterizerData<Cell> _rasterizerData = new RasterizerData<Cell>();
		private readonly RasterizerData<Cell> _rasterizerData2 = new RasterizerData<Cell>();

		public DemoSurface()
		{
			InitializeComponent();

			PixelFormat = Pixmap.Format.BGRA32;

			_pathStroke = new ConvStroke<NullMarkers, PathStorage>(_path) { Width = 1.0 };			

			_xy[0].X = 57 + 200;
			_xy[0].Y = 100 + 300;
			_xy[1].X = 369 + 200;
			_xy[1].Y = 170 + 200;
			_xy[2].X = 143 + 200;
			_xy[2].Y = 310 + 300;
		}

		protected override void OnResize(EventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var pixfmt = new TTPixfmt(ref RenderingBuffer);
			var rendererBaseColor = new TTRendererBaseColor(ref pixfmt);

			rendererBaseColor.Clear(new Color8(255, 255, 255));

			var pixelSize = 32;//(int)_sldPixelSize.Value;5

			var rendererEnlarged = new RendererEnlarged<TTRendererBaseColor>(_rasterizerData2, ref rendererBaseColor, pixelSize);

			var rasterizer = new TTRasterizerScanline(_rasterizerData);
			rasterizer.ApplyGammaFunction(new GammaFunctions.Power(1.2));

			rasterizer.Reset();
			rasterizer.MoveToD(_xy[0].X / pixelSize, _xy[0].Y / pixelSize);
			rasterizer.LineToD(_xy[1].X / pixelSize, _xy[1].Y / pixelSize);
			rasterizer.LineToD(_xy[2].X / pixelSize, _xy[2].Y / pixelSize);

			rendererEnlarged.Color = new Color8(0, 0, 0);
			Renderer.RenderScanlines<TTRasterizerScanline, ScanlineU, RendererEnlarged<TTRendererBaseColor>>(ref rasterizer, ref rendererEnlarged);

			TTRendererScanlineAASolidColor.RenderScanlines<TTRasterizerScanline, ScanlineP>(ref rasterizer, ref rendererBaseColor, new Color8(0, 0, 0));
			
			_path.RemoveAll();
			_path.MoveTo(_xy[0].X, _xy[0].Y);
			_path.LineTo(_xy[1].X, _xy[1].Y);
			rasterizer.AddPath(_pathStroke);
			TTRendererScanlineAASolidColor.RenderScanlines<TTRasterizerScanline, ScanlineP>(ref rasterizer, ref rendererBaseColor, new Color8(0, 150, 160, 200));
			
			_path.RemoveAll();
			_path.MoveTo(_xy[1].X, _xy[1].Y);
			_path.LineTo(_xy[2].X, _xy[2].Y);
			rasterizer.AddPath(_pathStroke);
			TTRendererScanlineAASolidColor.RenderScanlines<TTRasterizerScanline, ScanlineP>(ref rasterizer, ref rendererBaseColor, new Color8(0, 150, 160, 200));
			
			_path.RemoveAll();
			_path.MoveTo(_xy[2].X, _xy[2].Y);
			_path.LineTo(_xy[0].X, _xy[0].Y);
			rasterizer.AddPath(_pathStroke);
			TTRendererScanlineAASolidColor.RenderScanlines<TTRasterizerScanline, ScanlineP>(ref rasterizer, ref rendererBaseColor, new Color8(0, 150, 160, 200));
			
			
			//
			// // RenderControl(_scanlineU, _rendererBaseColor, _pnlControls);
			// RenderWidgets(_rendererBaseColor);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var x = e.X;
				var y = e.Y;
				if (_idx == 3)
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

				if (_idx < 0) return;
				_xy[_idx].X = x - _dx;
				_xy[_idx].Y = y - _dy;
				ForceRedraw();
			}
			else
			{
				OnMouseUp(e);
			}
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
				_idx = i;
				break;
			}
			if (i != 3) return;
			if (!Common.PointInTriangle(_xy[0].X, _xy[0].Y, _xy[1].X, _xy[1].Y, _xy[2].X, _xy[2].Y, x, y)) return;
			_dx = x - _xy[0].X;
			_dy = y - _xy[0].Y;
			_idx = 3;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_idx = -1;
		}
	}

	public struct Square
	{
		public double Size;

		public Square(double size)
		{
			Size = size;
		}

		public void Draw<TRasterizerScanline, TScanline, TColor, TRendererBase>(ref TRasterizerScanline rasterizer, ref TRendererBase renderer, TColor color, double x, double y)
			where TRasterizerScanline : unmanaged, IRasterizerScanline
			where TColor : unmanaged, IColor
			where TRendererBase : unmanaged, IRendererBase<TColor>	
			where TScanline  : unmanaged, IScanline
		{
			rasterizer.Reset();
			rasterizer.MoveToD(x * Size, y * Size);
			rasterizer.LineToD(x * Size + Size, y * Size);
			rasterizer.LineToD(x * Size + Size, y * Size + Size);
			rasterizer.LineToD(x * Size, y * Size + Size);
			RendererScanlineAASolid<TColor, TRendererBase>.RenderScanlines<TRasterizerScanline, TScanline>(ref rasterizer, ref renderer, color);
		}
	}

	public unsafe struct RendererEnlarged<TRendererBase> : IRendererScanline<Color8>
		where TRendererBase : unmanaged, IRendererBase<Color8>
	{
		private TTRasterizerScanline _rasterizer;
		private readonly TRendererBase* _renderer;
		private Square _square;

		public RendererEnlarged(RasterizerData<Cell> data, ref TRendererBase ren, double size) : this()
		{
			_renderer = (TRendererBase*) Unsafe.AsPointer(ref ren);
			_square = new Square(size);
			_rasterizer = new TTRasterizerScanline(data);
		}

		public double PixelSize
		{
			set => ((Square*) (Unsafe.AsPointer(ref _square)))->Size = value;
			get => _square.Size;
		}

		public Color8 Color { get; set; }

		public void Prepare()
		{
		}

		public void Render<TScanline>(ref TScanline scanline)
			where TScanline : unmanaged, IScanline
		{
			var y = scanline.Y;
			var spans = scanline.GetSpans(out var count);
			for (var i = 0; i < count; i++)
			{
				var span = spans[i];
				var x = span.X;
				var covers = span.Covers;
				var numPix = span.Length;
				for (var j = 0; j < numPix; j++)
				{
					var c = Color;
					c.A = (byte)((*covers++ * Color.A) >> 8);
					_square.Draw<TTRasterizerScanline, ScanlineU, Color8, TRendererBase>(ref _rasterizer, ref *_renderer, c, x, y);
					x++;
				}
			}
		}
	}
}
