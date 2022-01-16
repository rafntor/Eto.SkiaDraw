
namespace Eto.SkiaDraw.Demo
{
	using Eto.Forms;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();

			this.Content = new TableLayout(new TableRow(new TableCell(new TestView(), true), new TableCell(new TestView2(), true)));
		}
	}
}
