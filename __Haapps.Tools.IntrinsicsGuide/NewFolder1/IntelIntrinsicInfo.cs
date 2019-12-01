using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Haapps.Tools.IntrinsicsGuide.NewFolder1
{
	internal sealed class IntelIntrinsicInfo : IIntrinsicInfo
	{
		public IntelIntrinsicInfo(XmlNode node, IEnumerable<DotNetIntrinsicInfo> netIntrinsics)
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
			DotNetIntrinsic = netIntrinsics.FirstOrDefault(n => n.IntelName == Name);
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

		public DotNetIntrinsicInfo DotNetIntrinsic { get; }

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
				var netIntrinsics = CollectDotNetIntrinsics();
				var xml = new XmlDocument();
				xml.LoadXml(Properties.Resources.software_intel_com_IntrinsicsGuide_data_3_4_6);
				data = xml.SelectNodes(@"//intrinsic")?.Cast<XmlNode>().Select(n => new IntelIntrinsicInfo(n, netIntrinsics)).ToList();
			});
			return data;
		}

		private struct MapItem
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
		}

		private static IEnumerable<DotNetIntrinsicInfo> CollectDotNetIntrinsics()
		{
			var netIntrinsics = new List<DotNetIntrinsicInfo>();

			const string invalid = "invalid";
			var map1 = new Dictionary<string, string>();

			var tmp1 = Regex.Match(Properties.Resources.instrsxarch, @"INST\d+\((?<InstructionID>\w+)\s*,\s*""(?<InstructionName>\w+)""");
			while (tmp1.Success)
			{
				var id = tmp1.Groups["InstructionID"].Value;
				var name = tmp1.Groups["InstructionName"].Value;
				map1[id] = name;
				tmp1 = tmp1.NextMatch();
			}

			map1[invalid] = null;

			var map2 = new List<MapItem>();//new Dictionary<string, List<string>>();
			
			var tmp2 = Regex.Match(Properties.Resources.hwintrinsiclistxarch, @"HARDWARE_INTRINSIC\((?<IntrinsicID>\w+)\s*,\s*""(?<FunctionName>\w+)""\s*,\s*(?<Isa>[a-zA-Z\d]+)(?:_(?<Platform>X64))?\s*,\s*(?<Ival>-?\d+)\s*,\s*(?<SimdSize>\d+)\s*,\s*(?<NumArgs>-?\d+)\s*,\s*\{\s*INS_(?<TypByte>\w+)\s*,\s*INS_(?<TypUbyte>\w+)\s*,\s*INS_(?<TypShort>\w+)\s*,\s*INS_(?<TypUshort>\w+)\s*,\s*INS_(?<TypInt>\w+)\s*,\s*INS_(?<TypUint>\w+)\s*,\s*INS_(?<TypLong>\w+)\s*,\s*INS_(?<TypUlong>\w+)\s*,\s*INS_(?<TypFloat>\w+)\s*,\s*INS_(?<TypDouble>\w+)\s*}\s*,\s*(?<Category>\w+)\s*,\s*(?<Flags>[\w|]+)\)");
			while (tmp2.Success)
			{
				//var id = tmp2.Groups["IntrinsicID"].Value;
				var mi = new MapItem
				{
					FunctionName = tmp2.Groups["FunctionName"].Value,
					Isa = tmp2.Groups["Isa"].Value,
					Is64bitOnly = tmp2.Groups["Platform"].Value == "X64",
					I8 = map1[tmp2.Groups["TypByte"].Value],
					U8 = map1[tmp2.Groups["TypUbyte"].Value],
					I16 = map1[tmp2.Groups["TypShort"].Value],
					U16 = map1[tmp2.Groups["TypUshort"].Value],
					I32 = map1[tmp2.Groups["TypInt"].Value],
					U32 = map1[tmp2.Groups["TypUint"].Value],
					I64 = map1[tmp2.Groups["TypLong"].Value],
					U64 = map1[tmp2.Groups["TypUlong"].Value],
					F32 = map1[tmp2.Groups["TypFloat"].Value],
					F64 = map1[tmp2.Groups["TypDouble"].Value]
				};
				map2.Add(mi);
				tmp2 = tmp2.NextMatch();
			}

			using var ms = new MemoryStream(Properties.Resources.coreclr_src_System_Private_CoreLib_shared_System_Runtime_Intrinsics_X86);
			using var zip = new ZipArchive(ms);
			foreach (var entry in zip.Entries)
			{
				using var ze = entry.Open();
				using var tr = new StreamReader(ze);

				var tree = CSharpSyntaxTree.ParseText(tr.ReadToEnd());
				var compilation = CSharpCompilation.Create("Test")
					.AddReferences(
						MetadataReference.CreateFromFile(
							typeof(object).Assembly.Location))
					.AddReferences(
						MetadataReference.CreateFromFile(
							typeof(Vector128).Assembly.Location))
					.AddSyntaxTrees(tree);
				var model = compilation.GetSemanticModel(tree);
				
				var root = (CompilationUnitSyntax)tree.GetRoot();
				var mm = root.DescendantNodes();
				foreach (var m in mm.OfType<MethodDeclarationSyntax>())
				{
					var mdi = model.GetDeclaredSymbol(m);
					var cn = mdi.ContainingType.Name;
					var t = model.GetDeclaredSymbol(m.ParameterList.Parameters.First());
					var mems = compilation.GetTypeByMetadataName(t.Type.MetadataName);
					var cs = map2.FirstOrDefault(i => i.FunctionName == m.Identifier.ToString() && i.Isa == cn.ToUpper());
					// if (!string.IsNullOrEmpty(cs.U8) && mdi.Parameters.Any(p => p.OriginalDefinition == typeof(byte).Name))
					// {
					// 	var s = 0;
					// }
					// SyntaxTrivia comment = default;
					// if (!(m is MethodDeclarationSyntax) || !m.HasLeadingTrivia || (m.GetLeadingTrivia()).All(t => (comment = t).Kind() != SyntaxKind.SingleLineDocumentationCommentTrivia)) continue;
					// var xml = comment.GetStructure() as DocumentationCommentTriviaSyntax;
					// if (xml == null) continue;
					// foreach (var c in xml.Content.OfType<XmlElementSyntax>())
					// {
					// 	var r = Regex.Match(c.Content.ToFullString(), @"_m(?:m)?[\d]*_[\w_]+");
					// 	if (!r.Success) continue;
					// 	var i = r.Value;
					// 	var md = ((MethodDeclarationSyntax)m);
					// 	var mdi = model.GetDeclaredSymbol(md);
					// 	var cn = mdi.ContainingType.Name;
					// 	netIntrinsics.Add(new DotNetIntrinsicInfo(md.Identifier.ToString(), cn, i, md.ReturnType.ToString(), md.ParameterList.Parameters.Select(p => new DotNetIntrinsicInfo.Parameter(p.Identifier.ToString(), p.Type.ToString())).ToList()));
					// }
				}
			}

			return netIntrinsics;
		}
		//
		// private static TEnum GetEnumValueByDescription<TEnum>(string descr)
		// 	where TEnum : struct, IComparable, IFormattable, IConvertible
		// {
		// 	var enumType = typeof(TEnum);
		// 	foreach (TEnum enumValue in Enum.GetValues(enumType))
		// 	{
		// 		var enumValueMemberInfo = enumType.GetMember(enumValue.ToString())[0];
		// 		if (Attribute.IsDefined(enumValueMemberInfo, typeof(DescriptionAttribute)) && enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description == descr)
		// 		{
		// 			return enumValue;
		// 		}
		// 	}
		//
		// 	return default;
		// }
		//
		// internal static string GetEnumDescriptionByValue<TEnum>(TEnum value)
		// 	where TEnum : struct, IComparable, IFormattable, IConvertible
		// {
		// 	var enumType = typeof(TEnum);
		// 	var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
		// 	return Attribute.IsDefined(enumValueMemberInfo, typeof(DescriptionAttribute)) ? enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description : default;
		// }
		//
		// internal static Color GetColorByValue(InstructionSet value)
		// {
		// 	var enumType = typeof(InstructionSet);
		// 	var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
		// 	return Attribute.IsDefined(enumValueMemberInfo, typeof(UniqueColorAttribute)) ? enumValueMemberInfo.GetCustomAttribute<UniqueColorAttribute>().Color : default;
		// }
	}
}
