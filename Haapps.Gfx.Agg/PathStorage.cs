using System;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed class PathStorage : VertexSourceAbstract
	{
		private static PathItem _stopPathItem = PathItem.Stop;

		private readonly PodList<PathItem> _pathItems = new PodList<PathItem>(128);
		private int _iterator;

		private ref PathItem FirstItem
		{
			get
			{
				if (_pathItems.Count == 0)
				{
					return ref _stopPathItem;
				}

				return ref _pathItems[0];
			}
		}

		private ref PathItem LastItem
		{
			get
			{
				if (_pathItems.Count == 0)
				{
					return ref _stopPathItem;
				}

				return ref _pathItems[_pathItems.Count - 1];
			}
		}

		private ref PathItem PrevItem
		{
			get
			{
				if (_pathItems.Count < 2)
				{
					return ref _stopPathItem;
				}

				return ref _pathItems[_pathItems.Count - 2];
			}
		}

		public void RemoveAll()
		{
			_iterator = 0;
			_pathItems.Clear();
		}

		private void SwapVertices(int v1, int v2)
		{
			var v = _pathItems[v1];
			_pathItems[v1] = _pathItems[v2];
			_pathItems[v2] = v;
		}

		public int StartNewPath()
		{
			if (LastItem.Command != PathCommand.Stop)
			{
				_pathItems.Add(new PathItem(0.0, 0.0, PathCommand.Stop));
			}

			return _pathItems.Count;
		}

		public int PerceivePolygonOrientation(int start, int end)
		{
			var np = end - start;
			var area = 0.0;
			for (var i = 0; i < np; i++)
			{
				double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
				Vertex(start + i, ref x1, ref y1);
				Vertex(start + (i + 1) % np, ref x2, ref y2);
				area += x1 * y2 - y1 * x2;
			}

			return (int) (area < 0.0 ? PathFlags.Cw : PathFlags.Ccw);
		}

		public void InvertPolygon(int start, int end)
		{
			var tmpCmd = _pathItems[start].Command;
			--end;

			for (var i = start; i < end; i++)
			{
				_pathItems[i].Command = _pathItems[i + 1].Command;
			}

			_pathItems[end].Command = tmpCmd;

			while (end > start)
			{
				SwapVertices(start++, end--);
			}
		}

		public int ArrangePolygonOrientation(int start, PathFlags orientation)
		{
			if (orientation == PathFlags.None)
			{
				return start;
			}

			while (start < _pathItems.Count && !_pathItems[start].Command.Vertex())
			{
				++start;
			}

			while (start + 1 < _pathItems.Count && _pathItems[start].Command.MoveTo() && _pathItems[start + 1].Command.MoveTo())
			{
				++start;
			}

			var end = start + 1;
			while (end < _pathItems.Count && !_pathItems[end].Command.NextPoly())
			{
				++end;
			}

			if (end - start <= 2)
			{
				return end;
			}

			if (PerceivePolygonOrientation(start, end) == (int) orientation)
			{
				return end;
			}

			InvertPolygon(start, end);
			PathCommand cmd;
			while (end < _pathItems.Count && (cmd = _pathItems[end].Command).EndPoly())
			{
				_pathItems[end++].Command = (PathCommand) cmd.SetOrientation(orientation);
			}

			return end;
		}

		public int ArrangeOrientations(int start, PathFlags orientation)
		{
			if (orientation == PathFlags.None)
			{
				return start;
			}

			while (start < _pathItems.Count)
			{
				start = ArrangePolygonOrientation(start, orientation);
				if (!_pathItems[start].Command.Stop())
				{
					continue;
				}

				++start;
				break;
			}

			return start;
		}

		public void ArrangeOrientationsAllPaths(PathFlags orientation)
		{
			if (orientation == PathFlags.None)
			{
				return;
			}

			var start = 0;
			while (start < _pathItems.Count)
			{
				start = ArrangeOrientations(start, orientation);
			}
		}

		public void ConcatPath(VertexSourceAbstract vs, int pathId = 0)
		{
			double x = 0, y = 0;
			PathCommand cmd;
			vs.Rewind(pathId);
			while (!(cmd = vs.Vertex(ref x, ref y)).Stop())
			{
				_pathItems.Add(new PathItem(x, y, cmd));
			}
		}

		public void Transform<TTransform>(ref TTransform transform, int i = 0)
			where TTransform : unmanaged, ITransform
		{
			for (; i < _pathItems.Count; i++)
			{
				ref var pi = ref _pathItems[i];
				if (pi.Command.Stop())
				{
					break;
				}

				if (!pi.Command.Vertex())
				{
					continue;
				}

				transform.Transform(ref pi.X, ref pi.Y);
			}
		}

		public void MoveTo(double x, double y) => _pathItems.Add(new PathItem(x, y, PathCommand.MoveTo));

		public void MoveRel(double dx, double dy)
		{
			RelToAbs(ref dx, ref dy);
			_pathItems.Add(new PathItem(dx, dy, PathCommand.MoveTo));
		}

		public void LineTo(double x, double y) => _pathItems.Add(new PathItem(x, y, PathCommand.LineTo));

		public void LineRel(double dx, double dy)
		{
			RelToAbs(ref dx, ref dy);
			_pathItems.Add(new PathItem(dx, dy, PathCommand.LineTo));
		}

		public void HLineTo(double x) => _pathItems.Add(new PathItem(x, LastItem.Y, PathCommand.LineTo));

		public void HLineRel(double dx)
		{
			double dy = 0;
			RelToAbs(ref dx, ref dy);
			_pathItems.Add(new PathItem(dx, dy, PathCommand.LineTo));
		}

		public void VLineTo(double y) => _pathItems.Add(new PathItem(LastItem.X, y, PathCommand.LineTo));

		public void VLineRel(double dy)
		{
			double dx = 0;
			RelToAbs(ref dx, ref dy);
			_pathItems.Add(new PathItem(dx, dy, PathCommand.LineTo));
		}

		public void ArcTo(double rx, double ry, double angle, bool largeArcFlag, bool sweepFlag, double x, double y)
		{
			if (LastItem.Command.Vertex())
			{
				const double epsilon = 1e-30;
				var x0 = LastItem.X;
				var y0 = LastItem.Y;

				rx = Math.Abs(rx);
				ry = Math.Abs(ry);

				if (rx < epsilon || ry < epsilon)
				{
					LineTo(x, y);
					return;
				}

				if (Common.CalcDistance(x0, y0, x, y) < epsilon)
				{
					return;
				}

				var a = new BezierArcSvg();
				a.Init(x0, y0, rx, ry, angle, largeArcFlag, sweepFlag, x, y);
				if (a.RadiiOk)
				{
					JoinPath(a);
				}
				else
				{
					LineTo(x, y);
				}
			}
			else
			{
				MoveTo(x, y);
			}
		}

		public void ArcRel(double rx, double ry, double angle, bool largeArcFlag, bool sweepFlag, double dx, double dy)
		{
			RelToAbs(ref dx, ref dy);
			ArcTo(rx, ry, angle, largeArcFlag, sweepFlag, dx, dy);
		}

		public void Curve3(double xCtrl, double yCtrl, double xTo, double yTo)
		{
			_pathItems.Add(new PathItem(xCtrl, yCtrl, PathCommand.Curve3));
			_pathItems.Add(new PathItem(xTo, yTo, PathCommand.Curve3));
		}

		public void Curve3Rel(double dxCtrl, double dyCtrl, double dxTo, double dyTo)
		{
			RelToAbs(ref dxCtrl, ref dyCtrl);
			RelToAbs(ref dxTo, ref dyTo);
			_pathItems.Add(new PathItem(dxCtrl, dyCtrl, PathCommand.Curve3));
			_pathItems.Add(new PathItem(dxTo, dyTo, PathCommand.Curve3));
		}

		public void Curve3(double xTo, double yTo)
		{
			if (!LastItem.Command.Vertex())
			{
				return;
			}

			double xCtrl;
			double yCtrl;
			if (PrevItem.Command.Curve())
			{
				xCtrl = LastItem.X + LastItem.X - PrevItem.X;
				yCtrl = LastItem.Y + LastItem.Y - PrevItem.Y;
			}
			else
			{
				xCtrl = LastItem.X;
				yCtrl = LastItem.Y;
			}

			_pathItems.Add(new PathItem(xCtrl, yCtrl, PathCommand.Curve3));
			_pathItems.Add(new PathItem(xTo, yTo, PathCommand.Curve3));
		}

		public void Curve3Rel(double dxTo, double dyTo)
		{
			RelToAbs(ref dxTo, ref dyTo);
			Curve3(dxTo, dyTo);
		}

		public void Curve4(double xCtrl1, double yCtrl1, double xCtrl2, double yCtrl2, double xTo, double yTo)
		{
			_pathItems.Add(new PathItem(xCtrl1, yCtrl1, PathCommand.Curve4));
			_pathItems.Add(new PathItem(xCtrl2, yCtrl2, PathCommand.Curve4));
			_pathItems.Add(new PathItem(xTo, yTo, PathCommand.Curve4));
		}

		public void Curve4Rel(double dxCtrl1, double dyCtrl1, double dxCtrl2, double dyCtrl2, double dxTo, double dyTo)
		{
			RelToAbs(ref dxCtrl1, ref dyCtrl1);
			RelToAbs(ref dxCtrl2, ref dyCtrl2);
			RelToAbs(ref dxTo, ref dyTo);
			_pathItems.Add(new PathItem(dxCtrl1, dyCtrl1, PathCommand.Curve4));
			_pathItems.Add(new PathItem(dxCtrl2, dyCtrl2, PathCommand.Curve4));
			_pathItems.Add(new PathItem(dxTo, dyTo, PathCommand.Curve4));
		}

		public void Curve4(double xCtrl2, double yCtrl2, double xTo, double yTo)
		{
			if (!LastItem.Command.Vertex())
			{
				return;
			}

			double xCtrl1;
			double yCtrl1;
			if (PrevItem.Command.Curve())
			{
				xCtrl1 = LastItem.X + LastItem.X - PrevItem.X;
				yCtrl1 = LastItem.Y + LastItem.Y - PrevItem.Y;
			}
			else
			{
				xCtrl1 = LastItem.X;
				yCtrl1 = LastItem.Y;
			}

			_pathItems.Add(new PathItem(xCtrl1, yCtrl1, PathCommand.Curve4));
			_pathItems.Add(new PathItem(xCtrl2, yCtrl2, PathCommand.Curve4));
			_pathItems.Add(new PathItem(xTo, yTo, PathCommand.Curve4));
		}

		public void Curve4Rel(double dxCtrl2, double dyCtrl2, double dxTo, double dyTo)
		{
			RelToAbs(ref dxCtrl2, ref dyCtrl2);
			RelToAbs(ref dxTo, ref dyTo);
			Curve4(dxCtrl2, dyCtrl2, dxTo, dyTo);
		}

		public void EndPoly(int flags)
		{
			if (LastItem.Command.Vertex())
			{
				_pathItems.Add(new PathItem(0.0, 0.0, (PathCommand) ((int) PathCommand.EndPoly | flags)));
			}
		}

		public void ClosePolygon(int flags = 0)
		{
			if (LastItem.Command.Vertex())
			{
				_pathItems.Add(new PathItem(0.0, 0.0, (PathCommand) ((int) PathCommand.EndPoly | (int) PathFlags.Close | flags)));
			}
		}

		public void ClosePolygon(PathFlags flags)
		{
			if (LastItem.Command.Vertex())
			{
				_pathItems.Add(new PathItem(0.0, 0.0, (PathCommand) ((int) PathCommand.EndPoly | (int) PathFlags.Close | (int) flags)));
			}
		}

		private void RelToAbs(ref double x, ref double y)
		{
			if (!LastItem.Command.Vertex())
			{
				return;
			}

			x += LastItem.X;
			y += LastItem.Y;
		}

		public void JoinPath(VertexSourceAbstract vs, int pathId = 0)
		{
			double x = 0, y = 0;
			vs.Rewind(pathId);
			var cmd = vs.Vertex(ref x, ref y);
			if (cmd.Stop())
			{
				return;
			}

			if (cmd.Vertex())
			{
				var li = LastItem;
				var x0 = li.X;
				var y0 = li.Y;
				var cmd0 = li.Command;
				if (cmd0.Vertex())
				{
					if (Common.CalcDistance(x, y, x0, y0) > Common.VertexDistanceEpsilon)
					{
						if (cmd.MoveTo())
						{
							cmd = PathCommand.LineTo;
						}

						_pathItems.Add(new PathItem(x, y, cmd));
					}
				}
				else
				{
					if (cmd0.Stop())
					{
						cmd = PathCommand.MoveTo;
					}
					else
					{
						if (cmd.MoveTo())
						{
							cmd = PathCommand.LineTo;
						}
					}

					_pathItems.Add(new PathItem(x, y, cmd));
				}
			}

			while (!(cmd = vs.Vertex(ref x, ref y)).Stop())
			{
				_pathItems.Add(new PathItem(x, y, cmd.MoveTo() ? PathCommand.LineTo : cmd));
			}
		}

		public override void Rewind(int pathId = 0) => _iterator = pathId;

		public override PathCommand Vertex(ref double x, ref double y) => _iterator >= _pathItems.Count ? PathCommand.Stop : _pathItems[_iterator++].Decompose(out x, out y);

		public PathCommand Vertex(int idx, ref double x, ref double y) => _iterator >= _pathItems.Count ? PathCommand.Stop : _pathItems[idx].Decompose(out x, out y);

		public override void Dispose() => _pathItems?.Dispose();
	}
}