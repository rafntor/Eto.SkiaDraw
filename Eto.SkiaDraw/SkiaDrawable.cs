
namespace Eto.SkiaDraw
{
	using System;
	using System.ComponentModel;
	using Eto.Drawing;
	using Eto.Forms;
	using SkiaSharp;

	public class SkiaDrawable : Drawable
	{
		private readonly SKColorType colorType;
		private Bitmap etoBitmap = new Bitmap(1, 1, PixelFormat.Format32bppRgba);
		private SKImageInfo imgInfo = SKImageInfo.Empty;

		public SkiaDrawable()
		{
			colorType = Platform.Instance.IsWinForms || Platform.Instance.IsWpf ? SKColorType.Bgra8888 : SKColorType.Rgba8888;
		}

		protected virtual void OnPaint(SKCanvas canvas)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				OnPaint(e.Graphics);
			}
			catch (Exception ex)
			{
				e.Graphics.DrawText(Fonts.Monospace(12), Colors.Red, PointF.Empty, ex.ToString());
			}
		}
		private void OnPaint(Graphics graphics)
		{
			if (this.Width > 0 && this.Height > 0)
			{
				if (this.Size != this.etoBitmap.Size)
				{
					this.etoBitmap.Dispose();
					this.etoBitmap = new Bitmap(this.Size, PixelFormat.Format32bppRgba);
					this.imgInfo = new SKImageInfo(this.Width, this.Height, colorType, SKAlphaType.Premul);
				}

				using (var bmp = this.etoBitmap.Lock())
				{
					using (var surface = SKSurface.Create(this.imgInfo, bmp.Data, bmp.ScanWidth))
					{
						this.OnPaint(surface.Canvas);
					}
				}

				graphics.DrawImage(this.etoBitmap, PointF.Empty);
			}
		}
	}
}
