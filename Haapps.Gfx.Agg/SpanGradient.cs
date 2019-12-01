using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanGradient<TColor, TDataItem, TSpanGradientDataProvider, TSpanGradientApplier, TTransform, TSpanInterpolator, TGradientFunction> : ISpanGenerator<TColor>
		where TColor : unmanaged, IColor
		where TDataItem : unmanaged
		where TSpanGradientDataProvider : unmanaged, ISpanGradientDataProvider<TDataItem>
		where TSpanGradientApplier : unmanaged, ISpanGradientApplier<TColor, TDataItem, TSpanGradientDataProvider>
		where TTransform : unmanaged, ITransform
		where TSpanInterpolator : unmanaged, ISpanInterpolator<TTransform>
		where TGradientFunction : unmanaged, IGradientFunction
	{
		private const int DownscaleShift = 8 - Common.GradientSubpixelShift;
		
		private readonly TSpanGradientDataProvider* _dataProvider;
		private readonly TSpanInterpolator* _interpolator;
		private int _d1;
		private int _d2;

		public SpanGradient(ref TSpanInterpolator interpolator, ref TSpanGradientDataProvider dataProvider, double d1, double d2)
		{
			_interpolator = (TSpanInterpolator*) Unsafe.AsPointer(ref interpolator);
			_dataProvider = (TSpanGradientDataProvider*) Unsafe.AsPointer(ref dataProvider);
			_d1 = Common.RoundToI32(d1 * Common.GradientSubpixelScale);
			_d2 = Common.RoundToI32(d2 * Common.GradientSubpixelScale);
		}

		public void Init(double d1, double d2)
		{
			_d1 = Common.RoundToI32(d1 * Common.GradientSubpixelScale);
			_d2 = Common.RoundToI32(d2 * Common.GradientSubpixelScale);
		}

		public void Prepare()
		{
		}

		
		public void Generate(TColor* span, int x, int y, int length)
		{
			var dd = _d2 - _d1;
			if (dd < 1)
			{
				dd = 1;
			}

			TGradientFunction gradientFunction = default;
			TSpanGradientApplier gradientApplier = default;
			_interpolator->Begin(x + 0.5, y + 0.5, length);
			do
			{
				_interpolator->Coordinates(out x, out y);
				var d = gradientFunction.Execute(x >> DownscaleShift, y >> DownscaleShift, _d2);
				d = (d - _d1) * _dataProvider->Size / dd;
				if (d < 0)
				{
					d = 0;
				}

				if (d >= _dataProvider->Size)
				{
					d = _dataProvider->Size - 1;
				}
				gradientApplier.Apply(span, ref *_dataProvider, d);
				span++;
				_interpolator->Inc();
			} while (--length != 0);
		}
	}
}