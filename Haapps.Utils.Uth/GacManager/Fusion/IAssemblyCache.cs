using System;
using System.Runtime.InteropServices;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
	public interface IAssemblyCache
	{
		[PreserveSig]
		int UninstallAssembly(
			int flags,
			[MarshalAs(UnmanagedType.LPWStr)] string assemblyName,
			FusionInstallReference refData,
			out IassemblycacheUninstallDisposition disposition);

		[PreserveSig]
		int QueryAssemblyInfo(
			int flags,
			[MarshalAs(UnmanagedType.LPWStr)] string assemblyName,
			ref AssemblyInfo assemblyInfo);

		[PreserveSig]
		int Reserved(
			int flags,
			IntPtr pvReserved,
			out object ppAsmItem,
			[MarshalAs(UnmanagedType.LPWStr)] string assemblyName);

		[PreserveSig]
		int Reserved(out object ppAsmScavenger);

		[PreserveSig]
		int InstallAssembly(
			int flags,
			[MarshalAs(UnmanagedType.LPWStr)] string assemblyFilePath,
			FusionInstallReference refData);
	}
}