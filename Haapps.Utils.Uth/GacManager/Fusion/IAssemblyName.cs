using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
	public interface IAssemblyName
	{
		[PreserveSig]
		int SetProperty(
			AsmName propertyId,
			IntPtr pvProperty,
			int cbProperty);

		[PreserveSig]
		int GetProperty(AsmName propertyId, IntPtr pvProperty, ref uint pcbProperty);

		[PreserveSig]
		int Finalize();

		[PreserveSig]
		int GetDisplayName(
			StringBuilder pDisplayName,
			ref int pccDisplayName,
			AsmDisplayFlags displayFlags);

		[PreserveSig]
		int Reserved(ref Guid guid,
			object obj1,
			object obj2,
			string string1,
			long llFlags,
			IntPtr pvReserved,
			int cbReserved,
			out IntPtr ppv);

		[PreserveSig]
		int GetName(
			ref int pccBuffer,
			StringBuilder pwzName);

		[PreserveSig]
		int GetVersion(
			out int versionHi,
			out int versionLow);

		[PreserveSig]
		int IsEqual(
			IAssemblyName pAsmName,
			int cmpFlags);

		[PreserveSig]
		int Clone(out IAssemblyName pAsmName);
	}
}