
namespace Eto.SkiaDraw.Demo
{
	using SkiaSharp;

	internal class TestView2 : SkiaDrawable
	{
		public TestView2()
		{
			this.Paint += TestView2_Paint;
		}

		private void TestView2_Paint(object sender, SKPaintEventArgs e)
		{
			SKColor[] colors = { SKColors.MediumPurple, SKColors.GreenYellow };

			for (int i = 0; i < 5; ++i)
			{
				var c = this.Size / 2;

				var w = (float)this.Width / (2 + i);
				var h = (float)this.Height / (2 + i);

				e.Surface.Canvas.DrawRect(c.Width - w, c.Height - h, w*2, h*2, new SKPaint() { Color = colors[i % 2], IsAntialias = true });
			}
		}
	}
}
