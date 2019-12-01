using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct LineInterpolatorAABase<TColor, TRendererBase>
		where TColor : unmanaged, IColor
		where TRendererBase : unmanaged, IRendererBase<TColor>
	{
		[StructLayout(LayoutKind.Sequential, Size = 65*sizeof(int))]
		private struct I32x65 { }
		[StructLayout(LayoutKind.Sequential, Size = 132)]
		private struct U8x132 { }

		public const int MaxHalfWidth = 64;
		public const int MaxHalfWidthMul2 = MaxHalfWidth << 1;

		private LineParameters* _lp;
		private int _length;

		public int X;
		public int Y;
		public int OldX;
		public int OldY;
		public int MaxExtent;
		private I32x65 _dist;
		private U8x132 _covers;
		public DDA2LineInterpolator Li;
		public RendererOutlineAA<TColor, TRendererBase>* Renderer;
		public int Step;

		internal void Init(ref RendererOutlineAA<TColor, TRendererBase> ren, LineParameters* lp)
		{
			Step = 0;
			Renderer = (RendererOutlineAA<TColor, TRendererBase>*) Unsafe.AsPointer(ref ren);
			Width = ren.SubPixelWidth;
			MaxExtent = (Width + LineAA.LineSubpixelMask) >> LineAA.LineSubpixelShift;

			_lp = lp;

			Li = new DDA2LineInterpolator(lp->IsVertical ? LineAA.LineDblHr(lp->X2 - lp->X1) : LineAA.LineDblHr(lp->Y2 - lp->Y1), lp->IsVertical ? Math.Abs(lp->Y2 - lp->Y1) : Math.Abs(lp->X2 - lp->X1) + 1);
			_length = (lp->IsVertical == (lp->Increment > 0)) ? -lp->Length : lp->Length;
			X = lp->X1 >> LineAA.LineSubpixelShift;
			Y = lp->Y1 >> LineAA.LineSubpixelShift;
			OldX = X;
			OldY = Y;
			Count = lp->IsVertical ? Math.Abs((lp->Y2 >> LineAA.LineSubpixelShift) - Y) : Math.Abs((lp->X2 >> LineAA.LineSubpixelShift) - X);

			var li = new DDA2LineInterpolator(0, lp->IsVertical ? (lp->Dy << LineAA.LineSubpixelShift) : (lp->Dx << LineAA.LineSubpixelShift), lp->Length);

			var stop = Width + LineAA.LineSubpixelScaleMul2;
			int i;
			for (i = 0; i < MaxHalfWidth; ++i)
			{
				Dist[i] = li.Y;
				if (Dist[i] >= stop) break;
				li.Inc();
			}
			Dist[i] = 0x7FFF0000;
		}

		public int* Dist => (int*) Unsafe.AsPointer(ref _dist);

		public byte* Covers => (byte*) Unsafe.AsPointer(ref _covers);

		public bool IsVertical => _lp->IsVertical;

		public int Width { get; private set; }

		public int Count { get; private set; }

		public int StepHorBase<TDistInt>(ref TDistInt di)
			where TDistInt : struct, IDistanceInterpolator
		{
			Li.Inc();
			X += _lp->Increment;
			Y = (_lp->Y1 + Li.Y) >> LineAA.LineSubpixelShift;

			if (_lp->Increment > 0)
			{
				di.IncX(Y - OldY);
			}
			else
			{
				di.DecX(Y - OldY);
			}

			OldY = Y;
			return di.Dist / _length;
		}

		public int StepVerBase<TDistInt>(ref TDistInt di)
			where TDistInt : struct, IDistanceInterpolator
		{
			Li.Inc();
			Y += _lp->Increment;
			X = (_lp->X1 + Li.Y) >> LineAA.LineSubpixelShift;

			if (_lp->Increment > 0)
			{
				di.IncY(X - OldX);
			}
			else
			{
				di.DecY(X - OldX);
			}

			OldX = X;
			return di.Dist / _length;
		}
	}
}