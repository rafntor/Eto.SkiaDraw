
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

		public event EventHandler<SKPaintEventArgs> Paint;

		public SkiaDrawable()
		{
			this.colorType = Platform.Instance.IsWinForms || Platform.Instance.IsWpf ? SKColorType.Bgra8888 : SKColorType.Rgba8888;
		}

		protected virtual void OnPaint(SKPaintEventArgs e)
		{
			this.Paint?.Invoke(this, e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				this.OnPaint(e.Graphics, e.ClipRectangle);
			}
			catch (Exception ex)
			{
				e.Graphics.DrawText(Fonts.Monospace(12), Colors.Red, PointF.Empty, ex.ToString());
			}
		}

		private void OnPaint(Graphics graphics, RectangleF clipRectangle)
		{
			if (this.Width > 0 && this.Height > 0)
			{
				if (this.Size != this.etoBitmap.Size)
				{
					this.etoBitmap.Dispose();
					this.etoBitmap = new Bitmap(this.Size, PixelFormat.Format32bppRgba);
					this.imgInfo = new SKImageInfo(this.Width, this.Height, this.colorType, SKAlphaType.Unpremul);

					// A resize invalidates the whole surface, regardless of what the platform reports as dirty.
					clipRectangle = new RectangleF(0, 0, this.Width, this.Height);
				}

				var clipRect = SKRect.Create(this.imgInfo.Width, this.imgInfo.Height);
				if (!clipRectangle.IsEmpty)
				{
					clipRect.Intersect(SKRect.Create(clipRectangle.X, clipRectangle.Y, clipRectangle.Width, clipRectangle.Height));
				}

				using (var bmp = this.etoBitmap.Lock())
				{
					using (var surface = SKSurface.Create(this.imgInfo, bmp.Data, bmp.ScanWidth))
					{
						// Clip the canvas so consumers that skip drawing outside ClipRect (e.g. via
						// viewport culling) get a real perf win, and so nothing outside the requested
						// region overwrites still-valid content from a previous frame.
						surface.Canvas.Save();
						surface.Canvas.ClipRect(clipRect);
						this.OnPaint(new SKPaintEventArgs(surface, this.imgInfo, clipRect));
						surface.Canvas.Restore();
					}
				}

				graphics.DrawImage(this.etoBitmap, PointF.Empty);
			}
		}
	}
}
