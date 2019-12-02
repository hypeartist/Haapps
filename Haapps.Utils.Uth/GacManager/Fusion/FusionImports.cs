﻿using System;
using System.Runtime.InteropServices;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	public static class FusionImports
	{
		internal const string FusionDll = "fusion.dll";

		[DllImport(FusionDll)]
		public static extern int CreateAssemblyEnum(
			out IAssemblyEnum ppEnum,
			IntPtr pUnkReserved,
			IAssemblyName pName,
			AsmCacheFlags flags,
			IntPtr pvReserved);

		[DllImport(FusionDll)]
		public static extern int CreateAssemblyNameObject(
			out IAssemblyName ppAssemblyNameObj,
			[MarshalAs(UnmanagedType.LPWStr)] string szAssemblyName,
			CreateAsmNameObjFlags flags,
			IntPtr pvReserved);

		[DllImport(FusionDll)]
		public static extern int CreateAssemblyCache(
			out IAssemblyCache ppAsmCache,
			int reserved);

		[DllImport(FusionDll)]
		public static extern int CreateInstallReferenceEnum(
			out IInstallReferenceEnum ppRefEnum,
			IAssemblyName pName,
			int dwFlags,
			IntPtr pvReserved);
	}
}