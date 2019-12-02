using System.Runtime.InteropServices;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[StructLayout(LayoutKind.Sequential)]
	public struct AssemblyInfo
	{
		public int cbAssemblyInfo; // size of this structure for future expansion
		public int assemblyFlags;
		public long assemblySizeInKB;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string currentAssemblyPath;

		public int cchBuf; // size of path buf.
	}
}