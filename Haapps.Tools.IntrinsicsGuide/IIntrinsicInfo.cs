using System.Collections.Generic;

namespace Haapps.Tools.IntrinsicsGuide
{
	public interface IIntrinsicInfo
	{
		string Name { get; }
		string ReturnType { get; }
		IEnumerable<IIntrinsicIParameter> Parameters { get; }
	}
}