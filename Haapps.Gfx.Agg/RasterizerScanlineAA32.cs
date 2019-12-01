using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RasterizerScanlineAA32<TRasterizerConverter, TRasterizerClipper> : IRasterizerScanline
		where TRasterizerConverter : unmanaged, IRasterizerConverter32
		where TRasterizerClipper : unmanaged, IRasterizerClipper32<TRasterizerConverter>
	{
		private enum Status
		{
			Initial,
			MoveTo,
			LineTo,
			Closed
		}

		private static TRasterizerClipper Clipper = default;
		private static TRasterizerConverter Converter = default;

		private FixedSize256 _gamma;
		private int _scanY;
		
		private int _startX;
		private int _startY;

		private RasterizerCellsAA<Cell> _outline;
		private Status _status;

		public RasterizerScanlineAA32(RasterizerData<Cell> data) : this()
		{
			_outline = new RasterizerCellsAA<Cell>(ref data.Cells, ref data.SortedY, ref data.SortedCells);
			AutoClose = true;
			var gp = (byte*) (Unsafe.AsPointer(ref _gamma));
			for (var i = 0; i < Common.AAScale; i++)
			{
				gp[i] = (byte) i;
			}
		}

		public void ApplyGammaFunction<TGammaFunction>(TGammaFunction value)
			where TGammaFunction : unmanaged, IGammaFunction
		{
			var gp = (byte*) (Unsafe.AsPointer(ref _gamma));
			for (var i = 0; i < Common.AAScale; i++)
			{
				var v = value.Execute((double) i / Common.AAMask) * Common.AAMask;
				gp[i] = (byte) Common.RoundToU32(v);
			}
		}

		public int MinX => _outline.MinX;

		public int MinY => _outline.MinY;

		public int MaxX => _outline.MaxX;

		public int MaxY => _outline.MaxY;

		public FillingRule FillingRule { get; set; }

		public bool AutoClose { get; set; }

		public byte ApplyGamma(byte cover)
		{
			return ((byte*) (Unsafe.AsPointer(ref _gamma)))[cover];
		}

		
		private byte CalculateAlpha(int area)
		{
			var cover = area >> (Common.PolySubpixelShiftMul2 + 1 - Common.AAShift);

			if (cover < 0)
			{
				cover = -cover;
			}

			if (FillingRule == FillingRule.FillEvenOdd)
			{
				cover &= Common.AAMask2;
				if (cover > Common.AAScale)
				{
					cover = Common.AAScale2 - cover;
				}
			}

			if (cover > Common.AAMask)
			{
				cover = Common.AAMask;
			}

			return ((byte*) (Unsafe.AsPointer(ref _gamma)))[cover];
		}

		public void Reset()
		{
			_outline.Reset();
			_status = Status.Initial;
		}

		public bool SweepScanline<TScanline>(ref TScanline sl, int misc = 0)
			where TScanline : unmanaged, IScanline
		{
			for (;;)
			{
				if (_scanY > _outline.MaxY)
				{
					return false;
				}

				sl.ResetSpans();

				var cells = _outline.ScanlineCells(_scanY, out var count);
				var cover = 0;

				while (count != 0)
				{
					var curCell = *cells;
					var x = curCell->X;
					var area = curCell->Area;
					byte alpha;

					cover += curCell->Cover;

					while (--count != 0)
					{
						curCell = *++cells;
						if (curCell->X != x) break;
						area += curCell->Area;
						cover += curCell->Cover;
					}

					if (area != 0)
					{
						alpha = CalculateAlpha((cover << (Common.PolySubpixelShift + 1)) - area);
						if (alpha != 0)
						{
							sl.AddCell(x, alpha);
						}
						x++;
					}

					if ((count == 0 || curCell->X <= x)) continue;
					alpha = CalculateAlpha(cover << (Common.PolySubpixelShift + 1));
					if (alpha != 0)
					{
						sl.AddSpan(x, curCell->X - x, alpha);
					}
				}

				if (!sl.IsEmpty)
				{
					break;
				}

				++_scanY;
			}

			sl.Finalize(_scanY);
			++_scanY;
			return true;
		}

		public bool RewindScanlines()
		{
			if (AutoClose)
			{
				ClosePolygon();
			}

			_outline.SortCells();
			if (_outline.IsEmpty)
			{
				return false;
			}

			_scanY = _outline.MinY;
			return true;
		}

		public void ClipBox(double x1, double y1, double x2, double y2)
		{
			Reset();
			Clipper.ClipBox(Converter.Upscale(x1), Converter.Upscale(y1), Converter.Upscale(x2), Converter.Upscale(y2));
		}

		public void ClosePolygon()
		{
			if (_status != Status.LineTo)
			{
				return;
			}

			Clipper.LineTo(ref _outline, _startX, _startY);
			_status = Status.Closed;
		}

		
		public void MoveTo(int x, int y)
		{
			if (_outline.IsSorted)
			{
				Reset();
			}

			if (AutoClose)
			{
				ClosePolygon();
			}

			Clipper.MoveTo(_startX = Converter.Downscale(x), _startY = Converter.Downscale(y));
			_status = Status.MoveTo;
		}

		
		public void LineTo(int x, int y)
		{
			Clipper.LineTo(ref _outline, Converter.Downscale(x), Converter.Downscale(y));
			_status = Status.LineTo;
		}

		
		public void MoveToD(double x, double y)
		{
			if (_outline.IsSorted)
			{
				Reset();
			}

			if (AutoClose)
			{
				ClosePolygon();
			}

			Clipper.MoveTo(_startX = Converter.Upscale(x), _startY = Converter.Upscale(y));
			_status = Status.MoveTo;
		}

		
		public void LineToD(double x, double y)
		{
			Clipper.LineTo(ref _outline, Converter.Upscale(x), Converter.Upscale(y));
			_status = Status.LineTo;
		}

		public void AddVertex(double x, double y, PathCommand cmd)
		{
			if (cmd.MoveTo())
			{
				MoveToD(x, y);
			}
			else if (cmd.Vertex())
			{
				LineToD(x, y);
			}
			else if (cmd.Closed())
			{
				ClosePolygon();
			}
		}

		public void AddPath(VertexSourceAbstract vs, int pathId = 0)
		{
			PathCommand cmd;
			vs.Rewind(pathId);
			if (_outline.IsSorted)
			{
				Reset();
			}

			double x = 0;
			double y = 0;
			while (!(cmd = vs.Vertex(ref x, ref y)).Stop())
			{
				AddVertex(x, y, cmd);
			}
		}
	}
}