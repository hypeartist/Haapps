﻿using System.Reflection;

namespace Haapps.Utils.Uth.GacManager
{
	/// <summary>
	///     AssemblyReflectionProperties represent the properties of an assembly that
	///     are loaded by via reflection.
	/// </summary>
	public class AssemblyReflectionProperties
	{
		/// <summary>
		///     Gets or sets the runtime version.
		/// </summary>
		/// <value>
		///     The runtime version.
		/// </value>
		public string RuntimeVersion { get; set; }

		/// <summary>
		///     Loads the reflection properties from the specified path.
		/// </summary>
		/// <param name="displayName">The display name.</param>
		public void Load(string displayName)
		{
			//  Load reflection details.
			try
			{
				var assembly = Assembly.ReflectionOnlyLoad(displayName);
				RuntimeVersion = assembly.ImageRuntimeVersion;
			}
			catch
			{
				//
			}
		}
	}
}