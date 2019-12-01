using System;
using System.Runtime.CompilerServices;
using System.Text;
// ReSharper disable UnassignedReadonlyField
// ReSharper disable MemberCanBePrivate.Global

namespace Haapps.Utils.PodMemory
{
	public readonly unsafe struct PodStringAnsi
	{
		public readonly byte* Chars;
		public readonly int Length;

		public static PodStringAnsi FromMangedString(string str)
		{
			PodStringAnsi podStringStr = default;
			*(&(((PodStringAnsi*) Unsafe.AsPointer(ref podStringStr))->Chars)) = (byte*) PodHeap.AllocateRaw<byte>(str.Length + 1);
			*(&(((PodStringAnsi*) Unsafe.AsPointer(ref podStringStr))->Length)) = str.Length;
			Unsafe.CopyBlock(podStringStr.Chars, Unsafe.AsPointer(ref Encoding.ASCII.GetBytes(str).AsSpan()[0]), (uint) str.Length);
			podStringStr.Chars[str.Length] = 0;
			return podStringStr;
		}
	}
}