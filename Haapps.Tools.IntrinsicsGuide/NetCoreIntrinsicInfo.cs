using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Haapps.Tools.IntrinsicsGuide
{
	public sealed class NetCoreIntrinsicInfo : IIntrinsicInfo
	{
		private NetCoreIntrinsicInfo(string name, string className, string intelName, string retType, IEnumerable<NetCoreIntrinsicParameter> parameters, bool is64Bit)
		{
			Name = name;
			ClassName = className;
			IntelName = intelName;
			ReturnType = retType;
			Parameters = parameters;
			Is64Bit = is64Bit;
		}

		public string Name { get; }

		public string ClassName { get; }

		public string ReturnType { get; }

		public IEnumerable<IIntrinsicIParameter> Parameters { get; }

		public bool Is64Bit { get; }

		public string IntelName { get; }

		public override string ToString() => $"{ReturnType} {Name}({string.Join(", ", Parameters.Select(p => p.ToString()))}) // {IntelName}";

		public static async Task<IEnumerable<NetCoreIntrinsicInfo>> Collect()
		{
			var netIntrinsics = new List<NetCoreIntrinsicInfo>();
			await Task.Run(() =>
			{
				using var ms = new MemoryStream(Properties.Resources.coreclr_src_System_Private_CoreLib_shared_System_Runtime_Intrinsics_X86);
				using var zip = new ZipArchive(ms);
				foreach (var entry in zip.Entries)
				{
					using var zipEntryStream = entry.Open();
					using var streamReader = new StreamReader(zipEntryStream);

					var syntaxTree = CSharpSyntaxTree.ParseText(streamReader.ReadToEnd());
					var compilation = CSharpCompilation.Create("Test")
						.AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
						.AddSyntaxTrees(syntaxTree);
					var semanticModel = compilation.GetSemanticModel(syntaxTree);
					var syntaxTreeRoot = (CompilationUnitSyntax) syntaxTree.GetRoot();
					var syntaxNodes = syntaxTreeRoot.DescendantNodes().SelectMany(n => n.DescendantNodes()).Distinct();
					foreach (var syntaxNode in syntaxNodes)
					{
						SyntaxTrivia comment = default;
						if (!(syntaxNode is MethodDeclarationSyntax) || !syntaxNode.HasLeadingTrivia || (syntaxNode.GetLeadingTrivia()).All(t => (comment = t).Kind() != SyntaxKind.SingleLineDocumentationCommentTrivia)) continue;
						if (!(comment.GetStructure() is DocumentationCommentTriviaSyntax xml)) continue;
						foreach (var c in xml.Content.OfType<XmlElementSyntax>())
						{
							var match = Regex.Match(c.Content.ToFullString(), @"_m(?:m)?[\d]*_[\w_]+");
							if (!match.Success) continue;
							var intrinsicIntelName = match.Value;
							if (intrinsicIntelName == "_mm_sfence")
							{
								var s = 0;
							}
							var methodDeclarationSyntax = ((MethodDeclarationSyntax) syntaxNode);
							var methodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
							var className = methodSymbol.ContainingType.Name;
							var is64Bit = className == "X64";
							if (is64Bit)
							{
								className = methodSymbol.ContainingType.ContainingType?.Name;
							}
							netIntrinsics.Add(new NetCoreIntrinsicInfo(methodDeclarationSyntax.Identifier.ToString(), className, intrinsicIntelName, methodDeclarationSyntax.ReturnType.ToString(), methodDeclarationSyntax.ParameterList.Parameters.Select(p => new NetCoreIntrinsicParameter(p.Identifier.ToString(), p.Type.ToString())).ToList(), is64Bit));
						}
					}
				}

			});

			return netIntrinsics;
		}

		public sealed class NetCoreIntrinsicParameter : IIntrinsicIParameter
		{
			internal NetCoreIntrinsicParameter(string name, string type)
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