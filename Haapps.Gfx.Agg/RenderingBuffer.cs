using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct RenderingBuffer : IRenderingBuffer
	{
		private byte* _start;

		public RenderingBuffer(byte* data, int width, int height, int stride) : this() => Attach(data, width, height, stride);

		public byte* Data { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }
		
		public int Stride { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }
		
		public int StrideAbs { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public int BytesPerPixel { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public int Width { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }
		
		public int Height { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; private set; }

		public void Attach(byte* data, int width, int height, int stride)
		{
			Data = _start = data;
			Width = width;
			Height = height;
			Stride = stride;
			if (stride < 0)
			{
				_start = Data - (height - 1) * stride;
			}
			StrideAbs = (stride < 0 ? -stride : stride);
			BytesPerPixel = StrideAbs / Width;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int x, int y, int length) => _start + (uint)(y * Stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public byte* GetPixPtr(int x, int y) => _start + (uint)(y * Stride + x * BytesPerPixel);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public byte* GetPixPtr(int x, int y, int length) => _start + (uint)(y * Stride + x * BytesPerPixel);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly byte* GetRowPtr(int y) => _start + (uint)(y * Stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public readonly RowInfo GetRowInfo(int y) => new RowInfo(_start + y*Stride, 0, Width - 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void CopyFrom<TRenderingBuffer>(TRenderingBuffer src) 
			where TRenderingBuffer : unmanaged, IRenderingBuffer
		{
			var h = Height;
			if (src.Height < h)
			{
				h = src.Height;
			}

			var l = StrideAbs;
			if (src.StrideAbs < l)
			{
				l = src.StrideAbs;
			}

			l *= sizeof(byte);

			var w = Width;
			for (var y = 0; y < (uint)h; y++)
			{
				Unsafe.CopyBlock(GetRowPtr(0, y, w), src.Data, (uint) l);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Clear(byte value)
		{
			var w = Width;
			var h = (uint)Height;
			var stride = (uint)StrideAbs;
			for (var y = 0; y < h; y++)
			{
				var p = GetRowPtr(0, y, w);
				for (var x = 0; x < stride; x++)
				{
					*p++ = value;
				}
			}
		}
	}
}