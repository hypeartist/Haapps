using System;

namespace Haapps.Gfx.Agg
{
	public struct TransformViewport : ITransform
	{
		public static TransformViewport Default = new TransformViewport
		{
			_alignX = 0.5,
			_alignY = 0.5,
			_deviceX2 = 1.0,
			_deviceY2 = 1.0,
			_worldX2 = 1.0,
			_worldY2 = 1.0,
			_wx2 = 1.0,
			_wy2 = 1.0
		};

		private double _alignX;
		private double _alignY;
		private AspectRatio _aspect;
		private double _deviceX1;
		private double _deviceX2;
		private double _deviceY1;
		private double _deviceY2;
		private double _dx1;
		private double _dy1;
		private bool _isValid;
		private double _worldX1;
		private double _worldX2;
		private double _worldY1;
		private double _worldY2;
		private double _wx1;
		private double _wx2;
		private double _wy1;
		private double _wy2;

		public double DeviceDx => _dx1 - _wx1 * ScaleX;

		public double DeviceDy => _dy1 - _wy1 * ScaleY;

		public double ScaleX { get; private set; }

		public double ScaleY { get; private set; }

		public double Scale => (ScaleX + ScaleY) * 0.5;

		private void Update()
		{
			const double epsilon = 1e-30;
			if (Math.Abs(_worldX1 - _worldX2) < epsilon || Math.Abs(_worldY1 - _worldY2) < epsilon || Math.Abs(_deviceX1 - _deviceX2) < epsilon || Math.Abs(_deviceY1 - _deviceY2) < epsilon)
			{
				_wx1 = _worldX1;
				_wy1 = _worldY1;
				_wx2 = _worldX1 + 1.0;
				_wy2 = _worldY2 + 1.0;
				_dx1 = _deviceX1;
				_dy1 = _deviceY1;
				ScaleX = 1.0;
				ScaleY = 1.0;
				_isValid = false;
				return;
			}

			var worldX1 = _worldX1;
			var worldY1 = _worldY1;
			var worldX2 = _worldX2;
			var worldY2 = _worldY2;
			var deviceX1 = _deviceX1;
			var deviceY1 = _deviceY1;
			var deviceX2 = _deviceX2;
			var deviceY2 = _deviceY2;

			if (_aspect != AspectRatio.Stretch)
			{
				double d;
				ScaleX = (deviceX2 - deviceX1) / (worldX2 - worldX1);
				ScaleY = (deviceY2 - deviceY1) / (worldY2 - worldY1);

				if (_aspect == AspectRatio.Meet == ScaleX < ScaleY)
				{
					d = (worldY2 - worldY1) * ScaleY / ScaleX;
					worldY1 += (worldY2 - worldY1 - d) * _alignY;
					worldY2 = worldY1 + d;
				}
				else
				{
					d = (worldX2 - worldX1) * ScaleX / ScaleY;
					worldX1 += (worldX2 - worldX1 - d) * _alignX;
					worldX2 = worldX1 + d;
				}
			}

			_wx1 = worldX1;
			_wy1 = worldY1;
			_wx2 = worldX2;
			_wy2 = worldY2;
			_dx1 = deviceX1;
			_dy1 = deviceY1;
			ScaleX = (deviceX2 - deviceX1) / (worldX2 - worldX1);
			ScaleY = (deviceY2 - deviceY1) / (worldY2 - worldY1);
			_isValid = true;
		}

		public void PreserveAspectRatio(double alignx, double aligny, AspectRatio aspect)
		{
			_alignX = alignx;
			_alignY = aligny;
			_aspect = aspect;
			Update();
		}

		public void DeviceViewport(double x1, double y1, double x2, double y2)
		{
			_deviceX1 = x1;
			_deviceY1 = y1;
			_deviceX2 = x2;
			_deviceY2 = y2;
			Update();
		}

		public void WorldViewport(double x1, double y1, double x2, double y2)
		{
			_worldX1 = x1;
			_worldY1 = y1;
			_worldX2 = x2;
			_worldY2 = y2;
			Update();
		}

		public void DeviceViewport(out double x1, out double y1, out double x2, out double y2)
		{
			x1 = _deviceX1;
			y1 = _deviceY1;
			x2 = _deviceX2;
			y2 = _deviceY2;
		}

		public void WorldViewport(out double x1, out double y1, out double x2, out double y2)
		{
			x1 = _worldX1;
			y1 = _worldY1;
			x2 = _worldX2;
			y2 = _worldY2;
		}

		public void WorldViewportActual(out double x1, out double y1, out double x2, out double y2)
		{
			x1 = _wx1;
			y1 = _wy1;
			x2 = _wx2;
			y2 = _wy2;
		}

		public void Transform(ref double x, ref double y)
		{
			x = (x - _wx1) * ScaleX + _dx1;
			y = (y - _wy1) * ScaleY + _dy1;
		}

		public void TransformScaleOnly(ref double x, ref double y)
		{
			x *= ScaleX;
			y *= ScaleY;
		}

		public void InverseTransform(ref double x, ref double y)
		{
			x = (x - _dx1) / ScaleX + _wx1;
			y = (y - _dy1) / ScaleY + _wy1;
		}

		public void InverseTransformScaleOnly(ref double x, ref double y)
		{
			x /= ScaleX;
			y /= ScaleY;
		}

		public TransformAffine ToAffine()
		{
			var mtx = TransformAffine.AffineTranslation(-_wx1, -_wy1);
			mtx *= TransformAffine.AffineScaling(ScaleX, ScaleY);
			mtx *= TransformAffine.AffineTranslation(_dx1, _dy1);
			return mtx;
		}

		public TransformAffine ToAffineScaleOnly() => TransformAffine.AffineScaling(ScaleX, ScaleY);
	}
}