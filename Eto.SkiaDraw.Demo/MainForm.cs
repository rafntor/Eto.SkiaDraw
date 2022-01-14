
namespace Eto.SkiaDraw.Demo
{
	using Eto.Forms;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();

			this.Content = new TestView();
		}
	}
}
