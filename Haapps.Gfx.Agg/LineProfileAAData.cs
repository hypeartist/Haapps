using System;
using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public sealed class LineProfileAAData : IDisposable
	{
		private RefPodArray<byte> _profile;

		public ref RefPodArray<byte> Profile
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref _profile;
		}

		public void Dispose() => _profile.Dispose();
	}
}