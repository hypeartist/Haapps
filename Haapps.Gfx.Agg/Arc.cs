using System;

namespace Haapps.Gfx.Agg
{
	public sealed class Arc : VertexSourceAbstract
	{
		private double _x;
		private double _y;
		private double _rx;
		private double _ry;
		private double _angle;
		private double _start;
		private double _end;
		private double _scale = 1.0;
		private double _da;
		private bool _ccw;
		private bool _initialized;
		private PathCommand _command;

		public Arc()
		{
		}

		public Arc(double x, double y, double rx, double ry, double a1, double a2, bool ccw = true)
		{
			_x = x;
			_y = y;
			_rx = rx;
			_ry = ry;
			Normalize(a1, a2, ccw);
		}

		public void Initialize(double x, double y, double rx, double ry, double a1, double a2, bool ccw = true)
		{
			_x = x;
			_y = y;
			_rx = rx;
			_ry = ry;
			Normalize(a1, a2, ccw);
		}

		public double ApproximationScale
		{
			set
			{
				_scale = value;
				if (_initialized)
				{
					Normalize(_start, _end, _ccw);
				}
			}
			get => _scale;
		}

		private void Normalize(double a1, double a2, bool ccw)
		{
			var ra = (Math.Abs(_rx) + Math.Abs(_ry))/2;
			_da = Math.Acos(ra/(ra + 0.125/_scale))*2;
			if (ccw)
			{
				while (a2 < a1) a2 += Common.PiMul2;
			}
			else
			{
				while (a1 < a2) a1 += Common.PiMul2;
				_da = -_da;
			}
			_ccw = ccw;
			_start = a1;
			_end = a2;
			_initialized = true;
		}

		public override void Rewind(int pathId = 0)
		{
			_command = PathCommand.MoveTo;
			_angle = _start;
		}

		public override PathCommand Vertex(ref double x, ref double y)
		{
			if (_command.Stop())
			{
				return PathCommand.Stop;
			}
			if ((_angle < _end - _da/4) != _ccw)
			{
				x = _x + Math.Cos(_end)*_rx;
				y = _y + Math.Sin(_end)*_ry;
				_command = PathCommand.Stop;
				return PathCommand.LineTo;
			}

			x = _x + Math.Cos(_angle)*_rx;
			y = _y + Math.Sin(_angle)*_ry;

			_angle += _da;

			var pf = _command;
			_command = PathCommand.LineTo;
			return pf;
		}
		
		public override void Dispose()
		{
		}
	}
}