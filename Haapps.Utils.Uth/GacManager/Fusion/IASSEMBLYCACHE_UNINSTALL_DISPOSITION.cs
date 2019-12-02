namespace Haapps.Utils.Uth.GacManager.Fusion
{
	public enum IassemblycacheUninstallDisposition
	{
		Unknown = 0,

		/// <summary>
		///     The assembly files have been removed from the GAC.
		/// </summary>
		IassemblycacheUninstallDispositionUninstalled = 1,

		/// <summary>
		///     An application is using the assembly. This value is returned on Microsoft Windows 95 and Microsoft Windows 98.
		/// </summary>
		IassemblycacheUninstallDispositionStillInUse = 2,

		/// <summary>
		///     The assembly does not exist in the GAC.
		/// </summary>
		IassemblycacheUninstallDispositionAlreadyUninstalled = 3,

		/// <summary>
		///     Not used.
		/// </summary>
		IassemblycacheUninstallDispositionDeletePending = 4,

		/// <summary>
		///     The assembly has not been removed from the GAC because another application reference exists.
		/// </summary>
		IassemblycacheUninstallDispositionHasInstallReferences = 5,

		/// <summary>
		///     The reference that is specified in pRefData is not found in the GAC.
		/// </summary>
		IassemblycacheUninstallDispositionReferenceNotFound = 6
	}
}