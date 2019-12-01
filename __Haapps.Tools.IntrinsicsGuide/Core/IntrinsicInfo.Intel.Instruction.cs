namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Intel
		{
			public sealed class Instruction
			{
				internal Instruction(string name, string form, bool xed)
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
		}
	}
}