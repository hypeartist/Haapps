using System;
using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanGouraudColor : ISpanGenerator<Color8>
	{
		private const int SubpixelShift = 4;
		private const int SubpixelScale = 1 << SubpixelShift;

		private RefPodArray<CoordType> _coords;
		private RefPodArray<PathItem> _vertices;
		private DDALineInterpolator _interpolatorA;
		private DDALineInterpolator _interpolatorB;
		private DDALineInterpolator _interpolatorG;
		private DDALineInterpolator _interpolatorR;
		private Calc32 _rgba1;
		private Calc32 _rgba2;
		private Calc32 _rgba3;
		private bool _swap;
		private int _y2;

		public static SpanGouraudColor CreateAndInitialize()
		{
			var @this = new SpanGouraudColor
			{
				_interpolatorA = new DDALineInterpolator(14),
				_interpolatorB = new DDALineInterpolator(14),
				_interpolatorG = new DDALineInterpolator(14),
				_interpolatorR = new DDALineInterpolator(14),
				_coords = new RefPodArray<CoordType>(3),
				_vertices = new RefPodArray<PathItem>(8)
			};
			return @this;
		}

		public void Init(Color8 c1, Color8 c2, Color8 c3, double x1, double y1, double x2, double y2, double x3, double y3, double d)
		{
			_interpolatorA = new DDALineInterpolator(14);
			_interpolatorB = new DDALineInterpolator(14);
			_interpolatorG = new DDALineInterpolator(14);
			_interpolatorR = new DDALineInterpolator(14);
			_coords = new RefPodArray<CoordType>(3);
			_vertices = new RefPodArray<PathItem>(8);
			Triangle(x1, y1, x2, y2, x3, y3, d);
			_coords[0].Color = c1;
			_coords[1].Color = c2;
			_coords[2].Color = c3;
		}

		public FixedSizedVertexSource VertexSourceAdaptor => new FixedSizedVertexSource(_vertices.DataPtr, 8);

		public void Prepare()
		{
			var coord = stackalloc CoordType[3];
			ArrangeVertices(coord);

			_y2 = (int) coord[1].Y;

			_swap = Common.CrossProduct(coord[0].X, coord[0].Y, coord[2].X, coord[2].Y, coord[1].X, coord[1].Y) < 0.0;

			_rgba1.Init(ref coord[0], ref coord[2]);
			_rgba2.Init(ref coord[0], ref coord[1]);
			_rgba3.Init(ref coord[1], ref coord[2]);
		}

		public void Generate(Color8* span, int x, int y, int length)
		{
			_rgba1.Calc(y);
			ref var pc1 = ref _rgba1;
			ref var pc2 = ref _rgba2;

			if (y <= _y2)
			{
				_rgba2.Calc(y + _rgba2.DY);
			}
			else
			{
				_rgba3.Calc(y - _rgba3.DY);
				pc2 = _rgba3;
			}

			if (_swap)
			{
				ref var t = ref pc2;
				pc2 = pc1;
				pc1 = t;
			}

			var nlen = Math.Abs(pc2.X - pc1.X);
			if (nlen <= 0)
			{
				nlen = 1;
			}

			_interpolatorR.Init(pc1.R, pc2.R, nlen);
			_interpolatorG.Init(pc1.G, pc2.G, nlen);
			_interpolatorB.Init(pc1.B, pc2.B, nlen);
			_interpolatorA.Init(pc1.A, pc2.A, nlen);

			var start = pc1.X - (x << SubpixelShift);
			_interpolatorR.Sub(start);
			_interpolatorG.Sub(start);
			_interpolatorB.Sub(start);
			_interpolatorA.Sub(start);
			nlen += start;

			int vr, vg, vb, va;
			// var lim = Rgba8.BaseMask;

			while (length != 0 && start > 0)
			{
				vr = _interpolatorR.Y;
				vg = _interpolatorG.Y;
				vb = _interpolatorB.Y;
				va = _interpolatorA.Y;
				if (vr < 0)
				{
					vr = 0;
				}

				if (vr > Color8.BaseMask)
				{
					vr = Color8.BaseMask;
				}

				if (vg < 0)
				{
					vg = 0;
				}

				if (vg > Color8.BaseMask)
				{
					vg = Color8.BaseMask;
				}

				if (vb < 0)
				{
					vb = 0;
				}

				if (vb > Color8.BaseMask)
				{
					vb = Color8.BaseMask;
				}

				if (va < 0)
				{
					va = 0;
				}

				if (va > Color8.BaseMask)
				{
					va = Color8.BaseMask;
				}

				span->R = (byte) vr;
				span->G = (byte) vg;
				span->B = (byte) vb;
				span->A = (byte) va;

				_interpolatorR.Add(SubpixelScale);
				_interpolatorG.Add(SubpixelScale);
				_interpolatorB.Add(SubpixelScale);
				_interpolatorA.Add(SubpixelScale);

				nlen -= SubpixelScale;
				start -= SubpixelScale;
				++span;
				--length;
			}

			while (length != 0 && nlen > 0)
			{
				span->R = (byte) _interpolatorR.Y;
				span->G = (byte) _interpolatorG.Y;
				span->B = (byte) _interpolatorB.Y;
				span->A = (byte) _interpolatorA.Y;

				_interpolatorR.Add(SubpixelScale);
				_interpolatorG.Add(SubpixelScale);
				_interpolatorB.Add(SubpixelScale);
				_interpolatorA.Add(SubpixelScale);

				nlen -= SubpixelScale;
				++span;
				--length;
			}

			while (length != 0)
			{
				vr = _interpolatorR.Y;
				vg = _interpolatorG.Y;
				vb = _interpolatorB.Y;
				va = _interpolatorA.Y;
				if (vr < 0)
				{
					vr = 0;
				}

				if (vr > Color8.BaseMask)
				{
					vr = Color8.BaseMask;
				}

				if (vg < 0)
				{
					vg = 0;
				}

				if (vg > Color8.BaseMask)
				{
					vg = Color8.BaseMask;
				}

				if (vb < 0)
				{
					vb = 0;
				}

				if (vb > Color8.BaseMask)
				{
					vb = Color8.BaseMask;
				}

				if (va < 0)
				{
					va = 0;
				}

				if (va > Color8.BaseMask)
				{
					va = Color8.BaseMask;
				}

				span->R = (byte) vr;
				span->G = (byte) vg;
				span->B = (byte) vb;
				span->A = (byte) va;

				_interpolatorR.Add(SubpixelScale);
				_interpolatorG.Add(SubpixelScale);
				_interpolatorB.Add(SubpixelScale);
				_interpolatorA.Add(SubpixelScale);

				++span;
				--length;
			}
		}

		
		public void Colors(Color8 c1, Color8 c2, Color8 c3)
		{
			_coords[0].Color = c1;
			_coords[1].Color = c2;
			_coords[2].Color = c3;
		}

		
		private void ArrangeVertices(CoordType* coord)
		{
			coord[0] = _coords[0];
			coord[1] = _coords[1];
			coord[2] = _coords[2];

			if (_coords[0].Y > _coords[2].Y)
			{
				coord[0] = _coords[2];
				coord[2] = _coords[0];
			}

			CoordType tmp;
			if (coord[0].Y > coord[1].Y)
			{
				tmp = coord[1];
				coord[1] = coord[0];
				coord[0] = tmp;
			}

			if (!(coord[1].Y > coord[2].Y))
			{
				return;
			}

			tmp = coord[2];
			coord[2] = coord[1];
			coord[1] = tmp;
		}

		public void Triangle(double x1, double y1, double x2, double y2, double x3, double y3, double d)
		{
			_coords[0].X = _vertices[0].X = x1;
			_coords[0].Y = _vertices[0].Y = y1;
			_coords[1].X = _vertices[1].X = x2;
			_coords[1].Y = _vertices[1].Y = y2;
			_coords[2].X = _vertices[2].X = x3;
			_coords[2].Y = _vertices[2].Y = y3;

			_vertices[0].Command = PathCommand.MoveTo;
			_vertices[1].Command = PathCommand.LineTo;
			_vertices[2].Command = PathCommand.LineTo;
			_vertices[3].Command = PathCommand.Stop;

			if (!(Math.Abs(d) > double.Epsilon))
			{
				return;
			}

			Common.DilateTriangle(_coords[0].X, _coords[0].Y, _coords[1].X, _coords[1].Y, _coords[2].X, _coords[2].Y, _vertices.DataPtr, d);

			Common.CalcIntersection(_vertices[4].X, _vertices[4].Y, _vertices[5].X, _vertices[5].Y, _vertices[0].X, _vertices[0].Y, _vertices[1].X, _vertices[1].Y, ref _coords[0].X, ref _coords[0].Y);
			Common.CalcIntersection(_vertices[0].X, _vertices[0].Y, _vertices[1].X, _vertices[1].Y, _vertices[2].X, _vertices[2].Y, _vertices[3].X, _vertices[3].Y, ref _coords[1].X, ref _coords[1].Y);
			Common.CalcIntersection(_vertices[2].X, _vertices[2].Y, _vertices[3].X, _vertices[3].Y, _vertices[4].X, _vertices[4].Y, _vertices[5].X, _vertices[5].Y, ref _coords[2].X, ref _coords[2].Y);

			_vertices[3].Command = PathCommand.LineTo;
			_vertices[4].Command = PathCommand.LineTo;
			_vertices[5].Command = PathCommand.LineTo;
			_vertices[6].Command = PathCommand.Stop;
		}

		private struct CoordType
		{
			public double X;
			public double Y;
			public Color8 Color;
		}

		private struct Calc32
		{
			private double _x1;
			private double _y1;
			private double _dx;
			public double DY;
			private int _r1;
			private int _g1;
			private int _b1;
			private int _a1;
			private int _dr;
			private int _dg;
			private int _db;
			private int _da;
			public int R;
			public int G;
			public int B;
			public int A;
			public int X;

			
			public void Init(ref CoordType c1, ref CoordType c2)
			{
				_x1 = c1.X - 0.5;
				_y1 = c1.Y - 0.5;
				_dx = c2.X - c1.X;
				var dy = c2.Y - c1.Y;
				DY = dy < 1e-5 ? 1e5 : 1.0 / dy;
				_r1 = c1.Color.R;
				_g1 = c1.Color.G;
				_b1 = c1.Color.B;
				_a1 = c1.Color.A;
				_dr = c2.Color.R - _r1;
				_dg = c2.Color.G - _g1;
				_db = c2.Color.B - _b1;
				_da = c2.Color.A - _a1;
			}

			
			public void Calc(double y)
			{
				var k = (y - _y1) * DY;
				if (k < 0.0)
				{
					k = 0.0;
				}

				if (k > 1.0)
				{
					k = 1.0;
				}

				R = _r1 + (int) (_dr * k);
				G = _g1 + (int) (_dg * k);
				B = _b1 + (int) (_db * k);
				A = _a1 + (int) (_da * k);
				X = (int) ((_x1 + _dx * k) * SubpixelScale);
			}
		}
	}

	public sealed unsafe class FixedSizedVertexSource : VertexSourceAbstract
	{
		private readonly PathItem* _data;
		private readonly int _size;
		private int _iterator;

		public FixedSizedVertexSource(PathItem* data, int size)
		{
			_data = data;
			_size = size;
		}

		public override void Rewind(int pathId = 0) => _iterator = 0;

		public override PathCommand Vertex(ref double x, ref double y) => _data[_iterator++].Decompose(out x, out y);

		public override void Dispose()
		{
		}
	}
}