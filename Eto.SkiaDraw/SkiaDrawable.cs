
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
		private float scale = 1f;

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
				// Render at the screen's actual device-pixel density, not just the control's logical size.
				// Otherwise on a HiDPI/Retina display we'd rasterize a 1x buffer and let the OS stretch it,
				// producing a visibly blurry result instead of crisp shapes/text.
				var currentScale = this.ParentWindow?.LogicalPixelSize ?? 1f;
				var physicalSize = new Size((int)Math.Ceiling(this.Width * currentScale), (int)Math.Ceiling(this.Height * currentScale));

				if (physicalSize != this.etoBitmap.Size || currentScale != this.scale)
				{
					this.scale = currentScale;
					this.etoBitmap.Dispose();
					this.etoBitmap = new Bitmap(physicalSize, PixelFormat.Format32bppRgba);
					this.imgInfo = new SKImageInfo(physicalSize.Width, physicalSize.Height, this.colorType, SKAlphaType.Unpremul);

					// A resize/rescale invalidates the whole surface, regardless of what the platform reports as dirty.
					clipRectangle = new RectangleF(0, 0, this.Width, this.Height);
				}

				clipRectangle = clipRectangle.IsEmpty ? new RectangleF(0, 0, this.Width, this.Height) : clipRectangle;
				clipRectangle.Intersect(new RectangleF(0, 0, this.Width, this.Height));

				// The physical (device-pixel) clip is what actually limits work in the backing buffer; the
				// logical clip is what gets handed to consumers, since they draw using the control's own
				// (unscaled) coordinate space - see the canvas.Scale call below.
				var physicalClipRect = SKRect.Create(clipRectangle.X * this.scale, clipRectangle.Y * this.scale, clipRectangle.Width * this.scale, clipRectangle.Height * this.scale);
				var logicalClipRect = SKRect.Create(clipRectangle.X, clipRectangle.Y, clipRectangle.Width, clipRectangle.Height);

				using (var bmp = this.etoBitmap.Lock())
				{
					using (var surface = SKSurface.Create(this.imgInfo, bmp.Data, bmp.ScanWidth))
					{
						// Clip in device pixels (matching the true backing buffer) so consumers that skip
						// drawing outside ClipRect get a real perf win, and so nothing outside the requested
						// region overwrites still-valid content from a previous frame. Scale is applied after
						// clipping so consumers keep drawing in the control's logical coordinate space exactly
						// as before, regardless of device-pixel density.
						surface.Canvas.Save();
						surface.Canvas.ClipRect(physicalClipRect);
						surface.Canvas.Scale(this.scale);
						this.OnPaint(new SKPaintEventArgs(surface, this.imgInfo, logicalClipRect));
						surface.Canvas.Restore();
					}
				}

				graphics.DrawImage(this.etoBitmap, new RectangleF(0, 0, this.Width, this.Height));
			}
		}
	}
}
