namespace Haapps.Gfx.Agg
{
	public struct BresenhamEllipseInterpolator
	{
		private int _rx2;
		private int _ry2;
		private int _twoRx2;
		private int _twoRy2;
		private int _incX;
		private int _incY;
		private int _curF;

		public BresenhamEllipseInterpolator(int rx, int ry) : this()
		{
			_rx2 = rx * rx;
			_ry2 = ry * ry;
			_twoRx2 = _rx2 << 1;
			_twoRy2 = _ry2 << 1;
			_incY = -ry * _twoRx2;
		}

		public void Inc()
		{
			int fx, fy, fxy;

			var mx = fx = _curF + _incX + _ry2;
			if (mx < 0) mx = -mx;

			var my = fy = _curF + _incY + _rx2;
			if (my < 0) my = -my;

			var mxy = fxy = _curF + _incX + _ry2 + _incY + _rx2;
			if (mxy < 0) mxy = -mxy;

			var minM = mx;
			var flag = true;

			if (minM > my)
			{
				minM = my;
				flag = false;
			}

			DX = DY = 0;

			if (minM > mxy)
			{
				_incX = _incX + _twoRy2;
				_incY = _incY + _twoRx2;
				_curF = fxy;
				DX = 1;
				DY = 1;
				return;
			}

			if (flag)
			{
				_incX = _incX + _twoRy2;
				_curF = fx;
				DX = 1;
				return;
			}

			_incY += _twoRx2;
			_curF = fy;
			DY = 1;
		}

		public int DX { get; set; }

		public int DY { get; private set; }
	}
}