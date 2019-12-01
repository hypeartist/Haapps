using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct PixfmtAlphaMaskAdaptor<TColor, TPixfmt, TAlphaMask> : IPixfmt<TColor>, IPixfmtAlphaBlend<TColor>
		where TColor : unmanaged, IColor
		where TPixfmt : unmanaged, IPixfmt<TColor>
		where TAlphaMask : unmanaged, IAlphaMask
	{
		private readonly TPixfmt* _pixfmt;
		private readonly TAlphaMask* _alphaMask;

		public PixfmtAlphaMaskAdaptor(ref TPixfmt pixfmt, ref TAlphaMask alphaMask)
		{
			_pixfmt = (TPixfmt*) Unsafe.AsPointer(ref pixfmt);
			_alphaMask = (TAlphaMask*) Unsafe.AsPointer(ref alphaMask);
		}

		public readonly int BytesPerPixel
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pixfmt->BytesPerPixel;
		}

		public readonly int Width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pixfmt->Width;
		}

		public readonly int Height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pixfmt->Height;
		}

		public readonly int Stride
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pixfmt->Stride;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int y) => _pixfmt->GetRowPtr(y);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int x, int y, int length) => _pixfmt->GetRowPtr(x, y, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetPixPtr(int x, int y) => _pixfmt->GetPixPtr(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly RowInfo GetRowInfo(int y) => _pixfmt->GetRowInfo(y);

		// public readonly void Attach(TRenderingBuffer* buffer) => _pixfmt->Attach(buffer);

		// public readonly bool Attach<TPixfmtAlphaBlend>(TPixfmtAlphaBlend* pixfmt, int x1, int y1, int x2, int y2) 
			// where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor<TRenderingBuffer> => _pixfmt->Attach(pixfmt, x1, y1, x2, y2);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly TColor Pixel(int x, int y) => _pixfmt->Pixel(x, y);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyPixel(int x, int y, TColor color)
		{
			if (_pixfmt != null)
			{
				_pixfmt->CopyPixel(x, y, color);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendPixel(int x, int y, TColor color, byte cover)
		{
			if (_pixfmt != null)
			{
				_pixfmt->BlendPixel(x, y, color, _alphaMask->CombinePixel(x, y, cover));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyHLine(int x, int y, int length, TColor c)
		{
			var span = stackalloc byte[length];
			_alphaMask->FillHSpan(x, y, span, length);
			_pixfmt->BlendSolidHSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyVLine(int x, int y, int length, TColor c)
		{
			var span = stackalloc byte[length];
			_alphaMask->FillVSpan(x, y, span, length);
			_pixfmt->BlendSolidVSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendHLine(int x, int y, int length, TColor c, byte cover)
		{
			var span = stackalloc byte[length];
			Unsafe.InitBlock(span, Common.CoverFull, (uint) length);
			_alphaMask->CombineHSpan(x, y, span, length);
			_pixfmt->BlendSolidHSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendVLine(int x, int y, int length, TColor c, byte cover)
		{
			var span = stackalloc byte[length];
			Unsafe.InitBlock(span, Common.CoverFull, (uint) length);
			_alphaMask->CombineVSpan(x, y, span, length);
			_pixfmt->BlendSolidVSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidHSpan(int x, int y, int length, TColor c, byte* covers)
		{
			var span = stackalloc byte[length];
			Unsafe.CopyBlock(span, covers, (uint) length);
			_alphaMask->CombineHSpan(x, y, span, length);
			_pixfmt->BlendSolidHSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendSolidVSpan(int x, int y, int length, TColor c, byte* covers)
		{
			var span = stackalloc byte[length];
			Unsafe.CopyBlock(span, covers, (uint) length);
			_alphaMask->CombineVSpan(x, y, span, length);
			_pixfmt->BlendSolidVSpan(x, y, length, c, span);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorHSpan(int x, int y, int length, TColor* colors)
		{
			var span = stackalloc byte[length];
			_alphaMask->FillHSpan(x, y, span, length);
			_pixfmt->BlendColorHSpan(x, y, length, colors, span, Common.CoverFull);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void CopyColorVSpan(int x, int y, int length, TColor* colors)
		{
			var span = stackalloc byte[length];
			_alphaMask->FillVSpan(x, y, span, length);
			_pixfmt->BlendColorVSpan(x, y, length, colors, span, Common.CoverFull);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorHSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover)
		{
			var span = stackalloc byte[length];
			if (covers != null)
			{
				Unsafe.CopyBlock(span, covers, (uint) length);
				_alphaMask->CombineHSpan(x, y, span, length);
			}
			else
			{
				_alphaMask->FillHSpan(x, y, span, length);
			}
			_pixfmt->BlendColorHSpan(x, y, length, colors, span, cover);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly void BlendColorVSpan(int x, int y, int length, TColor* colors, byte* covers, byte cover)
		{
			var span = stackalloc byte[length];
			if (covers != null)
			{
				Unsafe.CopyBlock(span, covers, (uint) length);
				_alphaMask->CombineVSpan(x, y, span, length);
			}
			else
			{
				_alphaMask->FillVSpan(x, y, span, length);
			}
			_pixfmt->BlendColorVSpan(x, y, length, colors, span, cover);
		}

		public void BlendFromColor<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor color, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor => throw new System.NotImplementedException();

		public void BlendFromLUT<TPixfmtAlphaBlend>(TPixfmtAlphaBlend src, TColor* colorLUT, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlend : unmanaged, IPixelDataAccessor => throw new System.NotImplementedException();

		public void BlendFrom<TPixfmtAlphaBlendColor>(TPixfmtAlphaBlendColor src, int xdst, int ydst, int xsrc, int ysrc, int length, byte cover)
			where TPixfmtAlphaBlendColor : unmanaged, IPixfmt<TColor> => throw new System.NotImplementedException();
	}
}