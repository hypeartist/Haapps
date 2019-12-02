using System;
using System.Runtime.InteropServices;
using Haapps.Utils.Uth.GacManager.Fusion;

namespace Haapps.Utils.Uth.GacManager
{
	/// <summary>
	///     AssemblyMustBeStronglyNamedException.
	/// </summary>
	public class AssemblyMustBeStronglyNamedException : Exception
	{
	}

	/// <summary>
	///     The AssemblyCache class is a managed wrapper around the Fusion IAssemblyCache COM interface.
	/// </summary>
	[ComVisible(false)]
	public static class AssemblyCache
	{
		public static void InstallAssembly(string assemblyPath, FusionInstallReference reference, AssemblyCommitFlags flags)
		{
			if (reference != null)
			{
				if (!InstallReferenceGuid.IsValidInstallGuidScheme(reference.GuidScheme))
					throw new ArgumentException("Invalid reference guid.", "guid");
			}

			var hr = FusionImports.CreateAssemblyCache(out var ac, 0);
			if (hr >= 0)
			{
				hr = ac.InstallAssembly((int) flags, assemblyPath, reference);
			}

			if (hr >= 0) return;
			if (hr == -2146234300 /*0x80131044*/)
				throw new AssemblyMustBeStronglyNamedException();
			Marshal.ThrowExceptionForHR(hr);
		}

		// assemblyName has to be fully specified name. 
		// A.k.a, for v1.0/v1.1 assemblies, it should be "name, Version=xx, Culture=xx, PublicKeyToken=xx".
		// For v2.0 assemblies, it should be "name, Version=xx, Culture=xx, PublicKeyToken=xx, ProcessorArchitecture=xx".
		// If assemblyName is not fully specified, a random matching assembly will be uninstalled. 
		public static void UninstallAssembly(string assemblyName, FusionInstallReference reference, out IassemblycacheUninstallDisposition disp)
		{
			if (reference != null)
			{
				if (!InstallReferenceGuid.IsValidUninstallGuidScheme(reference.GuidScheme))
					throw new ArgumentException("Invalid reference guid.", "guid");
			}

			var hr = FusionImports.CreateAssemblyCache(out var ac, 0);
			if (hr < 0)
				Marshal.ThrowExceptionForHR(hr);

			//  Uninstall the assembly.
			hr = ac.UninstallAssembly(0, assemblyName, reference, out disp);
			if (hr < 0)
				Marshal.ThrowExceptionForHR(hr);
		}

		// See comments in UninstallAssembly
		public static string QueryAssemblyInfo(string assemblyName)
		{
			if (assemblyName == null)
			{
				throw new ArgumentException("Invalid name", "assemblyName");
			}

			var assemblyInfo = new AssemblyInfo {cchBuf = 1024};

			// Get a string with the desired length
			assemblyInfo.currentAssemblyPath = new string('\0', assemblyInfo.cchBuf);

			var hr = FusionImports.CreateAssemblyCache(out var ac, 0);
			if (hr >= 0)
			{
				hr = ac.QueryAssemblyInfo(0, assemblyName, ref assemblyInfo);
			}

			if (hr < 0)
			{
				Marshal.ThrowExceptionForHR(hr);
			}

			return assemblyInfo.currentAssemblyPath;
		}
	}
}