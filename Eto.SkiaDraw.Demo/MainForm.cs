
namespace Eto.SkiaDraw.Demo
{
	using Eto.Forms;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();

			var t1 = new TestView();
			var t2 = new TestView2();

			var check = new CheckBox { Text = "OpenGL Mode", Checked = t1.UseOpenGL };
			check.CheckedChanged += (o, e) => { t1.UseOpenGL = t2.UseOpenGL = check.Checked.Value; };

			var layout = new TableLayout(
				new TableRow(new TableCell(t1, true), new TableCell(t2, true)));

			Content = new DynamicLayout(new StackLayout(check), layout);

			//Content = layout;
		}
	}
}
