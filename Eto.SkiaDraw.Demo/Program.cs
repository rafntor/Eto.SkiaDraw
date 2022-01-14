
namespace Eto.SkiaDraw.Demo
{
	using System;
	using Eto.Forms;

	public static class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			new Application(Eto.Platform.Detect).Run(new MainForm());
		}
	}
}
