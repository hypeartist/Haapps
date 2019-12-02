namespace Haapps.Utils.Uth.GacManager.Fusion
{
	/// <summary>
	///     The values of the ASM_NAME enumeration are the property IDs for the name-value pairs included in a side-by-side
	///     assembly name.
	/// </summary>
	public enum AsmName
	{
		/// <summary>
		///     Property ID for the assembly's public key. The value is a byte array.
		/// </summary>
		AsmNamePublicKey,

		/// <summary>
		///     Property ID for the assembly's public key token. The value is a byte array.
		/// </summary>
		AsmNamePublicKeyToken,

		/// <summary>
		///     Property ID for a reserved name-value pair. The value is a byte array.
		/// </summary>
		AsmNameHashValue,

		/// <summary>
		///     Property ID for the assembly's simple name. The value is a string value.
		/// </summary>
		AsmNameName,

		/// <summary>
		///     Property ID for the assembly's major version. The value is a WORD value.
		/// </summary>
		AsmNameMajorVersion,

		/// <summary>
		///     Property ID for the assembly's minor version. The value is a WORD value.
		/// </summary>
		AsmNameMinorVersion,

		/// <summary>
		///     Property ID for the assembly's build version. The value is a WORD value.
		/// </summary>
		AsmNameBuildNumber,

		/// <summary>
		///     Property ID for the assembly's revision version. The value is a WORD value.
		/// </summary>
		AsmNameRevisionNumber,

		/// <summary>
		///     Property ID for the assembly's culture. The value is a string value.
		/// </summary>
		AsmNameCulture,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameProcessorIdArray,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameOsinfoArray,

		/// <summary>
		///     Property ID for a reserved name-value pair. The value is a DWORD value.
		/// </summary>
		AsmNameHashAlgid,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameAlias,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameCodebaseUrl,

		/// <summary>
		///     Property ID for a reserved name-value pair. The value is a FILETIME structure.
		/// </summary>
		AsmNameCodebaseLastmod,

		/// <summary>
		///     Property ID for the assembly as a simply named assembly that does not have a public key.
		/// </summary>
		AsmNameNullPublicKey,

		/// <summary>
		///     Property ID for the assembly as a simply named assembly that does not have a public key token.
		/// </summary>
		AsmNameNullPublicKeyToken,

		/// <summary>
		///     Property ID for a reserved name-value pair. The value is a string value.
		/// </summary>
		AsmNameCustom,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameNullCustom,

		/// <summary>
		///     Property ID for a reserved name-value pair.
		/// </summary>
		AsmNameMvid,

		/// <summary>
		///     Reserved.
		/// </summary>
		AsmNameMaxParams
	}
}