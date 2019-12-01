using System;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed class VcgenStroke : VertexGeneratorAbstract
	{
		private readonly PodList<Point64> _outVertices = new PodList<Point64>(64);
		private readonly PodList<VertexDistance> _srcVertices = new PodList<VertexDistance>(64);
		private int _closed;
		private int _outVertex;
		private Status _prevStatus;
		private int _srcVertex;
		private Status _status;
		private double _strokeWidth = 0.5;
		private double _widthAbs = 0.5;
		private double _widthEps = 0.5 / 1024.0;
		private int _widthSign = 1;

		public double Shorten { get; set; }

		public double InnerMiterLimit { get; set; } = 1.01;

		public double ApproximationScale { get; set; } = 1.0;

		public LineJoin LineJoin { get; set; } = LineJoin.MiterJoin;

		public LineCap LineCap { get; set; } = LineCap.Butt;

		public InnerJoin InnerJoin { get; set; } = InnerJoin.InnerMiter;

		public double Width
		{
			get => _strokeWidth * 2.0;

			set
			{
				_strokeWidth = value * 0.5;
				if (_strokeWidth < 0)
				{
					_widthAbs = -_strokeWidth;
					_widthSign = -1;
				}
				else
				{
					_widthAbs = _strokeWidth;
					_widthSign = 1;
				}

				_widthEps = _strokeWidth / 1024.0;
			}
		}

		public double MiterLimitTheta
		{
			set => MiterLimit = 1.0 / Math.Sin(value * 0.5);
		}

		public double MiterLimit { get; set; } = 4;

		public override void RemoveAll()
		{
			_srcVertices.Clear();
			_closed = 0;
			_status = Status.Initial;
		}

		public override void AddVertex(double x, double y, PathCommand cmd)
		{
			_status = Status.Initial;
			if (cmd.MoveTo())
			{
				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				AddVertex(x, y);
			}
			else
			{
				if (cmd.Vertex())
				{
					AddVertex(x, y);
				}
				else
				{
					_closed = cmd.GetCloseFlag();
				}
			}
		}

		public override void Rewind(int pathId = 0)
		{
			if (_status == Status.Initial)
			{
				CloseVertexPath(_closed != 0);
				if (Shorten > 0.0 && _srcVertices.Count > 1)
				{
					double d;
					var n = _srcVertices.Count - 2;
					while (n != 0)
					{
						d = _srcVertices[n].Distance;
						if (d > Shorten)
						{
							break;
						}

						if (_srcVertices.Count != 0)
						{
							_srcVertices.RemoveLast();
						}

						Shorten -= d;
						--n;
					}

					if (_srcVertices.Count < 2)
					{
						_srcVertices.RemoveLast();
					}
					else
					{
						n = _srcVertices.Count - 1;
						ref var prev = ref _srcVertices[n - 1];
						ref var last = ref _srcVertices[n];
						d = (prev.Distance - Shorten) / prev.Distance;
						var x = prev.X + (last.X - prev.X) * d;
						var y = prev.Y + (last.Y - prev.Y) * d;
						last.X = x;
						last.Y = y;
						_srcVertices[n] = last;
						_srcVertices[n - 1] = prev;
						if (!prev.Measure(ref last))
						{
							if (_srcVertices.Count != 0)
							{
								_srcVertices.RemoveLast();
							}
						}

						CloseVertexPath(_closed != 0);
					}
				}

				if (_srcVertices.Count < 3)
				{
					_closed = 0;
				}
			}

			_status = Status.Ready;
			_srcVertex = 0;
			_outVertex = 0;
		}

		public override PathCommand Vertex(ref double x, ref double y)
		{
			var cmd = PathCommand.LineTo;
			while (!cmd.Stop())
			{
				switch (_status)
				{
					case Status.Initial:
						Rewind();
						_status = Status.Ready;
						break;

					case Status.Ready:
						if (_srcVertices.Count < 2 + (_closed != 0 ? 1 : 0))
						{
							cmd = PathCommand.Stop;
							break;
						}

						_status = _closed != 0 ? Status.Outline1 : Status.Cap1;
						cmd = PathCommand.MoveTo;
						_srcVertex = 0;
						_outVertex = 0;
						break;

					case Status.Cap1:
						CalcCap(ref _srcVertices[0], ref _srcVertices[1], _srcVertices[0].Distance);
						_srcVertex = 1;
						_prevStatus = Status.Outline1;
						_status = Status.OutVertices;
						_outVertex = 0;
						break;

					case Status.Cap2:
						CalcCap(ref _srcVertices[_srcVertices.Count - 1], ref _srcVertices[_srcVertices.Count - 2], _srcVertices[_srcVertices.Count - 2].Distance);
						_prevStatus = Status.Outline2;
						_status = Status.OutVertices;
						_outVertex = 0;
						break;

					case Status.Outline1:
						if (_closed != 0)
						{
							if (_srcVertex >= _srcVertices.Count)
							{
								_prevStatus = Status.CloseFirst;
								_status = Status.EndPoly1;
								break;
							}
						}
						else
						{
							if (_srcVertex >= _srcVertices.Count - 1)
							{
								_status = Status.Cap2;
								break;
							}
						}

						CalcJoin(ref _srcVertices[(_srcVertex + _srcVertices.Count - 1) % _srcVertices.Count], ref _srcVertices[_srcVertex], ref _srcVertices[(_srcVertex + 1) % _srcVertices.Count], _srcVertices[(_srcVertex + _srcVertices.Count - 1) % _srcVertices.Count].Distance, _srcVertices[_srcVertex].Distance);
						++_srcVertex;
						_prevStatus = _status;
						_status = Status.OutVertices;
						_outVertex = 0;
						break;

					case Status.CloseFirst:
						_status = Status.Outline2;
						cmd = PathCommand.MoveTo;
						_status = Status.Outline2;
						break;

					case Status.Outline2:
						if (_srcVertex <= (_closed == 0 ? 1 : 0))
						{
							_status = Status.EndPoly2;
							_prevStatus = Status.Stop;
							break;
						}

						--_srcVertex;
						CalcJoin(ref _srcVertices[(_srcVertex + 1) % _srcVertices.Count], ref _srcVertices[_srcVertex], ref _srcVertices[(_srcVertex + _srcVertices.Count - 1) % _srcVertices.Count], _srcVertices[_srcVertex].Distance, _srcVertices[(_srcVertex + _srcVertices.Count - 1) % _srcVertices.Count].Distance);

						_prevStatus = _status;
						_status = Status.OutVertices;
						_outVertex = 0;
						break;

					case Status.OutVertices:
						if (_outVertex >= _outVertices.Count)
						{
							_status = _prevStatus;
						}
						else
						{
							var c = _outVertices[_outVertex++];
							x = c.X;
							y = c.Y;
							return cmd;
						}

						break;

					case Status.EndPoly1:
						_status = _prevStatus;
						return PathCommand.EndPoly | (PathCommand) (PathFlags.Close | PathFlags.Ccw);

					case Status.EndPoly2:
						_status = _prevStatus;
						return PathCommand.EndPoly | (PathCommand) (PathFlags.Close | PathFlags.Cw);

					case Status.Stop:
						cmd = PathCommand.Stop;
						break;
				}
			}

			return cmd;
		}

		private void AddVertex(double x, double y, double dist = 0)
		{
			if (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 2];
				ref var vd2 = ref _srcVertices[_srcVertices.Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (!ret)
				{
					if (_srcVertices.Count != 0)
					{
						_srcVertices.RemoveLast();
					}
				}
			}

			_srcVertices.Add(new VertexDistance(x, y, dist));
		}

		private void CloseVertexPath(bool closed)
		{
			while (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 2];
				ref var vd2 = ref _srcVertices[_srcVertices.Count - 1];
				var ret = vd1.Measure(ref vd2);
				if (ret)
				{
					break;
				}

				var t = _srcVertices[_srcVertices.Count - 1];
				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}

				AddVertex(t.X, t.Y, t.Distance);
			}

			if (!closed)
			{
				return;
			}

			while (_srcVertices.Count > 1)
			{
				ref var vd1 = ref _srcVertices[_srcVertices.Count - 1];
				ref var vd2 = ref _srcVertices[0];
				var ret = vd1.Measure(ref vd2);
				if (ret)
				{
					break;
				}

				if (_srcVertices.Count != 0)
				{
					_srcVertices.RemoveLast();
				}
			}
		}

		private void CalcArc(double x, double y, double dx1, double dy1, double dx2, double dy2)
		{
			var a1 = Math.Atan2(dy1 * _widthSign, dx1 * _widthSign);
			var a2 = Math.Atan2(dy2 * _widthSign, dx2 * _widthSign);
			int i, n;

			var da = Math.Acos(_widthAbs / (_widthAbs + 0.125 / ApproximationScale)) * 2;

			AddPoint(x + dx1, y + dy1);
			if (_widthSign > 0)
			{
				if (a1 > a2)
				{
					a2 += Common.PiMul2;
				}

				n = (int) ((a2 - a1) / da);
				da = (a2 - a1) / (n + 1);
				a1 += da;
				for (i = 0; i < n; i++)
				{
					AddPoint(x + Math.Cos(a1) * _strokeWidth, y + Math.Sin(a1) * _strokeWidth);
					a1 += da;
				}
			}
			else
			{
				if (a1 < a2)
				{
					a2 -= Common.PiMul2;
				}

				n = (int) ((a1 - a2) / da);
				da = (a1 - a2) / (n + 1);
				a1 -= da;
				for (i = 0; i < n; i++)
				{
					AddPoint(x + Math.Cos(a1) * _strokeWidth, y + Math.Sin(a1) * _strokeWidth);
					a1 -= da;
				}
			}

			AddPoint(x + dx2, y + dy2);
		}

		private void CalcMiter(ref VertexDistance v0, ref VertexDistance v1, ref VertexDistance v2, double dx1, double dy1, double dx2, double dy2, LineJoin lj, double mlimit, double dbevel)
		{
			var xi = v1.X;
			var yi = v1.Y;
			var di = 1.0;
			var lim = _widthAbs * mlimit;
			var miterLimitExceeded = true;
			var intersectionFailed = true;

			if (Common.CalcIntersection(v0.X + dx1, v0.Y - dy1, v1.X + dx1, v1.Y - dy1, v1.X + dx2, v1.Y - dy2, v2.X + dx2, v2.Y - dy2, ref xi, ref yi))
			{
				di = Common.CalcDistance(v1.X, v1.Y, xi, yi);
				if (di <= lim)
				{
					AddPoint(xi, yi);
					miterLimitExceeded = false;
				}

				intersectionFailed = false;
			}
			else
			{
				var x2 = v1.X + dx1;
				var y2 = v1.Y - dy1;
				if (Common.CrossProduct(v0.X, v0.Y, v1.X, v1.Y, x2, y2) < 0.0 == Common.CrossProduct(v1.X, v1.Y, v2.X, v2.Y, x2, y2) < 0.0)
				{
					AddPoint(v1.X + dx1, v1.Y - dy1);
					miterLimitExceeded = false;
				}
			}

			if (!miterLimitExceeded)
			{
				return;
			}

			switch (lj)
			{
				case LineJoin.MiterJoinRevert:

					AddPoint(v1.X + dx1, v1.Y - dy1);
					AddPoint(v1.X + dx2, v1.Y - dy2);
					break;

				case LineJoin.MiterJoinRound:
					CalcArc(v1.X, v1.Y, dx1, -dy1, dx2, -dy2);
					break;

				default:
					if (intersectionFailed)
					{
						mlimit *= _widthSign;
						AddPoint(v1.X + dx1 + dy1 * mlimit, v1.Y - dy1 + dx1 * mlimit);
						AddPoint(v1.X + dx2 - dy2 * mlimit, v1.Y - dy2 - dx2 * mlimit);
					}
					else
					{
						var x1 = v1.X + dx1;
						var y1 = v1.Y - dy1;
						var x2 = v1.X + dx2;
						var y2 = v1.Y - dy2;
						di = (lim - dbevel) / (di - dbevel);
						AddPoint(x1 + (xi - x1) * di, y1 + (yi - y1) * di);
						AddPoint(x2 + (xi - x2) * di, y2 + (yi - y2) * di);
					}

					break;
			}
		}

		private void CalcCap(ref VertexDistance v0, ref VertexDistance v1, double len)
		{
			_outVertices.Clear();

			var dx1 = (v1.Y - v0.Y) / len;
			var dy1 = (v1.X - v0.X) / len;
			double dx2 = 0;
			double dy2 = 0;

			dx1 *= _strokeWidth;
			dy1 *= _strokeWidth;

			if (LineCap != LineCap.Round)
			{
				if (LineCap == LineCap.Square)
				{
					dx2 = dy1 * _widthSign;
					dy2 = dx1 * _widthSign;
				}

				AddPoint(v0.X - dx1 - dx2, v0.Y + dy1 - dy2);
				AddPoint(v0.X + dx1 - dx2, v0.Y - dy1 - dy2);
			}
			else
			{
				var da = Math.Acos(_widthAbs / (_widthAbs + 0.125 / ApproximationScale)) * 2;
				double a1;
				int i;
				var n = (int) (Common.Pi / da);

				da = Common.Pi / (n + 1);
				AddPoint(v0.X - dx1, v0.Y + dy1);
				if (_widthSign > 0)
				{
					a1 = Math.Atan2(dy1, -dx1);
					a1 += da;
					for (i = 0; i < n; i++)
					{
						AddPoint(v0.X + Math.Cos(a1) * _strokeWidth, v0.Y + Math.Sin(a1) * _strokeWidth);
						a1 += da;
					}
				}
				else
				{
					a1 = Math.Atan2(-dy1, dx1);
					a1 -= da;
					for (i = 0; i < n; i++)
					{
						AddPoint(v0.X + Math.Cos(a1) * _strokeWidth, v0.Y + Math.Sin(a1) * _strokeWidth);
						a1 -= da;
					}
				}

				AddPoint(v0.X + dx1, v0.Y - dy1);
			}
		}

		private void CalcJoin(ref VertexDistance v0, ref VertexDistance v1, ref VertexDistance v2, double len1, double len2)
		{
			var dx1 = _strokeWidth * (v1.Y - v0.Y) / len1;
			var dy1 = _strokeWidth * (v1.X - v0.X) / len1;
			var dx2 = _strokeWidth * (v2.Y - v1.Y) / len2;
			var dy2 = _strokeWidth * (v2.X - v1.X) / len2;

			_outVertices.Clear();

			var cp = Common.CrossProduct(v0.X, v0.Y, v1.X, v1.Y, v2.X, v2.Y);
			if (Math.Abs(cp) > double.Epsilon && cp > 0 == _strokeWidth > 0)
			{
				var limit = (len1 < len2 ? len1 : len2) / _widthAbs;
				if (limit < InnerMiterLimit)
				{
					limit = InnerMiterLimit;
				}

				switch (InnerJoin)
				{
					default: // inner_bevel
						AddPoint(v1.X + dx1, v1.Y - dy1);
						AddPoint(v1.X + dx2, v1.Y - dy2);
						break;

					case InnerJoin.InnerMiter:
						CalcMiter(ref v0, ref v1, ref v2, dx1, dy1, dx2, dy2, LineJoin.MiterJoinRevert, limit, 0);
						break;

					case InnerJoin.InnerJag:
					case InnerJoin.InnerRound:
						cp = (dx1 - dx2) * (dx1 - dx2) + (dy1 - dy2) * (dy1 - dy2);
						if (cp < len1 * len1 && cp < len2 * len2)
						{
							CalcMiter(ref v0, ref v1, ref v2, dx1, dy1, dx2, dy2, LineJoin.MiterJoinRevert, limit, 0);
						}
						else
						{
							if (InnerJoin == InnerJoin.InnerJag)
							{
								AddPoint(v1.X + dx1, v1.Y - dy1);
								AddPoint(v1.X, v1.Y);
								AddPoint(v1.X + dx2, v1.Y - dy2);
							}
							else
							{
								AddPoint(v1.X + dx1, v1.Y - dy1);
								AddPoint(v1.X, v1.Y);
								CalcArc(v1.X, v1.Y, dx2, -dy2, dx1, -dy1);
								AddPoint(v1.X, v1.Y);
								AddPoint(v1.X + dx2, v1.Y - dy2);
							}
						}

						break;
				}
			}
			else
			{
				var dx = (dx1 + dx2) / 2;
				var dy = (dy1 + dy2) / 2;
				var dbevel = Math.Sqrt(dx * dx + dy * dy);

				if (LineJoin == LineJoin.RoundJoin || LineJoin == LineJoin.BevelJoin)
				{
					if (ApproximationScale * (_widthAbs - dbevel) < _widthEps)
					{
						if (Common.CalcIntersection(v0.X + dx1, v0.Y - dy1, v1.X + dx1, v1.Y - dy1, v1.X + dx2, v1.Y - dy2, v2.X + dx2, v2.Y - dy2, ref dx, ref dy))
						{
							AddPoint(dx, dy);
						}
						else
						{
							AddPoint(v1.X + dx1, v1.Y - dy1);
						}

						return;
					}
				}

				switch (LineJoin)
				{
					case LineJoin.MiterJoin:
					case LineJoin.MiterJoinRevert:
					case LineJoin.MiterJoinRound:
						CalcMiter(ref v0, ref v1, ref v2, dx1, dy1, dx2, dy2, LineJoin, MiterLimit, dbevel);
						break;

					case LineJoin.RoundJoin:
						CalcArc(v1.X, v1.Y, dx1, -dy1, dx2, -dy2);
						break;

					default:
						AddPoint(v1.X + dx1, v1.Y - dy1);
						AddPoint(v1.X + dx2, v1.Y - dy2);
						break;
				}
			}
		}

		private void AddPoint(double x, double y) => _outVertices.Add(new Point64(x, y));

		public override void Dispose()
		{
			_srcVertices?.Dispose();
			_outVertices?.Dispose();
		}

		private enum Status
		{
			Initial,
			Ready,
			Cap1,
			Cap2,
			Outline1,
			CloseFirst,
			Outline2,
			OutVertices,
			EndPoly1,
			EndPoly2,
			Stop
		}
	}
}