using System;
using System.Drawing;

namespace Haapps.Tools.IntrinsicsGuide.NewFolder1
{
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class UniqueColorAttribute : Attribute
	{
		public UniqueColorAttribute(string color) => Color = Color.FromName(color);

		public Color Color { get; }
	}
}