using System.ComponentModel;

namespace WindowsFormsApp1.NewFolder1
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