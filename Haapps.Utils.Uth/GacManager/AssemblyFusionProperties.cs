using System.Collections.Generic;
using System.Runtime.InteropServices;
using Haapps.Utils.Uth.GacManager.Fusion;

namespace Haapps.Utils.Uth.GacManager
{
	/// <summary>
	///     AssemblyFusionProperties represent the properties of an assembly that
	///     are loaded by the Fusion API.
	/// </summary>
	public class AssemblyFusionProperties
	{
		private const int BufferLength = 65535;

		public AssemblyFusionProperties()
		{
			InstallReferences = new List<FusionInstallReference>();
		}

		/// <summary>
		///     Gets or sets the major version.
		/// </summary>
		/// <value>
		///     The major version.
		/// </value>
		public ushort MajorVersion { get; set; }

		/// <summary>
		///     Gets or sets the minor version.
		/// </summary>
		/// <value>
		///     The minor version.
		/// </value>
		public ushort MinorVersion { get; set; }

		/// <summary>
		///     Gets or sets the build number.
		/// </summary>
		/// <value>
		///     The build number.
		/// </value>
		public ushort BuildNumber { get; set; }

		/// <summary>
		///     Gets or sets the revision number.
		/// </summary>
		/// <value>
		///     The revision number.
		/// </value>
		public ushort RevisionNumber { get; set; }

		/// <summary>
		///     Gets or sets the public key.
		/// </summary>
		/// <value>
		///     The public key.
		/// </value>
		public byte[] PublicKey { get; set; }

		/// <summary>
		///     Gets or sets the hash value. This is currently reserved.
		/// </summary>
		/// <value>
		///     The hash value.
		/// </value>
		public byte[] ReservedHashValue { get; set; }

		/// <summary>
		///     Gets or sets the reserved hash algorithm id.
		/// </summary>
		/// <value>
		///     The reserved hash algorithm id.
		/// </value>
		public uint ReservedHashAlgorithmId { get; set; }

		/// <summary>
		///     Gets the install references.
		/// </summary>
		public List<FusionInstallReference> InstallReferences { get; }

		/// <summary>
		///     Loads the fusion properties given the assembly name COM object.
		/// </summary>
		/// <param name="assemblyName">Name of the assembly.</param>
		public void Load(IAssemblyName assemblyName)
		{
			//  Load the properties.
			MajorVersion = GetShortProperty(assemblyName, AsmName.AsmNameMajorVersion);
			MinorVersion = GetShortProperty(assemblyName, AsmName.AsmNameMinorVersion);
			BuildNumber = GetShortProperty(assemblyName, AsmName.AsmNameBuildNumber);
			RevisionNumber = GetShortProperty(assemblyName, AsmName.AsmNameRevisionNumber);
			PublicKey = GetByteArrayProperty(assemblyName, AsmName.AsmNamePublicKey);

			//  Create an install reference enumerator.
			var enumerator = new InstallReferenceEnumerator(assemblyName);
			var reference = enumerator.GetNextReference();
			while (reference != null)
			{
				InstallReferences.Add(reference);
				reference = enumerator.GetNextReference();
			}

			//  Load the reserved properties.
			//ReservedHashValue = GetByteArrayProperty(assemblyName, ASM_NAME.ASM_NAME_HASH_VALUE);
			//ReservedHashAlgorithmId = GetDwordProperty(assemblyName, ASM_NAME.ASM_NAME_HASH_ALGID);
		}

		internal static ushort GetShortProperty(IAssemblyName name, AsmName propertyName)
		{
			uint bufferSize = 512;
			var buffer = Marshal.AllocHGlobal((int) bufferSize);
			name.GetProperty(propertyName, buffer, ref bufferSize);
			var low = Marshal.ReadByte(buffer);
			var high = Marshal.ReadByte(buffer, 1);
			Marshal.FreeHGlobal(buffer);
			return (ushort) (low + (high << 8));
		}

		internal static uint GetDwordProperty(IAssemblyName name, AsmName propertyName)
		{
			uint bufferSize = 512;
			var buffer = Marshal.AllocHGlobal((int) bufferSize);
			name.GetProperty(propertyName, buffer, ref bufferSize);
			var a = Marshal.ReadByte(buffer);
			var b = Marshal.ReadByte(buffer, 1);
			var c = Marshal.ReadByte(buffer);
			var d = Marshal.ReadByte(buffer, 1);
			Marshal.FreeHGlobal(buffer);
			return (uint) (a + (b << 8) + (c << 16) + (d << 24));
		}

		internal static string GetStringProperty(IAssemblyName name, AsmName propertyName)
		{
			uint bufferSize = BufferLength;
			var buffer = Marshal.AllocHGlobal((int) bufferSize);
			name.GetProperty(propertyName, buffer, ref bufferSize);
			var stringVaule = Marshal.PtrToStringUni(buffer, (int) bufferSize);
			Marshal.FreeHGlobal(buffer);
			return stringVaule;
		}

		internal static byte[] GetByteArrayProperty(IAssemblyName name, AsmName propertyName)
		{
			uint bufferSize = 512;
			var buffer = Marshal.AllocHGlobal((int) bufferSize);
			name.GetProperty(propertyName, buffer, ref bufferSize);
			var result = new byte[bufferSize];
			for (var i = 0; i < bufferSize; i++)
				result[i] = Marshal.ReadByte(buffer, i);
			Marshal.FreeHGlobal(buffer);
			return result;
		}
	}
}