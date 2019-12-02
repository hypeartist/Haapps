using System;
using System.Runtime.InteropServices;
using Haapps.Utils.Uth.GacManager.Fusion;

namespace Haapps.Utils.Uth.GacManager
{
	/// <summary>
	///     The AssemblyCacheEnumerator is an object that can be used to enumerate all assemblies in the GAC.
	/// </summary>
	[ComVisible(false)]
	public class AssemblyCacheEnumerator
	{
		private IAssemblyEnum _assemblyEnumerator;
		private bool _done;

		public AssemblyCacheEnumerator()
		{
			Initialise(null);
		}

		public AssemblyCacheEnumerator(string assemblyName)
		{
			Initialise(assemblyName);
		}

		private void Initialise(string assemblyName)
		{
			IAssemblyName fusionName = null;
			int hr;

			//  If we have an assembly name, create the assembly name object.
			if (assemblyName != null)
			{
				hr = FusionImports.CreateAssemblyNameObject(out fusionName, assemblyName,
					CreateAsmNameObjFlags.CanofParseDisplayName, IntPtr.Zero);

				//  Check the result.
				if (hr < 0)
					Marshal.ThrowExceptionForHR(hr);
			}

			//  Create the assembly enumerator.
			hr = FusionImports.CreateAssemblyEnum(out _assemblyEnumerator, IntPtr.Zero,
				fusionName, AsmCacheFlags.AsmCacheGac, IntPtr.Zero);

			//  Check the result.
			if (hr < 0)
				Marshal.ThrowExceptionForHR(hr);
		}

		/// <summary>
		///     Gets the next assembly.
		/// </summary>
		/// <returns>The next assembly, or null of all assemblies have been enumerated.</returns>
		public IAssemblyName GetNextAssembly()
		{
			if (_done) return null;

			// Now get next IAssemblyName from m_AssemblyEnum
			var hr = _assemblyEnumerator.GetNextAssembly((IntPtr) 0, out var fusionName, 0);

			if (hr < 0) Marshal.ThrowExceptionForHR(hr);

			//  If we haven't got a fusion object, we're done.
			if (fusionName != null)
			{
				return fusionName;
			}
			_done = true;
			return null;

		}
	}
}