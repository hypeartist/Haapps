using System;

namespace Haapps.Gfx.Agg
{
	public sealed class Ellipse : VertexSourceAbstract
	{
		private bool _cw;
		private int _num;
		private double _rx;
		private double _ry;
		private double _scale = 1.0;
		private int _step;
		private double _x;
		private double _y;

		public Ellipse()
		{
		}

		public Ellipse(double x, double y, double rx, double ry, int numSteps = 0, bool cw = false)
		{
			_x = x;
			_y = y;
			_rx = rx;
			_ry = ry;
			_num = numSteps;
			_cw = cw;
			_step = 0;
			if (_num != 0)
			{
				return;
			}

			var ra = (Math.Abs(_rx) + Math.Abs(_ry)) / 2;
			var da = Math.Acos(ra / (ra + 0.125 / _scale)) * 2;
			_num = (int) (Common.PiMul2 / da + 0.5);
		}

		public double ApproxScale
		{
			set
			{
				_scale = value;
				var ra = (Math.Abs(_rx) + Math.Abs(_ry)) / 2;
				var da = Math.Acos(ra / (ra + 0.125 / _scale)) * 2;
				_num = (int) (Common.PiMul2 / da + 0.5);
			}
		}

		public void Init(double x, double y, double rx, double ry, int numSteps = 0, bool cw = false)
		{
			_x = x;
			_y = y;
			_rx = rx;
			_ry = ry;
			_num = numSteps;
			_cw = cw;
			_step = 0;
			if (_num != 0)
			{
				return;
			}

			var ra = (Math.Abs(_rx) + Math.Abs(_ry)) / 2;
			var da = Math.Acos(ra / (ra + 0.125 / _scale)) * 2;
			_num = (int) (Common.PiMul2 / da + 0.5);
		}

		public override void Rewind(int pathId) => _step = 0;

		public override PathCommand Vertex(ref double x, ref double y)
		{
			if (_step == _num)
			{
				++_step;
				return PathCommand.EndPoly | (PathCommand) (PathFlags.Close | PathFlags.Ccw);
			}

			if (_step > _num)
			{
				return PathCommand.Stop;
			}

			var angle = _step / (double) _num * Common.PiMul2;
			if (_cw)
			{
				angle = Common.PiMul2 - angle;
			}

			x = _x + Math.Cos(angle) * _rx;
			y = _y + Math.Sin(angle) * _ry;
			_step++;
			return _step == 1 ? PathCommand.MoveTo : PathCommand.LineTo;
		}

		public override void Dispose()
		{
		}
	}
}