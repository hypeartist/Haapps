using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WindowsFormsApp1.NewFolder1
{
	internal sealed class IntelIntrinsicInfo : IIntrinsicInfo
	{
		public IntelIntrinsicInfo(XmlNode node, IReadOnlyDictionary<string, List<DotNetIntrinsicInfo>> dotNetIntrinsics)
		{
			Technology = (node.Attributes?.GetNamedItem("tech")?.Value ?? string.Empty).GetEnumValueByDescription<InstructionSet>();
			ReturnType = node.Attributes?.GetNamedItem("rettype")?.Value;
			Name = node.Attributes?.GetNamedItem("name")?.Value.ToLower();
			Types = node.SelectNodes(@"type")?.Cast<XmlNode>().Select(n => n.InnerText.GetEnumValueByDescription<OperationType>()).ToList();
			Cpuids = node.SelectNodes(@"CPUID")?.Cast<XmlNode>().Select(n => n.InnerText.GetEnumValueByDescription<Cpuid>()).ToList();
			Categories = node.SelectNodes(@"category")?.Cast<XmlNode>().Select(n => n.InnerText.GetEnumValueByDescription<InstructionCategory>()).ToList();
			Parameters = node.SelectNodes(@"parameter")?.Cast<XmlNode>().Select(n => new Parameter(n.Attributes?.GetNamedItem("varname")?.Value, n.Attributes?.GetNamedItem("type")?.Value, n.Attributes?.GetNamedItem("hint")?.Value)).ToList();
			Description = node.SelectNodes(@"description")?.Cast<XmlNode>().Select(n => n.InnerText).FirstOrDefault();
			Operations = node.SelectNodes(@"operation")?.Cast<XmlNode>().Select(n => new Operation(n.InnerText, n.Attributes?.GetNamedItem("validate")?.Value?.ToLower() != "false")).ToList();
			Instructions = node.SelectNodes(@"instruction")?.Cast<XmlNode>().Select(n => new Instruction(n.Attributes?.GetNamedItem("name")?.Value, n.Attributes?.GetNamedItem("form")?.Value, n.Attributes?.GetNamedItem("xed") != null)).ToList();
			Sequence = node.Attributes?.GetNamedItem("sequence")?.Value?.ToLower() == "true";
			VEX = node.Attributes?.GetNamedItem("vexEq")?.Value?.ToLower() == "true";
			Header = node.SelectNodes(@"header")?.Cast<XmlNode>().Select(n => n.InnerText).FirstOrDefault();
			DotNetIntrinsic = Instructions?.Where(i => dotNetIntrinsics.Keys.Contains(i.Name)).SelectMany(i => dotNetIntrinsics[i.Name]).ToList();
			Color = Technology.GetColorByValue();
		}

		public string Description { get; }

		public string Header { get; }

		public IEnumerable<OperationType> Types { get; }

		public IEnumerable<Cpuid> Cpuids { get; }

		public IEnumerable<InstructionCategory> Categories { get; }

		public IEnumerable<IParameter> Parameters { get; }

		public IEnumerable<Operation> Operations { get; }

		public IEnumerable<Instruction> Instructions { get; }

		public string InstructionAsText => string.Join(", ", Instructions.Select(i => i.ToString()));

		public InstructionSet Technology { get; }

		public string ReturnType { get; }

		public string Name { get; }

		public bool Sequence { get; }

		public bool VEX { get; }

		public IEnumerable<DotNetIntrinsicInfo> DotNetIntrinsic { get; }

		public Color Color { get; }

		internal sealed class Instruction
		{
			public Instruction(string name, string form, bool xed)
			{
				Name = name;
				Form = form;
				Xed = xed;
			}

			public string Name { get; }

			public string Form { get; }

			public bool Xed { get; }

			public override string ToString() => $"{Name} {Form}";
		}

		internal sealed class Operation
		{
			public Operation(string description, bool validate)
			{
				Description = description;
				Validate = validate;
			}

			public bool Validate { get; }

			public string Description { get; }
		}

		internal sealed class Parameter : IParameter
		{
			public Parameter(string name, string type, string hint)
			{
				Name = name;
				Type = type;
				Hint = hint;
			}

			public string Name { get; }

			public string Type { get; }

			public string Hint { get; }
		}

		public static async Task<IEnumerable<IntelIntrinsicInfo>> Collect()
		{
			IEnumerable<IntelIntrinsicInfo> data = new List<IntelIntrinsicInfo>();
			await Task.Run(() =>
			{
				var dotNetIntrinsics = CollectDotNetIntrinsics();
				var xml = new XmlDocument();
				xml.LoadXml(Properties.Resources.software_intel_com_IntrinsicsGuide_data_3_4_6);
				data = xml.SelectNodes(@"//intrinsic")?.Cast<XmlNode>().Select(n => new IntelIntrinsicInfo(n, dotNetIntrinsics)).ToList();
			});
			var t = data.FirstOrDefault(d => d.Name == "_mm_loadu_si128");
			return data;
		}

		private struct MatrixItem
		{
			public string FunctionName;
			public string Isa;
			public bool Is64bitOnly;
			public string I8;
			public string U8;
			public string I16;
			public string U16;
			public string I32;
			public string U32;
			public string I64;
			public string U64;
			public string F32;
			public string F64;

			public string I8M;
			public string U8M;
			public string I16M;
			public string U16M;
			public string I32M;
			public string U32M;
			public string I64M;
			public string U64M;
			public string F32M;
			public string F64M;

			public string Category;
		}

		private struct Void
		{
		}

		private static IReadOnlyDictionary<string, List<DotNetIntrinsicInfo>> CollectDotNetIntrinsics()
		{
			const string invalid = "invalid";
			var id2name = new Dictionary<string, string>();

			var tmp1 = Regex.Match(Properties.Resources.instrsxarch, @"INST\d+\((?<InstructionID>\w+)\s*,\s*""(?<InstructionName>\w+)""");
			while (tmp1.Success)
			{
				var id = tmp1.Groups["InstructionID"].Value;
				var name = tmp1.Groups["InstructionName"].Value;
				id2name[id] = name;
				tmp1 = tmp1.NextMatch();
			}

			id2name[invalid] = null;

			var dotNetIntrinsic2InstructionMatrix = new List<MatrixItem>();
			
			var tmp2 = Regex.Match(Properties.Resources.hwintrinsiclistxarch, @"HARDWARE_INTRINSIC\((?<IntrinsicID>\w+)\s*,\s*""(?<FunctionName>\w+)""\s*,\s*(?<Isa>[a-zA-Z\d]+)(?:_(?<Platform>X64))?\s*,\s*(?<Ival>-?\d+)\s*,\s*(?<SimdSize>\d+)\s*,\s*(?<NumArgs>-?\d+)\s*,\s*\{\s*INS_(?<TypByte>\w+)\s*,\s*INS_(?<TypUbyte>\w+)\s*,\s*INS_(?<TypShort>\w+)\s*,\s*INS_(?<TypUshort>\w+)\s*,\s*INS_(?<TypInt>\w+)\s*,\s*INS_(?<TypUint>\w+)\s*,\s*INS_(?<TypLong>\w+)\s*,\s*INS_(?<TypUlong>\w+)\s*,\s*INS_(?<TypFloat>\w+)\s*,\s*INS_(?<TypDouble>\w+)\s*}\s*,\s*(?<Category>\w+)\s*,\s*(?<Flags>[\w|]+)\)");
			while (tmp2.Success)
			{
				var mi = new MatrixItem
				{
					FunctionName = tmp2.Groups["FunctionName"].Value,
					Isa = tmp2.Groups["Isa"].Value,
					Is64bitOnly = tmp2.Groups["Platform"].Value == "X64",
					I8 = id2name[tmp2.Groups["TypByte"].Value],
					U8 = id2name[tmp2.Groups["TypUbyte"].Value],
					I16 = id2name[tmp2.Groups["TypShort"].Value],
					U16 = id2name[tmp2.Groups["TypUshort"].Value],
					I32 = id2name[tmp2.Groups["TypInt"].Value],
					U32 = id2name[tmp2.Groups["TypUint"].Value],
					I64 = id2name[tmp2.Groups["TypLong"].Value],
					U64 = id2name[tmp2.Groups["TypUlong"].Value],
					F32 = id2name[tmp2.Groups["TypFloat"].Value],
					F64 = id2name[tmp2.Groups["TypDouble"].Value],
					Category = tmp2.Groups["Category"].Value
				};
				dotNetIntrinsic2InstructionMatrix.Add(mi);
				tmp2 = tmp2.NextMatch();
			}

			var cats = dotNetIntrinsic2InstructionMatrix.Select(d => d.Category).Distinct().ToList();

			var instruction2DotNetIntrinsic = new Dictionary<string, List<DotNetIntrinsicInfo>>();
			var sdf = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);

			bool CheckTypeArg<T>(IParameterSymbol parameter)
			{
				var type = parameter.Type;
				switch (type.Kind)
				{
					case SymbolKind.NamedType:
						var namedTypeSymbol = (INamedTypeSymbol) type;
						return (namedTypeSymbol.IsGenericType && namedTypeSymbol.TypeArguments.Any(p => p.ToDisplayString(sdf) == typeof(T).FullName)) || namedTypeSymbol.ToDisplayString(sdf) == typeof(T).FullName;
					case SymbolKind.PointerType:
						var pointerTypeSymbol = (IPointerTypeSymbol) type;
						return pointerTypeSymbol.PointedAtType.ToDisplayString(sdf) == typeof(T).FullName;
				}
				;
				return false;
			}

			int LocateMatrixItem(MethodDeclarationSyntax m, IMethodSymbol ms, out bool is64Bit, out string className)
			{
				className = ms.ContainingType.Name;
				is64Bit = className == "X64";
				if (is64Bit)
				{
					className = ms.ContainingType.ContainingType?.Name;
				}

				if (string.IsNullOrEmpty(className))
				{
					return -1;
				}
				for (var i = 0; i < dotNetIntrinsic2InstructionMatrix.Count; i++)
				{
					var mapItem = dotNetIntrinsic2InstructionMatrix[i];
					if (mapItem.FunctionName == m.Identifier.ToString() && mapItem.Isa == className.ToUpper() && mapItem.Is64bitOnly == is64Bit)
					{
						return i;
					}
				}

				return -1;
			}

			bool CheckAndAddToFinalMap<T>(SemanticModel model, in MatrixItem mapItem, IParameterSymbol parameter, MethodDeclarationSyntax m, bool is64Bit, string className)
				where T:struct
			{
				var methodName = m.Identifier.ToString();
				var instruction = default(T) switch
				{
					Void _ => methodName switch
					{
						"LoadFence" => "lfence",
						"StoreFence" => "sfence",
						"MemoryFence" => "mfence",
						_ => null
					},
					byte _ => mapItem.U8,
					sbyte _ => mapItem.I8,
					ushort _ => mapItem.U16,
					short _ => mapItem.I16,
					uint _ => mapItem.U32,
					int _ => mapItem.I32,
					ulong _ => mapItem.U64,
					long _ => mapItem.I64,
					float _ => mapItem.F32,
					double _ => mapItem.F64,
					_ => null
				};
				if (string.IsNullOrEmpty(instruction) || typeof(T) != typeof(Void) && !CheckTypeArg<T>(parameter))
				{
					return false;
				}
				if (!instruction2DotNetIntrinsic.ContainsKey(instruction))
				{
					instruction2DotNetIntrinsic[instruction] = new List<DotNetIntrinsicInfo>();
				}
				var dnii = new DotNetIntrinsicInfo(m.Identifier.ToString(), className, instruction, model.GetSymbolInfo(m.ReturnType).Symbol.ToDisplayString() ?? "void", m.ParameterList.Parameters.Select(p=>new DotNetIntrinsicInfo.Parameter(p.Identifier.ToString(), model.GetSymbolInfo(p.Type).Symbol.ToDisplayString())).ToList(), is64Bit);
				instruction2DotNetIntrinsic[instruction].Add(dnii);
				return true;
			}

			using var stream = new MemoryStream(Properties.Resources.coreclr_src_System_Private_CoreLib_shared_System_Runtime_Intrinsics_X86);
			using var zip = new ZipArchive(stream);
			foreach (var entry in zip.Entries)
			{
				using var ze = entry.Open();
				using var tr = new StreamReader(ze);

				var tree = CSharpSyntaxTree.ParseText(tr.ReadToEnd());
				var compilation = CSharpCompilation.Create("Test")
					.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
					.AddReferences(MetadataReference.CreateFromFile(typeof(Vector128).Assembly.Location))
					.AddSyntaxTrees(tree);
				var model = compilation.GetSemanticModel(tree);
				
				var syntaxTreeRoot = (CompilationUnitSyntax)tree.GetRoot();
				var syntaxNodes = syntaxTreeRoot.DescendantNodes().SelectMany(n=>n.DescendantNodes());
				foreach (var methodDeclarationSyntax in syntaxNodes.OfType<MethodDeclarationSyntax>())
				{
					var methodSymbol = model.GetDeclaredSymbol(methodDeclarationSyntax);
					var itemIndex = LocateMatrixItem(methodDeclarationSyntax, methodSymbol, out var is64Bit, out var className);
					if(itemIndex < 0) continue;
					var matrixItem = dotNetIntrinsic2InstructionMatrix[itemIndex];
					var parameter = methodDeclarationSyntax.ParameterList.Parameters.Select(p => model.GetDeclaredSymbol(p)).FirstOrDefault();
					if(CheckAndAddToFinalMap<Void>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<byte>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<sbyte>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<ushort>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<short>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<uint>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<int>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<ulong>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<long>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<float>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
					if(CheckAndAddToFinalMap<double>(model, in matrixItem, parameter, methodDeclarationSyntax, is64Bit, className)) continue;
				}
			}

			return instruction2DotNetIntrinsic;
		}
	}
}
