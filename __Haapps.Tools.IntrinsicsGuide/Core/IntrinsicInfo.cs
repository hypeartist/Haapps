using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public interface IIntrinsicInfo
		{
			string Name { get; }
			string ReturnType { get; }
			IEnumerable<IParameter> Parameters { get; }
		}

		public interface IParameter
		{
			string Name { get; }
			string Type { get; }
		}

		public enum Type
		{
			Undefined,
			[Description("Integer")]
			Integer,
			[Description("Floating Point")]
			FloatingPoint,
			[Description("Mask")]
			Mask
		}

		public enum CPUID
		{
			Undefined,
			[Description("MMX")]
			MMX,
			[Description("SSE")]
			SSE,
			[Description("KNCNI")]
			KNCNI,
			[Description("PREFETCHWT1")]
			PREFETCHWT1,
			[Description("SSE2")]
			SSE2,
			[Description("FP16C")]
			FP16C,
			[Description("SSE3")]
			SSE3,
			[Description("MONITOR")]
			MONITOR,
			[Description("SSSE3")]
			SSSE3,
			[Description("SSE4.1")]
			SSE41,
			[Description("SSE4.2")]
			SSE42,
			[Description("POPCNT")]
			POPCNT,
			[Description("AES")]
			AES,
			[Description("PCLMULQDQ")]
			PCLMULQDQ,
			[Description("AVX")]
			AVX,
			[Description("AVX2")]
			AVX2,
			[Description("CLFLUSHOPT")]
			CLFLUSHOPT,
			[Description("CLWB")]
			CLWB,
			[Description("FMA")]
			FMA,
			[Description("BMI1")]
			BMI1,
			[Description("BMI2")]
			BMI2,
			[Description("INVPCID")]
			INVPCID,
			[Description("LZCNT")]
			LZCNT,
			[Description("RTM")]
			RTM,
			[Description("RDTSCP")]
			RDTSCP,
			[Description("RDPID")]
			RDPID,
			[Description("FXSR")]
			FXSR,
			[Description("TSC")]
			TSC,
			[Description("XSAVE")]
			XSAVE,
			[Description("XSAVEOPT")]
			XSAVEOPT,
			[Description("FSGSBASE")]
			FSGSBASE,
			[Description("RDRAND")]
			RDRAND,
			[Description("RDSEED")]
			RDSEED,
			[Description("ADX")]
			ADX,
			[Description("SHA")]
			SHA,
			[Description("MPX")]
			MPX,
			[Description("MOVBE")]
			MOVBE,
			[Description("XSAVEC")]
			XSAVEC,
			[Description("XSS")]
			XSS,
			[Description("PCONFIG")]
			PCONFIG,
			[Description("CLDEMOTE")]
			CLDEMOTE,
			[Description("GFNI")]
			GFNI,
			[Description("AVX512F")]
			AVX512F,
			[Description("AVX512VL")]
			AVX512VL,
			[Description("AVX512BW")]
			AVX512BW,
			[Description("WBNOINVD")]
			WBNOINVD,
			[Description("WAITPKG")]
			WAITPKG,
			[Description("MOVDIR64B")]
			MOVDIR64B,
			[Description("MOVDIRI")]
			MOVDIRI,
			[Description("VPCLMULQDQ")]
			VPCLMULQDQ,
			[Description("AVX512_VBMI2")]
			AVX512_VBMI2,
			[Description("VAES")]
			VAES,
			[Description("AVX512_VNNI")]
			AVX512_VNNI,
			[Description("AVX512DQ")]
			AVX512DQ,
			[Description("AVX512F/KNCNI")]
			AVX512FKNCNI,
			[Description("AVX512CD")]
			AVX512CD,
			[Description("AVX512PF")]
			AVX512PF,
			[Description("AVX512PF/KNCNI")]
			AVX512PFKNCNI,
			[Description("AVX512ER")]
			AVX512ER,
			[Description("AVX512")]
			AVX512,
			[Description("AVX512_4VNNIW")]
			AVX512_4VNNIW,
			[Description("AVX512_4FMAPS")]
			AVX512_4FMAPS,
			[Description("AVX512IFMA52")]
			AVX512IFMA52,
			[Description("AVX512_VBMI")]
			AVX512_VBMI,
			[Description("AVX512_BITALG")]
			AVX512_BITALG,
			[Description("AVX512_BF16")]
			AVX512_BF16,
			[Description("AVX512VPOPCNTDQ")]
			AVX512VPOPCNTDQ,
			[Description("AVX512_VP2INTERSECT")]
			AVX512_VP2INTERSECT
		}

		public enum Category
		{
			Undefined,
			[Description("Application-Targeted")]
			ApplicationTargeted,
			[Description("Arithmetic")]
			Arithmetic,
			[Description("Bit Manipulation")]
			BitManipulation,
			[Description("Cast")]
			Cast,
			[Description("Compare")]
			Compare,
			[Description("Convert")]
			Convert,
			[Description("Cryptography")]
			Cryptography,
			[Description("Elementary Math Functions")]
			ElementaryMathFunctions,
			[Description("General Support")]
			GeneralSupport,
			[Description("Load")]
			Load,
			[Description("Logical")]
			Logical,
			[Description("Mask")]
			Mask,
			[Description("Miscellaneous")]
			Miscellaneous,
			[Description("Move")]
			Move,
			[Description("OS-Targeted")]
			OSTargeted,
			[Description("Probability/Statistics")]
			ProbabilityStatistics,
			[Description("Random")]
			Random,
			[Description("Set")]
			Set,
			[Description("Shift")]
			Shift,
			[Description("Special Math Functions")]
			SpecialMathFunctions,
			[Description("Store")]
			Store,
			[Description("String Compare")]
			StringCompare,
			[Description("Swizzle")]
			Swizzle,
			[Description("Trigonometry")]
			Trigonometry
		}

		public enum Technology
		{
			[UniqueColor(nameof(Color.Black))]
			Undefined,
			[Description("MMX")]
			[UniqueColor(nameof(Color.Gold))]
			MMX,
			[Description("SSE")]
			[UniqueColor(nameof(Color.LawnGreen))]
			SSE,
			[Description("SSE2")]
			[UniqueColor(nameof(Color.OliveDrab))]
			SSE2,
			[Description("SSE3")]
			[UniqueColor(nameof(Color.DarkGreen))]
			SSE3,
			[Description("SSSE3")]
			[UniqueColor(nameof(Color.LightSeaGreen))]
			SSSE3,
			[Description("SSE4.1")]
			[UniqueColor(nameof(Color.SteelBlue))]
			SSE41,
			[Description("SSE4.2")]
			[UniqueColor(nameof(Color.Blue))]
			SSE42,
			[Description("AVX")]
			[UniqueColor(nameof(Color.DarkOrchid))]
			AVX,
			[Description("AVX2")]
			[UniqueColor(nameof(Color.Fuchsia))]
			AVX2,
			[Description("FMA")]
			[UniqueColor(nameof(Color.PaleVioletRed))]
			FMA,
			[Description("AVX-512")]
			[UniqueColor(nameof(Color.Red))]
			AVX512,
			[Description("KNC")]
			[UniqueColor(nameof(Color.SaddleBrown))]
			KNC,
			[Description("AVX-512/KNC")]
			[UniqueColor(nameof(Color.Coral))]
			AVX512KNC,
			[Description("SVML")]
			[UniqueColor(nameof(Color.SlateGray))]
			SVML,
			[Description("SVML/KNC")]
			[UniqueColor(nameof(Color.DarkSlateGray))]
			SVMLKNC,
			[Description("Other")]
			[UniqueColor(nameof(Color.Black))]
			Other
		}

		[AttributeUsage(AttributeTargets.Field)]
		private sealed class UniqueColorAttribute : Attribute
		{
			public UniqueColorAttribute(string color)
			{
				Color = Color.FromName(color);
			}

			public Color Color { get; }
		}

		public static async Task<IEnumerable<Intel>> Collect()
		{
			IEnumerable<Intel> data = new List<Intel>();
			await Task.Run(() =>
			{
				var netIntrinsics = CollectNetIntrinsics();
				var xml = new XmlDocument();
				xml.LoadXml(Properties.Resources.software_intel_com_IntrinsicsGuide_data_3_4_6);
				data = xml.SelectNodes(@"//intrinsic")?.Cast<XmlNode>().Select(n => new Intel(n, netIntrinsics)).ToList();
			});
			return data;
		}

		private static IEnumerable<Net> CollectNetIntrinsics()
		{
			var netIntrinsics = new List<Net>();

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
					.AddSyntaxTrees(tree);
				var model = compilation.GetSemanticModel(tree);
				var root = (CompilationUnitSyntax) tree.GetRoot();
				var mm = root.DescendantNodes();
				foreach (var m in mm)
				{
					SyntaxTrivia comment = default;
					if(!(m is MethodDeclarationSyntax) || !m.HasLeadingTrivia || (m.GetLeadingTrivia()).All(t=> (comment = t).Kind() != SyntaxKind.SingleLineDocumentationCommentTrivia)) continue;
					var xml = comment.GetStructure() as DocumentationCommentTriviaSyntax;
					if(xml == null) continue;
					foreach (var c in xml.Content.OfType<XmlElementSyntax>())
					{
						var r = Regex.Match(c.Content.ToFullString(), @"_m(?:m)?[\d]*_[\w_]+");
						if(!r.Success) continue;
						var i = r.Value;
						var md = ((MethodDeclarationSyntax) m);
						var mdi = model.GetDeclaredSymbol(md);
						var cn = mdi.ContainingType.Name;
						netIntrinsics.Add(new Net(md.Identifier.ToString(), cn, i, md.ReturnType.ToString(), md.ParameterList.Parameters.Select(p => new Net.Parameter(p.Identifier.ToString(), p.Type.ToString())).ToList()));
						// var f = (ret: md.ReturnType.ToString(), name: md.Identifier.ToString(), parameters: md.ParameterList.Parameters.Select(p => (name: p.Identifier.ToString(), type: p.Type.ToString(), mods: p.Modifiers.Select(o => o.ToString()).ToList(), attributes: p.AttributeLists.Select(a => a.Target.ToString()).ToList())), $"{md.ReturnType.ToFullString()} {md.Identifier.ToFullString()}{md.ParameterList.ToFullString()}");
					}
				}
			}

			return netIntrinsics;
		}

		private static TEnum GetEnumValueByDescription<TEnum>(string descr)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			var enumType = typeof(TEnum);
			foreach (TEnum enumValue in Enum.GetValues(enumType))
			{
				var enumValueMemberInfo = enumType.GetMember(enumValue.ToString())[0];
				if (Attribute.IsDefined(enumValueMemberInfo, typeof(DescriptionAttribute)) && enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description == descr)
				{
					return enumValue;
				}
			}

			return default;
		}

		internal static string GetEnumDescriptionByValue<TEnum>(TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			var enumType = typeof(TEnum);
			var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
			return Attribute.IsDefined(enumValueMemberInfo, typeof(DescriptionAttribute)) ? enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description : default;
		}

		internal static Color GetColorByValue(Technology value)
		{
			var enumType = typeof(Technology);
			var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
			return Attribute.IsDefined(enumValueMemberInfo, typeof(UniqueColorAttribute)) ? enumValueMemberInfo.GetCustomAttribute<UniqueColorAttribute>().Color : default;
		}
	}
}