using System.Collections.Generic;
using System.Linq;

namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Net : IIntrinsicInfo
		{
			internal Net(string name, string className, string intelName, string retType, List<Parameter> parameters)
			{
				Name = name;
				ClassName = className;
				IntelName = intelName;
				ReturnType = retType;
				Parameters = parameters;
			}

			public string Name { get; }

			public string ClassName { get; }

			public string ReturnType { get; }

			public IEnumerable<IParameter> Parameters { get; }

			public string IntelName { get; }

			public override string ToString() => $"{ReturnType} {Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}) // {IntelName}";
		}
	}
}