using Eto.Drawing;
using Eto.Forms;
using System;

namespace Eto.SkiaDraw.Demo
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			Content = new TestView();
		}
	}
}
