using System;
using System.Linq;
using Haapps.Tools.IntrinsicsGuide;

namespace ConsoleApp2
{
	class Program
	{
		static void Main(string[] args)
		{
			var ii = IntelIntrinsicInfo.Collect().GetAwaiter().GetResult();
			var nci = NetCoreIntrinsicInfo.Collect().GetAwaiter().GetResult();
			var ic = nci.Count(i => i.IntelName == "_mm_sfence");
			foreach (var i in ii)
			{
				var nii = nci.Where(ni => ni.IntelName == i.Name).ToList();
				if(!nii.Any()) continue;
				var s = 0;
			}
			Console.WriteLine("Hello World!");
		}
	}
}
