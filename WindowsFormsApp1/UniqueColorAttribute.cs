using System;
using System.Drawing;

namespace WindowsFormsApp1
{
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class UniqueColorAttribute : Attribute
	{
		public UniqueColorAttribute(string color) => Color = Color.FromName(color);

		public Color Color { get; }
	}
}