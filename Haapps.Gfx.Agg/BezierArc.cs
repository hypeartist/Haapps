using System;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed unsafe class BezierArc : VertexSourceAbstract
	{
		private readonly PodArray<double> _px = new PodArray<double>(4);
		private readonly PodArray<double> _py = new PodArray<double>(4);
		private PathCommand _command;
		private int _vertex;

		public BezierArc()
		{
		}

		public BezierArc(double x, double y, double rx, double ry, double startAngle, double sweepAngle) => Init(x, y, rx, ry, startAngle, sweepAngle);

		public int VerticesCount { get; private set; }

		public PodArray<double> Vertices { get; } = new PodArray<double>(26);

		private void ArcToBezier(double cx, double cy, double rx, double ry, double startAngle, double sweepAngle, double* curve)
		{
			var x0 = Math.Cos(sweepAngle / 2.0);
			var y0 = Math.Sin(sweepAngle / 2.0);
			var tx = (1.0 - x0) * 4.0 / 3.0;
			var ty = y0 - tx * x0 / y0;
			_px[0] = x0;
			_py[0] = -y0;
			_px[1] = x0 + tx;
			_py[1] = -ty;
			_px[2] = x0 + tx;
			_py[2] = ty;
			_px[3] = x0;
			_py[3] = y0;

			var sn = Math.Sin(startAngle + sweepAngle / 2.0);
			var cs = Math.Cos(startAngle + sweepAngle / 2.0);

			for (var i = 0; i < 4; i++)
			{
				curve[i * 2] = cx + rx * (_px[i] * cs - _py[i] * sn);
				curve[i * 2 + 1] = cy + ry * (_px[i] * sn + _py[i] * cs);
			}
		}

		public void Init(double x, double y, double rx, double ry, double startAngle, double sweepAngle)
		{
			startAngle = startAngle % Common.PiMul2;
			if (sweepAngle >= Common.PiMul2)
			{
				sweepAngle = Common.PiMul2;
			}

			if (sweepAngle <= -Common.PiMul2)
			{
				sweepAngle = -Common.PiMul2;
			}

			if (Math.Abs(sweepAngle) < 1e-10)
			{
				VerticesCount = 4;
				_command = PathCommand.LineTo;
				Vertices[0] = x + rx * Math.Cos(startAngle);
				Vertices[1] = y + ry * Math.Sin(startAngle);
				Vertices[2] = x + rx * Math.Cos(startAngle + sweepAngle);
				Vertices[3] = y + ry * Math.Sin(startAngle + sweepAngle);
				return;
			}

			var totalSweep = 0.0;
			VerticesCount = 2;
			_command = PathCommand.Curve4;
			var done = false;
			do
			{
				double localSweep;
				double prevSweep;
				if (sweepAngle < 0.0)
				{
					prevSweep = totalSweep;
					localSweep = -Common.PiDiv2;
					totalSweep -= Common.PiDiv2;
					if (totalSweep <= sweepAngle + Common.BezierArcAngleEpsilon)
					{
						localSweep = sweepAngle - prevSweep;
						done = true;
					}
				}
				else
				{
					prevSweep = totalSweep;
					localSweep = Common.PiDiv2;
					totalSweep += Common.PiDiv2;
					if (totalSweep >= sweepAngle - Common.BezierArcAngleEpsilon)
					{
						localSweep = sweepAngle - prevSweep;
						done = true;
					}
				}

				ArcToBezier(x, y, rx, ry, startAngle, localSweep, Vertices.DataPtr + VerticesCount - 2);
				VerticesCount = VerticesCount + 6;
				startAngle += localSweep;
			} while (!done && VerticesCount < 26);
		}

		public override void Rewind(int pathId = 0) => _vertex = 0;

		public override PathCommand Vertex(ref double x, ref double y)
		{
			if (_vertex >= VerticesCount)
			{
				return PathCommand.Stop;
			}

			x = Vertices[_vertex];
			y = Vertices[_vertex + 1];
			_vertex += 2;
			return _vertex == 2 ? PathCommand.MoveTo : _command;
		}

		public override void Dispose()
		{
			_px?.Dispose();
			_py?.Dispose();
			Vertices?.Dispose();
		}
	}
}