using System;
using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed class RasterizerOutlineData : IDisposable
	{
		private RefPodList<LineAAVertex> _srcVerices;

		public RasterizerOutlineData()
		{
			_srcVerices = new RefPodList<LineAAVertex>(64);
		}

		public ref RefPodList<LineAAVertex> SrcVertices
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref _srcVerices;
		}

		public void Dispose() => _srcVerices.Dispose();
	}
}