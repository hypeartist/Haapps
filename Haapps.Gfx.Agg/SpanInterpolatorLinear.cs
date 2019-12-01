using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public unsafe struct SpanInterpolatorLinear<TTransform> : ISpanInterpolator<TTransform>
		where TTransform : unmanaged, ITransform
	{
		private const int SubpixelShift = 8;
		private const int SubpixelScale = 1 << SubpixelShift;

		private readonly TTransform* _transform;
		private DDA2LineInterpolator _liX;
		private DDA2LineInterpolator _liY;

		public SpanInterpolatorLinear(ref TTransform transform) : this() => _transform = (TTransform*) Unsafe.AsPointer(ref transform);

		public SpanInterpolatorLinear(ref TTransform transform, double x, double y, int length):this()
		{
			_transform = (TTransform*) Unsafe.AsPointer(ref transform);
			Begin(x, y, length);
		}

		public ref TTransform Transform => ref *_transform;

		public void Begin(double x, double y, int length)
		{
			var tx = x;
			var ty = y;
			_transform->Transform(ref tx, ref ty);
			var x1 = Common.RoundToI32(tx * SubpixelScale);
			var y1 = Common.RoundToI32(ty * SubpixelScale);

			tx = x + length;
			ty = y;
			_transform->Transform(ref tx, ref ty);
			var x2 = Common.RoundToI32(tx * SubpixelScale);
			var y2 = Common.RoundToI32(ty * SubpixelScale);

			_liX = new DDA2LineInterpolator(x1, x2, length);
			_liY = new DDA2LineInterpolator(y1, y2, length);
		}

		public void Resynchronize(double xe, double ye, int length)
		{
			_transform->Transform(ref xe, ref ye);
			_liX = new DDA2LineInterpolator(_liX.Y, Common.RoundToI32(xe * SubpixelScale), length);
			_liY = new DDA2LineInterpolator(_liY.Y, Common.RoundToI32(ye * SubpixelScale), length);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Inc()
		{
			_liX.Inc();
			_liY.Inc();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Coordinates(out int x, out int y)
		{
			x = _liX.Y;
			y = _liY.Y;
		}
	}
}