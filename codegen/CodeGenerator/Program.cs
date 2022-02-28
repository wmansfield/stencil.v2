using System;
using System.Windows.Forms;

namespace CodeGenerator
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new PrimaryForm());
		}
	}
}
