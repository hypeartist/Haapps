using System;

namespace Haapps.Gfx.Agg
{
	public sealed class BezierArcSvg : VertexSourceAbstract
	{
		private readonly BezierArc _arc = new BezierArc();

		public bool RadiiOk { get; private set; }

		public void Init(double x0, double y0, double rx, double ry, double angle, bool largeArcFlag, bool sweepFlag, double x2, double y2)
		{
			RadiiOk = true;

			if (rx < 0.0)
			{
				rx = -rx;
			}

			if (ry < 0.0)
			{
				ry = -rx;
			}

			var dx2 = (x0 - x2) / 2.0;
			var dy2 = (y0 - y2) / 2.0;

			var cosA = Math.Cos(angle);
			var sinA = Math.Sin(angle);

			var x1 = cosA * dx2 + sinA * dy2;
			var y1 = -sinA * dx2 + cosA * dy2;

			var prx = rx * rx;
			var pry = ry * ry;
			var px1 = x1 * x1;
			var py1 = y1 * y1;

			var radiiCheck = px1 / prx + py1 / pry;
			if (radiiCheck > 1.0)
			{
				rx = Math.Sqrt(radiiCheck) * rx;
				ry = Math.Sqrt(radiiCheck) * ry;
				prx = rx * rx;
				pry = ry * ry;
				if (radiiCheck > 10.0)
				{
					RadiiOk = false;
				}
			}

			var sign = largeArcFlag == sweepFlag ? -1.0 : 1.0;
			var sq = (prx * pry - prx * py1 - pry * px1) / (prx * py1 + pry * px1);
			var coef = sign * Math.Sqrt(sq < 0 ? 0 : sq);
			var cx1 = coef * (rx * y1 / ry);
			var cy1 = coef * -(ry * x1 / rx);

			var sx2 = (x0 + x2) / 2.0;
			var sy2 = (y0 + y2) / 2.0;
			var cx = sx2 + (cosA * cx1 - sinA * cy1);
			var cy = sy2 + (sinA * cx1 + cosA * cy1);

			var ux = (x1 - cx1) / rx;
			var uy = (y1 - cy1) / ry;
			var vx = (-x1 - cx1) / rx;
			var vy = (-y1 - cy1) / ry;

			var n = Math.Sqrt(ux * ux + uy * uy);
			var p = ux;
			sign = uy < 0 ? -1.0 : 1.0;
			var v = p / n;
			if (v < -1.0)
			{
				v = -1.0;
			}

			if (v > 1.0)
			{
				v = 1.0;
			}

			var startAngle = sign * Math.Acos(v);

			n = Math.Sqrt((ux * ux + uy * uy) * (vx * vx + vy * vy));
			p = ux * vx + uy * vy;
			sign = ux * vy - uy * vx < 0 ? -1.0 : 1.0;
			v = p / n;
			if (v < -1.0)
			{
				v = -1.0;
			}

			if (v > 1.0)
			{
				v = 1.0;
			}

			var sweepAngle = sign * Math.Acos(v);
			if (!sweepFlag && sweepAngle > 0)
			{
				sweepAngle -= Common.PiMul2;
			}
			else if (sweepFlag && sweepAngle < 0)
			{
				sweepAngle += Common.PiMul2;
			}

			_arc.Init(0.0, 0.0, rx, ry, startAngle, sweepAngle);
			var mtx = TransformAffine.AffineRotation(angle);
			mtx *= TransformAffine.AffineTranslation(cx, cy);

			for (var i = 2; i < _arc.VerticesCount - 2; i += 2)
			{
				var x = _arc.Vertices[i];
				var y = _arc.Vertices[i + 1];
				mtx.Transform(ref x, ref y);
				_arc.Vertices[i] = x;
				_arc.Vertices[i + 1] = y;
			}

			_arc.Vertices[0] = x0;
			_arc.Vertices[1] = y0;
			if (_arc.VerticesCount <= 2)
			{
				return;
			}

			_arc.Vertices[_arc.VerticesCount - 2] = x2;
			_arc.Vertices[_arc.VerticesCount - 1] = y2;
		}

		public override void Rewind(int pathId = 0) => _arc.Rewind();

		public override PathCommand Vertex(ref double x, ref double y) => _arc.Vertex(ref x, ref y);

		public override void Dispose() => _arc.Dispose();
	}
}