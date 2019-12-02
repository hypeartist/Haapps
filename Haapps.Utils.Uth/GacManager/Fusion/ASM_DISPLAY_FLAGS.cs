using System;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[Flags]
	public enum AsmDisplayFlags
	{
		AsmDisplayfVersion = 0x01,
		AsmDisplayfCulture = 0x02,
		AsmDisplayfPublicKeyToken = 0x04,
		AsmDisplayfPublicKey = 0x08,
		AsmDisplayfCustom = 0x10,
		AsmDisplayfProcessorarchitecture = 0x20,
		AsmDisplayfLanguageid = 0x40,
		AsmDisplayfRetarget = 0x80,
		AsmDisplayfConfigMask = 0x100,
		AsmDisplayfMvid = 0x200,

		AsmDisplayfFull =
			AsmDisplayfVersion |
			AsmDisplayfCulture |
			AsmDisplayfPublicKeyToken |
			AsmDisplayfRetarget |
			AsmDisplayfProcessorarchitecture,

		All =
			AsmDisplayfVersion |
			AsmDisplayfCulture |
			AsmDisplayfPublicKeyToken |
			AsmDisplayfPublicKey |
			AsmDisplayfCustom |
			AsmDisplayfProcessorarchitecture |
			AsmDisplayfLanguageid |
			AsmDisplayfRetarget |
			AsmDisplayfConfigMask |
			AsmDisplayfConfigMask
	}
}