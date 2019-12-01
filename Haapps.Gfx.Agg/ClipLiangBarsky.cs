using System;
using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public static unsafe class ClipLiangBarsky
	{
		private const int X1Clipped = 4;
		private const int X2Clipped = 1;
		private const int Y1Clipped = 8;
		private const int Y2Clipped = 2;
		private const int XClipped = X1Clipped | X2Clipped;
		private const int YClipped = Y1Clipped | Y2Clipped;

		
		private static int _(int v) => (int) (1 ^ ((uint)(v - 1) >> 31));

		
		private static int _(double v) => (int) (1 ^ ((uint)(v - 1) >> 31));

		
		public static int ClippingFlags(int x, int y, ref Rectangle32 clipBox) => (_(x - clipBox.X2) | (_(y - clipBox.Y2) << 1) | (_(x - clipBox.X1) << 2) | (_(y - clipBox.Y1) << 3));

		
		public static int ClippingFlags(double x, double y, ref Rectangle64 clipBox) => (_(x - clipBox.X2) | (_(y - clipBox.Y2) << 1) | (_(x - clipBox.X1) << 2) | (_(y - clipBox.Y1) << 3));

		
		public static int ClippingFlagsX(int x, ref Rectangle32 clipBox) => (_(x - clipBox.X2) | (_(x - clipBox.X1) << 2));

		
		public static int ClippingFlagsX(double x, ref Rectangle64 clipBox) => (_(x - clipBox.X2) | (_(x - clipBox.X1) << 2));

		
		public static int ClippingFlagsY(int y, ref Rectangle32 clipBox) => ((_(y - clipBox.Y2) << 1) | (_(y - clipBox.Y1) << 3));

		
		public static int ClippingFlagsY(double y, ref Rectangle64 clipBox) => ((_(y - clipBox.Y2) << 1) | (_(y - clipBox.Y1) << 3));

		public static bool ClipMovePoint(int x1, int y1, int x2, int y2, ref Rectangle32 clipBox, ref int x, ref int y, int flags)
		{
			int bound;

			if ((flags & XClipped) != 0)
			{
				if (x1 == x2)
				{
					return false;
				}
				bound = ((flags & X1Clipped) != 0) ? clipBox.X1 : clipBox.X2;
				y = (int) ((double) (bound - x1)*(y2 - y1)/(x2 - x1) + y1);
				x = bound;
			}

			flags = (_(y - clipBox.Y2) << 1) | (_(y - clipBox.Y1) << 3);
			if ((flags & YClipped) == 0)
			{
				return true;
			}
			if (y1 == y2)
			{
				return false;
			}
			bound = ((flags & Y1Clipped) != 0) ? clipBox.Y1 : clipBox.Y2;
			x = (int) ((double) (bound - y1)*(x2 - x1)/(y2 - y1) + x1);
			y = bound;
			return true;
		}

		public static bool ClipMovePoint(double x1, double y1, double x2, double y2, ref Rectangle64 clipBox, ref double x, ref double y, int flags)
		{
			double bound;

			if ((flags & XClipped) != 0)
			{
				if (Math.Abs(x1 - x2) < double.Epsilon)
				{
					return false;
				}
				bound = ((flags & X1Clipped) != 0) ? clipBox.X1 : clipBox.X2;
				y = (bound - x1)*(y2 - y1)/(x2 - x1) + y1;
				x = bound;
			}

			flags = (_(y - clipBox.Y2) << 1) | (_(y - clipBox.Y1) << 3);
			if ((flags & YClipped) == 0)
			{
				return true;
			}
			if (Math.Abs(y1 - y2) < double.Epsilon)
			{
				return false;
			}
			bound = ((flags & Y1Clipped) != 0) ? clipBox.Y1 : clipBox.Y2;
			x = (bound - y1)*(x2 - x1)/(y2 - y1) + x1;
			y = bound;
			return true;
		}

		public static int Clip(double x1, double y1, double x2, double y2, ref Rectangle64 clipBox, double* x, double* y)
		{
			const double nearZero = 1e-30;

			var deltax = x2 - x1;
			var deltay = y2 - y1;
			double xin;
			double xout;
			double yin;
			double yout;
			double tin1;
			double tin2;
			var np = 0;

			if (Math.Abs(deltax) < double.Epsilon)
			{
				deltax = (x1 > clipBox.X1) ? -nearZero : nearZero;
			}

			if (Math.Abs(deltay) < double.Epsilon)
			{
				deltay = (y1 > clipBox.Y1) ? -nearZero : nearZero;
			}

			if (deltax > 0.0)
			{
				xin = clipBox.X1;
				xout = clipBox.X2;
			}
			else
			{
				xin = clipBox.X2;
				xout = clipBox.X1;
			}

			if (deltay > 0.0)
			{
				yin = clipBox.Y1;
				yout = clipBox.Y2;
			}
			else
			{
				yin = clipBox.Y2;
				yout = clipBox.Y1;
			}

			var tinx = (xin - x1)/deltax;
			var tiny = (yin - y1)/deltay;

			if (tinx < tiny)
			{
				tin1 = tinx;
				tin2 = tiny;
			}
			else
			{
				tin1 = tiny;
				tin2 = tinx;
			}

			if (!(tin1 <= 1.0))
			{
				return np;
			}
			if (0.0 < tin1)
			{
				*x++ = xin;
				*y++ = yin;
				++np;
			}

			if (!(tin2 <= 1.0))
			{
				return np;
			}
			var toutx = (xout - x1)/deltax;
			var touty = (yout - y1)/deltay;

			var tout1 = (toutx < touty) ? toutx : touty;

			if (!(tin2 > 0.0) && !(tout1 > 0.0))
			{
				return np;
			}
			if (tin2 <= tout1)
			{
				if (tin2 > 0.0)
				{
					if (tinx > tiny)
					{
						*x++ = xin;
						*y++ = y1 + tinx*deltay;
					}
					else
					{
						*x++ = x1 + tiny*deltax;
						*y++ = yin;
					}
					++np;
				}

				if (tout1 < 1.0)
				{
					if (toutx < touty)
					{
						*x = xout;
						*y = y1 + toutx*deltay;
					}
					else
					{
						*x = x1 + touty*deltax;
						*y = yout;
					}
				}
				else
				{
					*x = x2;
					*y = y2;
				}
				++np;
			}
			else
			{
				if (tinx > tiny)
				{
					*x = xin;
					*y = yout;
				}
				else
				{
					*x = xout;
					*y = yin;
				}
				++np;
			}
			return np;
		}

		public static int Clip(int x1, int y1, int x2, int y2, ref Rectangle32 clipBox, int* x, int* y)
		{
			const double nearZero = 1e-30;

			double deltax = x2 - x1;
			double deltay = y2 - y1;
			double xin;
			double xout;
			double yin;
			double yout;
			double tin1;
			double tin2;
			var np = 0;

			if (Math.Abs(deltax) < double.Epsilon)
			{
				deltax = (x1 > clipBox.X1) ? -nearZero : nearZero;
			}

			if (Math.Abs(deltay) < double.Epsilon)
			{
				deltay = (y1 > clipBox.Y1) ? -nearZero : nearZero;
			}

			if (deltax > 0.0)
			{
				xin = clipBox.X1;
				xout = clipBox.X2;
			}
			else
			{
				xin = clipBox.X2;
				xout = clipBox.X1;
			}

			if (deltay > 0.0)
			{
				yin = clipBox.Y1;
				yout = clipBox.Y2;
			}
			else
			{
				yin = clipBox.Y2;
				yout = clipBox.Y1;
			}

			var tinx = (xin - x1)/deltax;
			var tiny = (yin - y1)/deltay;

			if (tinx < tiny)
			{
				tin1 = tinx;
				tin2 = tiny;
			}
			else
			{
				tin1 = tiny;
				tin2 = tinx;
			}

			if (!(tin1 <= 1.0))
			{
				return np;
			}
			if (0.0 < tin1)
			{
				*x++ = (int) xin;
				*y++ = (int) yin;
				++np;
			}

			if (!(tin2 <= 1.0))
			{
				return np;
			}
			var toutx = (xout - x1)/deltax;
			var touty = (yout - y1)/deltay;

			var tout1 = (toutx < touty) ? toutx : touty;

			if (!(tin2 > 0.0) && !(tout1 > 0.0))
			{
				return np;
			}
			if (tin2 <= tout1)
			{
				if (tin2 > 0.0)
				{
					if (tinx > tiny)
					{
						*x++ = (int) xin;
						*y++ = (int) (y1 + tinx*deltay);
					}
					else
					{
						*x++ = (int) (x1 + tiny*deltax);
						*y++ = (int) yin;
					}
					++np;
				}

				if (tout1 < 1.0)
				{
					if (toutx < touty)
					{
						*x = (int) xout;
						*y = (int) (y1 + toutx*deltay);
					}
					else
					{
						*x = (int) (x1 + touty*deltax);
						*y = (int) yout;
					}
				}
				else
				{
					*x = x2;
					*y = y2;
				}
				++np;
			}
			else
			{
				if (tinx > tiny)
				{
					*x = (int) xin;
					*y = (int) yout;
				}
				else
				{
					*x = (int) xout;
					*y = (int) yin;
				}
				++np;
			}
			return np;
		}

		public static int ClipLineSegment(ref double x1, ref double y1, ref double x2, ref double y2, ref Rectangle64 clipBox)
		{
			var f1 = _(x1 - clipBox.X2) | (_(y1 - clipBox.Y2) << 1) | (_(x1 - clipBox.X1) << 2) | (_(y1 - clipBox.Y1) << 3);
			var f2 = _(x2 - clipBox.X2) | (_(y2 - clipBox.Y2) << 1) | (_(x2 - clipBox.X1) << 2) | (_(y2 - clipBox.Y1) << 3);
			var ret = 0;

			if ((f2 | f1) == 0)
			{
				// Fully visible
				return 0;
			}

			if ((f1 & XClipped) != 0 && (f1 & XClipped) == (f2 & XClipped))
			{
				// Fully clipped
				return 4;
			}

			if ((f1 & YClipped) != 0 && (f1 & YClipped) == (f2 & YClipped))
			{
				// Fully clipped
				return 4;
			}

			var tx1 = x1;
			var ty1 = y1;
			var tx2 = x2;
			var ty2 = y2;
			if (f1 != 0)
			{
				if (!ClipMovePoint(tx1, ty1, tx2, ty2, ref clipBox, ref x1, ref y1, f1))
				{
					return 4;
				}
				if (Math.Abs(x1 - x2) < double.Epsilon && Math.Abs(y1 - y2) < double.Epsilon)
				{
					return 4;
				}
				ret |= 1;
			}
			if (f2 == 0)
			{
				return ret;
			}
			if (!ClipMovePoint(tx1, ty1, tx2, ty2, ref clipBox, ref x2, ref y2, f2))
			{
				return 4;
			}
			if (Math.Abs(x1 - x2) < double.Epsilon && Math.Abs(y1 - y2) < double.Epsilon)
			{
				return 4;
			}
			ret |= 2;
			return ret;
		}

		public static int ClipLineSegment(ref int x1, ref int y1, ref int x2, ref int y2, ref Rectangle32 clipBox)
		{
			var f1 = _(x1 - clipBox.X2) | (_(y1 - clipBox.Y2) << 1) | (_(x1 - clipBox.X1) << 2) | (_(y1 - clipBox.Y1) << 3);
			var f2 = _(x2 - clipBox.X2) | (_(y2 - clipBox.Y2) << 1) | (_(x2 - clipBox.X1) << 2) | (_(y2 - clipBox.Y1) << 3);
			var ret = 0;

			if ((f2 | f1) == 0)
			{
				// Fully visible
				return 0;
			}

			if ((f1 & XClipped) != 0 && (f1 & XClipped) == (f2 & XClipped))
			{
				// Fully clipped
				return 4;
			}

			if ((f1 & YClipped) != 0 && (f1 & YClipped) == (f2 & YClipped))
			{
				// Fully clipped
				return 4;
			}

			var tx1 = x1;
			var ty1 = y1;
			var tx2 = x2;
			var ty2 = y2;
			if (f1 != 0)
			{
				if (!ClipMovePoint(tx1, ty1, tx2, ty2, ref clipBox, ref x1, ref y1, f1))
				{
					return 4;
				}
				if (x1 == x2 && y1 == y2)
				{
					return 4;
				}
				ret |= 1;
			}
			if (f2 == 0)
			{
				return ret;
			}
			if (!ClipMovePoint(tx1, ty1, tx2, ty2, ref clipBox, ref x2, ref y2, f2))
			{
				return 4;
			}
			if (x1 == x2 && y1 == y2)
			{
				return 4;
			}
			ret |= 2;
			return ret;
		}
	}
}