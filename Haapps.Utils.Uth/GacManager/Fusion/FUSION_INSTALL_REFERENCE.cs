using System;
using System.Runtime.InteropServices;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[StructLayout(LayoutKind.Sequential)]
	public class FusionInstallReference
	{
		private int cbSize;

		private readonly int flags;

		public FusionInstallReference(Guid guid, string id, string data)
		{
			cbSize = 2 * IntPtr.Size + 16 + (id.Length + data.Length) * 2;
			flags = 0;
			// quiet compiler warning 
			if (flags == 0)
			{
			}

			GuidScheme = guid;
			Identifier = id;
			Description = data;
		}

		public Guid GuidScheme { get; }

		[field: MarshalAs(UnmanagedType.LPWStr)]
		public string Identifier { get; }

		[field: MarshalAs(UnmanagedType.LPWStr)]
		public string Description { get; }
	}
}