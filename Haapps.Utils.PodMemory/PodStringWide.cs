using System;
using System.Runtime.CompilerServices;
// ReSharper disable UnassignedReadonlyField
// ReSharper disable MemberCanBePrivate.Global

namespace Haapps.Utils.PodMemory
{
	public readonly unsafe struct PodStringWide
	{
		public readonly char* Chars;
		public readonly int Length;

		public static PodStringWide FromMangedString(string str)
		{
			PodStringWide podStringStr = default;
			*(&(((PodStringWide*) Unsafe.AsPointer(ref podStringStr))->Chars)) = (char*) PodHeap.AllocateRaw<char>(str.Length + 1);
			*(&(((PodStringAnsi*) Unsafe.AsPointer(ref podStringStr))->Length)) = str.Length;
			Unsafe.CopyBlock(podStringStr.Chars, Unsafe.AsPointer(ref Unsafe.AsRef(in str.AsSpan()[0])), (uint) (str.Length * sizeof(char)));
			podStringStr.Chars[str.Length] = (char) 0;
			return podStringStr;
		}
	}
}