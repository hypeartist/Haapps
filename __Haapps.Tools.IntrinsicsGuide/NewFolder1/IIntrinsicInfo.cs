using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Haapps.Tools.IntrinsicsGuide.NewFolder1
{
	public interface IIntrinsicInfo
	{
		string Name { get; }
		string ReturnType { get; }
		IEnumerable<IParameter> Parameters { get; }
	}

	internal static class EnumExtensions
	{
		public static TEnum GetEnumValueByDescription<TEnum>(this string descr)
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

		public static string GetEnumDescriptionByValue<TEnum>(this TEnum value)
			where TEnum : struct, IComparable, IFormattable, IConvertible
		{
			var enumType = typeof(TEnum);
			var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
			return Attribute.IsDefined(enumValueMemberInfo, typeof(DescriptionAttribute)) ? enumValueMemberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description : default;
		}

		internal static Color GetColorByValue(this InstructionSet value)
		{
			var enumType = typeof(InstructionSet);
			var enumValueMemberInfo = enumType.GetMember(value.ToString())[0];
			return Attribute.IsDefined(enumValueMemberInfo, typeof(UniqueColorAttribute)) ? enumValueMemberInfo.GetCustomAttribute<UniqueColorAttribute>().Color : default;
		}

		public static IEnumerable<INamedTypeSymbol> GetTypesByMetadataName(this Compilation compilation, string typeMetadataName)
		{
			return compilation.References
				.Select(compilation.GetAssemblyOrModuleSymbol)
				.OfType<IAssemblySymbol>()
				.Select(assemblySymbol => assemblySymbol.GetTypeByMetadataName(typeMetadataName))
				.Where(t => t != null);
		}
	}
}