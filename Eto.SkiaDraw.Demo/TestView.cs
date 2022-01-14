
namespace Eto.SkiaDraw.Demo
{
	using SkiaSharp;

	internal class TestView : SkiaDrawable
	{
		public TestView()
		{
		}

		protected override void OnPaint(SKCanvas canvas)
		{
			SKColor [] colors = { SKColors.DeepPink, SKColors.DeepSkyBlue };

			for (int i = 0; i < 5; ++i)
				canvas.DrawOval((float)this.Width / 2, (float)this.Height / 2, (float)this.Width / (3 + i), (float)this.Height / (3 + i), new SKPaint() { Color = colors[i % 2], IsAntialias = true });
		}
	}
}
