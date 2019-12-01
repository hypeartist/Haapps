namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Net
		{
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
}