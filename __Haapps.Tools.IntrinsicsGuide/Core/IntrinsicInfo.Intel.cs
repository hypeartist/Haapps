using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Intel : IIntrinsicInfo
		{
			internal Intel(XmlNode node, IEnumerable<Net> netIntrinsics)
			{
				Technology = GetEnumValueByDescription<Technology>(node.Attributes?.GetNamedItem("tech")?.Value);
				ReturnType = node.Attributes?.GetNamedItem("rettype")?.Value;
				Name = node.Attributes?.GetNamedItem("name")?.Value.ToLower();
				Types = node.SelectNodes(@"type")?.Cast<XmlNode>().Select(n => GetEnumValueByDescription<Type>(n.InnerText)).ToList();
				CPUIDs = node.SelectNodes(@"CPUID")?.Cast<XmlNode>().Select(n => GetEnumValueByDescription<CPUID>(n.InnerText)).ToList();
				Categories = node.SelectNodes(@"category")?.Cast<XmlNode>().Select(n => GetEnumValueByDescription<Category>(n.InnerText)).ToList();
				Parameters = node.SelectNodes(@"parameter")?.Cast<XmlNode>().Select(n => new Parameter(n.Attributes?.GetNamedItem("varname")?.Value, n.Attributes.GetNamedItem("type")?.Value, n.Attributes.GetNamedItem("hint")?.Value)).ToList();
				Description = node.SelectNodes(@"description")?.Cast<XmlNode>().Select(n => n.InnerText).FirstOrDefault();
				Operations = node.SelectNodes(@"operation")?.Cast<XmlNode>().Select(n => new Operation(n.InnerText, n.Attributes?.GetNamedItem("validate")?.Value?.ToLower() != "false")).ToList();
				Instructions = node.SelectNodes(@"instruction")?.Cast<XmlNode>().Select(n => new Instruction(n.Attributes?.GetNamedItem("name")?.Value, n.Attributes.GetNamedItem("form")?.Value, n.Attributes.GetNamedItem("xed") != null)).ToList();
				Sequence = node.Attributes?.GetNamedItem("sequence")?.Value?.ToLower() == "true";
				VEX = node.Attributes?.GetNamedItem("vexEq")?.Value?.ToLower() == "true";
				Header = node.SelectNodes(@"header")?.Cast<XmlNode>().Select(n => n.InnerText).FirstOrDefault();
				NetIntrinsic = netIntrinsics.FirstOrDefault(n => n.IntelName == Name);
				Color = GetColorByValue(Technology);
			}

			public string Description { get; }
			public string Header { get; }
			public IEnumerable<Type> Types { get; }
			public IEnumerable<CPUID> CPUIDs { get; }
			public IEnumerable<Category> Categories { get; }
			public IEnumerable<IParameter> Parameters { get; }
			public IEnumerable<Operation> Operations { get; }
			public IEnumerable<Instruction> Instructions { get; }
			public string InstructionAsText => string.Join(", ", Instructions.Select(i => i.ToString()));
			public Technology Technology { get; }
			public string ReturnType { get; }
			public string Name { get; }
			public bool Sequence { get; }
			public bool VEX { get; }
			public Net NetIntrinsic { get; }
			public Color Color { get; }
		}
	}
}