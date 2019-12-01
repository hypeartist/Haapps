using System.ComponentModel;

namespace Haapps.Tools.IntrinsicsGuide.NewFolder1
{
	internal enum OperationType
	{
		Undefined,
		[Description("Integer")]
		Integer,
		[Description("Floating Point")]
		FloatingPoint,
		[Description("Mask")]
		Mask
	}
}