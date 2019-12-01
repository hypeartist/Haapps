using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RasterizerOutlineAA<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		private RendererOutlineAA<TColor, TRendererBase>* _renderer;
		private RefPodList<LineAAVertex>* _srcVertices;
		private OutlineAAJoin _lineJoin;
		private int _startX;
		private int _startY;

		private struct DrawVars
		{
			public int Idx;
			public int X1;
			public int Y1;
			public int X2;
			public int Y2;
			public LineParameters Curr;
			public LineParameters Next;
			public int LCurr;
			public int LNext;
			public int XB1;
			public int YB1;
			public int XB2;
			public int YB2;
			public int Flags;
		}

		public RasterizerOutlineAA(ref RendererOutlineAA<TColor, TRendererBase> renderer, RasterizerOutlineData data) : this()
		{
			_renderer = (RendererOutlineAA<TColor, TRendererBase>*) Unsafe.AsPointer(ref renderer);
			LineJoin = renderer.AccurateJoinOnly ? OutlineAAJoin.MiterAccurateJoin : OutlineAAJoin.RoundJoin;
			RoundCap = false;
			_srcVertices = (RefPodList<LineAAVertex>*) Unsafe.AsPointer(ref data.SrcVertices);
		}

		public OutlineAAJoin LineJoin
		{
			get => _lineJoin;
			set => _lineJoin = _renderer->AccurateJoinOnly ? OutlineAAJoin.MiterAccurateJoin : value;
		}

		public bool RoundCap { get; set; }

		public void Attach(ref RendererOutlineAA<TColor, TRendererBase> renderer) => _renderer = (RendererOutlineAA<TColor, TRendererBase>*)Unsafe.AsPointer(ref renderer);

		private void AddVertex(int x, int y, int length = 0)
		{
			if (_srcVertices->Count > 1)
			{
				ref var vd1 = ref (*_srcVertices)[_srcVertices->Count - 2];
				ref var vd2 = ref (*_srcVertices)[_srcVertices->Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (!ret)
				{
					RemoveLastVertex();
				}
			}

			_srcVertices->Add(new LineAAVertex(x, y));
		}

		private void RemoveLastVertex()
		{
			if (_srcVertices->Count != 0)
			{
				_srcVertices->RemoveLast();
			}
		}

		private void ModifyLastVertex(int x, int y, int length = 0)
		{
			RemoveLastVertex();
			AddVertex(x, y, length);
		}

		private void Close(bool closed)
		{
			while (_srcVertices->Count > 1)
			{
				ref var vd1 = ref (*_srcVertices)[_srcVertices->Count - 2];
				ref var vd2 = ref (*_srcVertices)[_srcVertices->Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (ret) break;
				var t = (*_srcVertices)[_srcVertices->Count - 1];
				RemoveLastVertex();
				ModifyLastVertex(t.X, t.Y, t.Length);
			}

			if (!closed) return;
			while (_srcVertices->Count > 1)
			{
				ref var vd1 = ref (*_srcVertices)[_srcVertices->Count - 1];
				ref var vd2 = ref (*_srcVertices)[0];
				var ret = vd1.Measure(ref vd2);
				if (ret) break;
				RemoveLastVertex();
			}
		}

		private static int Conv(double x) => Common.RoundToI32(x * LineAA.LineSubpixelScale);

		public void MoveTo(int x, int y)
		{
			_startX = x;
			_startY = y;
			ModifyLastVertex(x, y);
		}

		public void LineTo(int x, int y)
		{
			AddVertex(x, y);
		}

		public void MoveToD(double x, double y)
		{
			MoveTo(Conv(x), Conv(y));
		}

		public void LineToD(double x, double y)
		{
			LineTo(Conv(x), Conv(y));
		}

		public void AddVertex(double x, double y, PathCommand cmd)
		{
			if (cmd.MoveTo())
			{
				Render(false);
				MoveToD(x, y);
			}
			else
			{
				if (cmd.EndPoly())
				{
					Render(cmd.Closed());
					if (cmd.Closed())
					{
						MoveTo(_startX, _startY);
					}
				}
				else
				{
					LineToD(x, y);
				}
			}
		}

		public void AddPath(VertexSourceAbstract vs, int pathId = 0)
		{
			double x = 0;
			double y = 0;

			PathCommand cmd;
			vs.Rewind(pathId);
			while (!(cmd = vs.Vertex(ref x, ref y)).Stop())
			{
				AddVertex(x, y, cmd);
			}
			Render(false);
		}

		public void RenderAllPaths(VertexSourceAbstract vs, TColor* colors, int* pathId, int numPaths)
		{
			for (var i = 0; i < numPaths; i++)
			{
				_renderer->Color = colors[i];
				AddPath(vs, pathId[i]);
			}
		}

		private void Draw(ref DrawVars dv, int start, int end)
		{
			int i;

			for (i = start; i < end; i++)
			{
				if (_lineJoin == OutlineAAJoin.RoundJoin)
				{
					dv.XB1 = dv.Curr.X1 + (dv.Curr.Y2 - dv.Curr.Y1);
					dv.YB1 = dv.Curr.Y1 - (dv.Curr.X2 - dv.Curr.X1);
					dv.XB2 = dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1);
					dv.YB2 = dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1);
				}

				switch (dv.Flags)
				{
					case 0:
						_renderer->Line3(ref dv.Curr, dv.XB1, dv.YB1, dv.XB2, dv.YB2);
						break;
					case 1:
						_renderer->Line2(ref dv.Curr, dv.XB2, dv.YB2);
						break;
					case 2:
						_renderer->Line1(ref dv.Curr, dv.XB1, dv.YB1);
						break;
					case 3:
						_renderer->Line0(ref dv.Curr);
						break;
				}

				if (_lineJoin == OutlineAAJoin.RoundJoin && (dv.Flags & 2) == 0)
				{
					_renderer->Pie(dv.Curr.X2, dv.Curr.Y2, dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1), dv.Curr.X2 + (dv.Next.Y2 - dv.Next.Y1), dv.Curr.Y2 - (dv.Next.X2 - dv.Next.X1));
				}

				dv.X1 = dv.X2;
				dv.Y1 = dv.Y2;
				dv.LCurr = dv.LNext;
				dv.LNext = (*_srcVertices)[dv.Idx].Length;

				++dv.Idx;
				if (dv.Idx >= _srcVertices->Count)
				{
					dv.Idx = 0;
				}

				var v = (*_srcVertices)[dv.Idx];
				dv.X2 = v.X;
				dv.Y2 = v.Y;

				dv.Curr = dv.Next;
				dv.Next = new LineParameters(dv.X1, dv.Y1, dv.X2, dv.Y2, dv.LNext);
				dv.XB1 = dv.XB2;
				dv.YB1 = dv.YB2;

				switch (_lineJoin)
				{
					case OutlineAAJoin.NoJoin:
						dv.Flags = 3;
						break;

					case OutlineAAJoin.MiterJoin:
						dv.Flags >>= 1;
						dv.Flags |= (dv.Curr.DiagQuadrant == dv.Next.DiagQuadrant ? 1 : 0) << 1;
						if ((dv.Flags & 2) == 0)
						{
							LineParameters.Bisectrix(ref dv.Curr, ref dv.Next, out dv.XB2, out dv.YB2);
						}
						break;

					case OutlineAAJoin.RoundJoin:
						dv.Flags >>= 1;
						dv.Flags |= (dv.Curr.DiagQuadrant == dv.Next.DiagQuadrant ? 1 : 0) << 1;
						break;

					case OutlineAAJoin.MiterAccurateJoin:
						dv.Flags = 0;
						LineParameters.Bisectrix(ref dv.Curr, ref dv.Next, out dv.XB2, out dv.YB2);
						break;
				}
			}
		}

		public void Render(bool closePolygon)
		{
			Close(closePolygon);
			var dv = new DrawVars();
			LineAAVertex v;
			int x1;
			int y1;
			int x2;
			int y2;
			int lprev;

			if (closePolygon)
			{
				if (_srcVertices->Count >= 3)
				{
					dv.Idx = 2;

					v = (*_srcVertices)[_srcVertices->Count - 1];
					x1 = v.X;
					y1 = v.Y;
					lprev = v.Length;

					v = (*_srcVertices)[0];
					x2 = v.X;
					y2 = v.Y;
					dv.LCurr = v.Length;
					var prev = new LineParameters(x1, y1, x2, y2, lprev);

					v = (*_srcVertices)[1];
					dv.X1 = v.X;
					dv.Y1 = v.Y;
					dv.LNext = v.Length;
					dv.Curr = new LineParameters(x2, y2, dv.X1, dv.Y1, dv.LCurr);

					v = (*_srcVertices)[dv.Idx];
					dv.X2 = v.X;
					dv.Y2 = v.Y;
					dv.Next = new LineParameters(dv.X1, dv.Y1, dv.X2, dv.Y2, dv.LNext);

					dv.XB1 = 0;
					dv.YB1 = 0;
					dv.XB2 = 0;
					dv.YB2 = 0;

					switch (_lineJoin)
					{
						case OutlineAAJoin.NoJoin:
							dv.Flags = 3;
							break;

						case OutlineAAJoin.MiterJoin:
						case OutlineAAJoin.RoundJoin:
							dv.Flags = (prev.DiagQuadrant == dv.Curr.DiagQuadrant ? 1 : 0) | ((dv.Curr.DiagQuadrant == dv.Next.DiagQuadrant ? 1 : 0) << 1);
							break;

						case OutlineAAJoin.MiterAccurateJoin:
							dv.Flags = 0;
							break;
					}

					if ((dv.Flags & 1) == 0 && _lineJoin != OutlineAAJoin.RoundJoin)
					{
						LineParameters.Bisectrix(ref prev, ref dv.Curr, out dv.XB1, out dv.YB1);
					}

					if ((dv.Flags & 2) == 0 && _lineJoin != OutlineAAJoin.RoundJoin)
					{
						LineParameters.Bisectrix(ref dv.Curr, ref dv.Next, out dv.XB2, out dv.YB2);
					}
					Draw(ref dv, 0, _srcVertices->Count);
				}
			}
			else
			{
				switch (_srcVertices->Count)
				{
					case 0:
					case 1:
						break;

					case 2:
					{
						v = (*_srcVertices)[0];
						x1 = v.X;
						y1 = v.Y;
						lprev = v.Length;
						v = (*_srcVertices)[1];
						x2 = v.X;
						y2 = v.Y;
						var lp = new LineParameters(x1, y1, x2, y2, lprev);
						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistStart, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
						}
						_renderer->Line3(ref lp, x1 + (y2 - y1), y1 - (x2 - x1), x2 + (y2 - y1), y2 - (x2 - x1));
						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistEnd, x2, y2, x2 + (y2 - y1), y2 - (x2 - x1));
						}
					}
						break;

					case 3:
					{
						v = (*_srcVertices)[0];
						x1 = v.X;
						y1 = v.Y;
						lprev = v.Length;
						v = (*_srcVertices)[1];
						x2 = v.X;
						y2 = v.Y;
						var lnext = v.Length;
						v = (*_srcVertices)[2];
						var x3 = v.X;
						var y3 = v.Y;
						var lp1 = new LineParameters(x1, y1, x2, y2, lprev);
						var lp2 = new LineParameters(x2, y2, x3, y3, lnext);

						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistStart, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
						}

						if (_lineJoin == OutlineAAJoin.RoundJoin)
						{
							_renderer->Line3(ref lp1, x1 + (y2 - y1), y1 - (x2 - x1), x2 + (y2 - y1), y2 - (x2 - x1));
							_renderer->Pie(x2, y2, x2 + (y2 - y1), y2 - (x2 - x1), x2 + (y3 - y2), y2 - (x3 - x2));
							_renderer->Line3(ref lp2, x2 + (y3 - y2), y2 - (x3 - x2), x3 + (y3 - y2), y3 - (x3 - x2));
						}
						else
						{
							LineParameters.Bisectrix(ref lp1, ref lp2, out dv.XB1, out dv.YB1);
							_renderer->Line3(ref lp1, x1 + (y2 - y1), y1 - (x2 - x1), dv.XB1, dv.YB1);
							_renderer->Line3(ref lp2, dv.XB1, dv.YB1, x3 + (y3 - y2), y3 - (x3 - x2));
						}
						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistEnd, x3, y3, x3 + (y3 - y2), y3 - (x3 - x2));
						}
					}
						break;

					default:
					{
						dv.Idx = 3;

						v = (*_srcVertices)[0];
						x1 = v.X;
						y1 = v.Y;
						lprev = v.Length;

						v = (*_srcVertices)[1];
						x2 = v.X;
						y2 = v.Y;
						dv.LCurr = v.Length;
						var prev = new LineParameters(x1, y1, x2, y2, lprev);

						v = (*_srcVertices)[2];
						dv.X1 = v.X;
						dv.Y1 = v.Y;
						dv.LNext = v.Length;
						dv.Curr = new LineParameters(x2, y2, dv.X1, dv.Y1, dv.LCurr);

						v = (*_srcVertices)[dv.Idx];
						dv.X2 = v.X;
						dv.Y2 = v.Y;
						dv.Next = new LineParameters(dv.X1, dv.Y1, dv.X2, dv.Y2, dv.LNext);

						dv.XB1 = 0;
						dv.YB1 = 0;
						dv.XB2 = 0;
						dv.YB2 = 0;

						switch (_lineJoin)
						{
							case OutlineAAJoin.NoJoin:
								dv.Flags = 3;
								break;

							case OutlineAAJoin.MiterJoin:
							case OutlineAAJoin.RoundJoin:
								dv.Flags = (prev.DiagQuadrant == dv.Curr.DiagQuadrant ? 1 : 0) | ((dv.Curr.DiagQuadrant == dv.Next.DiagQuadrant ? 1 : 0) << 1);
								break;

							case OutlineAAJoin.MiterAccurateJoin:
								dv.Flags = 0;
								break;
						}

						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistStart, x1, y1, x1 + (y2 - y1), y1 - (x2 - x1));
						}
						if ((dv.Flags & 1) == 0)
						{
							if (_lineJoin == OutlineAAJoin.RoundJoin)
							{
								_renderer->Line3(ref prev, x1 + (y2 - y1), y1 - (x2 - x1), x2 + (y2 - y1), y2 - (x2 - x1));
								_renderer->Pie(prev.X2, prev.Y2, x2 + (y2 - y1), y2 - (x2 - x1), dv.Curr.X1 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y1 - (dv.Curr.X2 - dv.Curr.X1));
							}
							else
							{
								LineParameters.Bisectrix(ref prev, ref dv.Curr, out dv.XB1, out dv.YB1);
								_renderer->Line3(ref prev, x1 + (y2 - y1), y1 - (x2 - x1), dv.XB1, dv.YB1);
							}
						}
						else
						{
							_renderer->Line1(ref prev, x1 + (y2 - y1), y1 - (x2 - x1));
						}
						if ((dv.Flags & 2) == 0 && _lineJoin != OutlineAAJoin.RoundJoin)
						{
							LineParameters.Bisectrix(ref dv.Curr, ref dv.Next, out dv.XB2, out dv.YB2);
						}

						Draw(ref dv, 1, _srcVertices->Count - 2);

						if ((dv.Flags & 1) == 0)
						{
							if (_lineJoin == OutlineAAJoin.RoundJoin)
							{
								_renderer->Line3(ref dv.Curr, dv.Curr.X1 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y1 - (dv.Curr.X2 - dv.Curr.X1), dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1));
							}
							else
							{
								_renderer->Line3(ref dv.Curr, dv.XB1, dv.YB1, dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1));
							}
						}
						else
						{
							_renderer->Line2(ref dv.Curr, dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1));
						}
						if (RoundCap)
						{
							_renderer->Semidot(RendererOutlineCmp.DistEnd, dv.Curr.X2, dv.Curr.Y2, dv.Curr.X2 + (dv.Curr.Y2 - dv.Curr.Y1), dv.Curr.Y2 - (dv.Curr.X2 - dv.Curr.X1));
						}
					}
						break;
				}
			}
			_srcVertices->Clear();
		}
	}
}