using System;
using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct LineProfileAA
	{
		private readonly RefPodArray<byte>* _profile;
		private FixedSize256 _gamma;

		public LineProfileAA(double w, LineProfileAAData data) : this()
		{
			_profile = (RefPodArray<byte>*) Unsafe.AsPointer(ref data.Profile);
			var gp = (byte*)Unsafe.AsPointer(ref _gamma);
			for (var i = 0; i < Common.AAScale; i++)
			{
				gp[i] = (byte)i;
			}
			MinWidth = 1.0;
			SmootherWidth = 1.0;
			Width = w;
		}

		public void ApplyGammaFunction<TGammaFunction>(TGammaFunction value)
			where TGammaFunction : unmanaged, IGammaFunction
		{
			var gp = (byte*)(Unsafe.AsPointer(ref _gamma));
			for (var i = 0; i < Common.AAScale; i++)
			{
				var v = value.Execute((double)i / Common.AAMask) * Common.AAMask;
				gp[i] = (byte)Common.RoundToU32(v);
			}
		}

		public double Width
		{
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}

				if (value < SmootherWidth)
				{
					value += value;
				}
				else
				{
					value += SmootherWidth;
				}

				value *= 0.5;

				value -= SmootherWidth;
				var s = SmootherWidth;
				if (value < 0.0)
				{
					s += value;
					value = 0.0;
				}
				Set(value, s);
			}
		}

		public int SubpixelWidth { get; private set; }

		public double SmootherWidth { get; set; }

		public double MinWidth { get; set; }

		public byte Value(int dist)
		{
			return (*_profile)[dist + Common.SubpixelScaleMul2];
		}

		private byte* Profile(double w)
		{
			SubpixelWidth = Common.RoundToU32(w * Common.SubpixelScale);
			_profile->Reallocate(SubpixelWidth + Common.SubpixelScale * 6);
			return _profile->DataPtr;
		}

		private void Set(double centerWidth, double smootherWidth)
		{
			var baseVal = 1.0;
			if (Math.Abs(centerWidth) < double.Epsilon)
			{
				centerWidth = 1.0 / Common.SubpixelScale;
			}
			if (Math.Abs(smootherWidth) < double.Epsilon)
			{
				smootherWidth = 1.0 / Common.SubpixelScale;
			}

			var width = centerWidth + smootherWidth;
			if (width < MinWidth)
			{
				var k = width / MinWidth;
				baseVal *= k;
				centerWidth /= k;
				smootherWidth /= k;
			}

			var ch = Profile(centerWidth + smootherWidth);

			var subpixelCenterWidth = (int)(centerWidth * Common.SubpixelScale);
			var subpixelSmootherWidth = (int)(smootherWidth * Common.SubpixelScale);

			var chCenter = ch + Common.SubpixelScaleMul2;
			var chSmoother = chCenter + subpixelCenterWidth;

			var gp = (byte*)(Unsafe.AsPointer(ref _gamma));
			var val = gp[(int)(baseVal * Common.AAMask)];
			ch = chCenter;
			for (var i = 0; i < subpixelCenterWidth; i++)
			{
				*ch++ = val;
			}

			for (var i = 0; i < subpixelSmootherWidth; i++)
			{
				*chSmoother++ = gp[(int)((baseVal - baseVal * ((double)(i) / subpixelSmootherWidth)) * Common.AAMask)];
			}

			var smoother = _profile->Size - subpixelSmootherWidth - subpixelCenterWidth - Common.SubpixelScaleMul2;

			val = gp[0];
			for (var i = 0; i < smoother; i++)
			{
				*chSmoother++ = val;
			}

			ch = chCenter;
			for (var i = 0; i < Common.SubpixelScaleMul2; i++)
			{
				*--ch = *chCenter++;
			}
		}
	}
}