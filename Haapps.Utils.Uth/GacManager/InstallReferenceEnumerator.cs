using System;
using System.Runtime.InteropServices;
using Haapps.Utils.Uth.GacManager.Fusion;

namespace Haapps.Utils.Uth.GacManager
{
	public class InstallReferenceEnumerator
	{
		private readonly IInstallReferenceEnum _refEnum;

		public InstallReferenceEnumerator(string assemblyName)
		{
			var hr = FusionImports.CreateAssemblyNameObject(out var fusionName, assemblyName, CreateAsmNameObjFlags.CanofParseDisplayName, IntPtr.Zero);

			if (hr >= 0) hr = FusionImports.CreateInstallReferenceEnum(out _refEnum, fusionName, 0, IntPtr.Zero);

			if (hr < 0) Marshal.ThrowExceptionForHR(hr);
		}

		public InstallReferenceEnumerator(IAssemblyName assemblyName)
		{
			var hr = FusionImports.CreateInstallReferenceEnum(out _refEnum, assemblyName, 0, IntPtr.Zero);

			if (hr < 0) Marshal.ThrowExceptionForHR(hr);
		}

		public FusionInstallReference GetNextReference()
		{
			var hr = _refEnum.GetNextInstallReferenceItem(out var item, 0, IntPtr.Zero);
			if ((uint) hr == 0x80070103)
				// ERROR_NO_MORE_ITEMS
				return null;

			if (hr < 0) Marshal.ThrowExceptionForHR(hr);

			var instRef = new FusionInstallReference(Guid.Empty, string.Empty, string.Empty);

			hr = item.GetReference(out var refData, 0, IntPtr.Zero);
			if (hr < 0) Marshal.ThrowExceptionForHR(hr);

			Marshal.PtrToStructure(refData, instRef);
			return instRef;
		}
	}
}