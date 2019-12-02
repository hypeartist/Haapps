using System;

namespace Haapps.Utils.Uth.GacManager.Fusion
{
	[Flags]
	public enum AsmCacheFlags
	{
		AsmCacheZap = 0x01,
		AsmCacheGac = 0x02,
		AsmCacheDownload = 0x04,
		AsmCacheRoot = 0x08,
		AsmCacheRootEx = 0x80
	}
}