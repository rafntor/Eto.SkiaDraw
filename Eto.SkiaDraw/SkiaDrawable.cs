
using System;
using System.ComponentModel;
using SkiaSharp;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.SkiaDraw
{
	public class SkiaDrawable : Drawable
	{
		static SKColorType _color_type = Platform.Instance.IsWinForms || Platform.Instance.IsWpf ? SKColorType.Bgra8888 : SKColorType.Rgba8888;
		Bitmap _eto_bitmap = new Bitmap(1, 1, PixelFormat.Format32bppRgba);
		SKImageInfo _img_info = SKImageInfo.Empty;
		public SkiaDrawable()
		{
		}
		protected virtual void OnPaint(SKCanvas canvas)
		{
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			if (Width > 0 && Height > 0)
			{
				if (Size != _eto_bitmap.Size)
				{
					_eto_bitmap.Dispose();
					_eto_bitmap = new Bitmap(Size, PixelFormat.Format32bppRgba);
					_img_info = new SKImageInfo(Width, Height, _color_type, SKAlphaType.Premul);
				}

				using (var bmp = _eto_bitmap.Lock())
				{
					using (var surface = SKSurface.Create(_img_info, bmp.Data, bmp.ScanWidth))
					{
						OnPaint(surface.Canvas);
					}
				}

				e.Graphics.DrawImage(_eto_bitmap, PointF.Empty);
			}
		}
	}
}
