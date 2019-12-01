namespace Haapps.Gfx.Agg
{
	public unsafe interface IRenderingBuffer : IPixelDataAccessor
	{
		byte* Data { get; }
		int StrideAbs { get; }
		void Attach(byte* data, int width, int height, int stride);
		void CopyFrom<TRenderingBuffer>(TRenderingBuffer src) 
			where TRenderingBuffer : unmanaged, IRenderingBuffer;
		void Clear(byte value);
	}
}