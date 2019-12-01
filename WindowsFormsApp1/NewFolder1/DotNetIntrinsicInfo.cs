using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1.NewFolder1
{
	internal sealed class DotNetIntrinsicInfo : IIntrinsicInfo
	{
		public DotNetIntrinsicInfo(string name, string className, string instruction, string retType, IEnumerable<Parameter> parameters, bool is64Bit = false)
		{
			Name = name;
			ClassName = className;
			Instruction = instruction;
			ReturnType = retType;
			Parameters = parameters;
			Is64Bit = is64Bit;
		}

		public string Name { get; }

		public string ClassName { get; }

		public string ReturnType { get; }

		public IEnumerable<IParameter> Parameters { get; }

		public bool Is64Bit { get; }

		public string Instruction { get; }

		public override string ToString() => $"{ReturnType} {Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}) // {Instruction} ";

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