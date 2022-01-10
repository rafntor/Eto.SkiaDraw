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

#if DEBUG
			Content.MouseDoubleClick += (o, e) => (Content as SkiaDrawable).Test(100);
#endif
		}
	}
}
