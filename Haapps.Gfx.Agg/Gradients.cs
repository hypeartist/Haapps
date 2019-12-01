using System;
using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public static class Gradients
	{
		public struct Circle : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Common.FastSqrt(x * x + y * y);
		}

		public struct Radial : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Common.FastSqrt(x * x + y * y);
		}

		public struct RadialD : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Common.RoundToU32(Math.Sqrt(x * (double) x + y * (double) y));
		}

		public struct X : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => x;
		}

		public struct Y : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => y;
		}

		public struct Diamond : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0)
			{
				var ax = Math.Abs(x);
				var ay = Math.Abs(y);
				return ax > ay ? ax : ay;
			}
		}

		public struct XY : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Math.Abs(x) * Math.Abs(y) / d;
		}

		public struct SqrtXY : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Common.FastSqrt(Math.Abs(x) * Math.Abs(y));
		}

		public struct Conic : IGradientFunction
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public int Execute(int x, int y, int d = 0) => Common.RoundToU32(Math.Abs(Math.Atan2(y, x)) * d / Common.Pi);
		}

		public struct RadialFocus : IGradientFunction
		{
			private const int GradientSubpixelScale = 16;

			private int _r;
			private int _fx;
			private int _fy;
			private double _r2;
			private double _fx2;
			private double _fy2;
			private double _mul;

			public RadialFocus(double r, double fx, double fy) : this()
			{
				_r = Common.RoundToI32(r * GradientSubpixelScale);
				_fx = Common.RoundToI32(fx * GradientSubpixelScale);
				_fy = Common.RoundToI32(fy * GradientSubpixelScale);
				UpdateValues();
			}

			public readonly int Radius => _r / GradientSubpixelScale;

			public readonly int FocusX => _fx / GradientSubpixelScale;

			public readonly int FocusY => _fy / GradientSubpixelScale;

			public void Init(double r, double fx, double fy)
			{
				_r = Common.RoundToI32(r * GradientSubpixelScale);
				_fx = Common.RoundToI32(fx * GradientSubpixelScale);
				_fy = Common.RoundToI32(fy * GradientSubpixelScale);
				UpdateValues();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			public readonly int Execute(int x, int y, int d = 0)
			{
				double dx = x - _fx;
				double dy = y - _fy;
				var d2 = dx * _fy - dy * _fx;
				var d3 = _r2 * (dx * dx + dy * dy) - d2 * d2;
				return Common.RoundToI32((dx * _fx + dy * _fy + Math.Sqrt(Math.Abs(d3))) * _mul);
			}

			private void UpdateValues()
			{
				_r2 = _r * _r;
				_fx2 = _fx * _fx;
				_fy2 = _fy * _fy;
				var d = _r2 - (_fx2 + _fy2);
				if (Math.Abs(d) < double.Epsilon)
				{
					if (_fx != 0)
					{
						if (_fx < 0)
						{
							++_fx;
						}
						else
						{
							--_fx;
						}
					}

					if (_fy != 0)
					{
						if (_fy < 0)
						{
							++_fy;
						}
						else
						{
							--_fy;
						}
					}

					_fx2 = _fx * _fx;
					_fy2 = _fy * _fy;
					d = _r2 - (_fx2 + _fy2);
				}

				_mul = _r / d;
			}
		}
	}
}