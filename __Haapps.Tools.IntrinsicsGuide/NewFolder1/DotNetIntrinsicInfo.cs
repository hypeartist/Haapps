using System.Collections.Generic;
using System.Linq;

namespace Haapps.Tools.IntrinsicsGuide.NewFolder1
{
	internal sealed class DotNetIntrinsicInfo : IIntrinsicInfo
	{
		public DotNetIntrinsicInfo(string name, string className, string intelName, string retType, List<Parameter> parameters)
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

		public sealed class Parameter : IParameter
		{
			internal Parameter(string name, string type)
			{
				Name = name;
				Type = type;
			}

			public string Name { get; }

			public string Type { get; }

			public override string ToString() => $"{Type} {Name}";
		}
	}
}