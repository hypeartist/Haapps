namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Intel
		{
			public sealed class Operation
			{
				internal Operation(string description, bool validate)
				{
					Description = description;
					Validate = validate;
				}

				public bool Validate { get; }
				public string Description { get; }
			}
		}
	}
}