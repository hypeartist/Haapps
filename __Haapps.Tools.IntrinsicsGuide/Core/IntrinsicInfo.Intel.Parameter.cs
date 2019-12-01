using System;
using System.Collections.Generic;
using System.Text;

namespace Haapps.Tools.IntrinsicsGuide.Core
{
	public abstract partial class IntrinsicInfo
	{
		public sealed partial class Intel
		{
			public sealed partial class Parameter : IParameter
			{
				internal Parameter(string name, string type, string hint)
				{
					Name = name;
					Type = type;
					Hint = hint;
				}

				public string Name { get; }
				public string Type { get; }
				public string Hint { get; }
			}
		}
	}
}
