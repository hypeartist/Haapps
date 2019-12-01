using System;

namespace Haapps.Gfx.Agg
{
	public static class GammaFunctions
	{
		public struct Linear : IGammaFunction
		{
			public double Start;
			public double End;

			public static Linear Default => new Linear
			{
				End = 1.0
			};

			public Linear(double s, double e)
			{
				Start = s;
				End = e;
			}

			public readonly double Execute(double x)
			{
				if (x < Start)
				{
					return 0.0;
				}

				if (x > End)
				{
					return 1.0;
				}

				return (x - Start) / (End - Start);
			}
		}

		public struct Multiply : IGammaFunction
		{
			public double Value;

			public static Multiply Default => new Multiply
			{
				Value = 0.5
			};

			public Multiply(double x) => Value = x;

			public readonly double Execute(double x)
			{
				var y = x * Value;
				if (y > 1.0)
				{
					y = 1.0;
				}

				return y;
			}
		}

		public struct None : IGammaFunction
		{
			public double Execute(double x) => x;
		}

		public struct Power : IGammaFunction
		{
			public double Value;

			public static Power Default => new Power
			{
				Value = 1.0
			};

			public Power(double x) => Value = x;

			public readonly double Execute(double x) => Math.Pow(x, Value);
		}

		public struct Threshold : IGammaFunction
		{
			public double Value;

			public static Threshold Default => new Threshold
			{
				Value = 0.5
			};

			public Threshold(double x) => Value = x;

			public readonly double Execute(double x) => x < Value ? 0.0 : 1.0;
		}
	}
}