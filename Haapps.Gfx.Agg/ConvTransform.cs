using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg
{
	public sealed unsafe class ConvTransform<TVertexSrc, TTransform> : VertexSourceAbstract
		where TVertexSrc : VertexSourceAbstract
		where TTransform : unmanaged, ITransform
	{
		private readonly TVertexSrc _source;
		private TTransform* _transform;

		public ConvTransform(TVertexSrc vs, ref TTransform trans)
		{
			_source = vs;
			_transform = (TTransform*) Unsafe.AsPointer(ref trans);
		}

		public ref TTransform Transform => ref Unsafe.AsRef<TTransform>(_transform);

		public override void Rewind(int pathId = 0) => _source.Rewind(pathId);

		public override PathCommand Vertex(ref double x, ref double y)
		{
			var cmd = _source.Vertex(ref x, ref y);
			if (cmd.Vertex())
			{
				_transform->Transform(ref x, ref y);
			}

			return cmd;
		}

		public override void Dispose()
		{
		}
	}
}