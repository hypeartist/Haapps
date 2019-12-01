using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg
{
	public unsafe struct Span
	{
		public int X;
		public int Length;
		public byte* Covers;
#if DEBUG
		public override string ToString() => $"X: {X}, Length: {Length}, Covers: {Covers->ToString("X8")}";
#endif
	}
}