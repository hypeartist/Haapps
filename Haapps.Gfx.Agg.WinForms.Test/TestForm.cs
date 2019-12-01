using System.Windows.Forms;
using DemoSurface = Haapps.Gfx.Agg.WinForms.Test.Examples.AATest.DemoSurface;

namespace Haapps.Gfx.Agg.WinForms.Test
{
	public partial class TestForm : Form
	{
		public TestForm()
		{
			InitializeComponent();

			var ctrl = new DemoSurface {Dock = DockStyle.Fill};
			Controls.Add(ctrl);
		}
	}
}