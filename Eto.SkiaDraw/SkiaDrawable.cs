
using System;
using SkiaSharp;
using Eto.Forms;
using Eto.Drawing;

namespace Eto.SkiaDraw
{
	public class SkiaDrawable : Drawable
	{
		Bitmap _eto_bitmap = new Bitmap(1, 1, PixelFormat.Format32bppRgba);
		SKCanvas _skia_canvas = new SKCanvas(new SKBitmap());
		SKBitmap _skia_bitmap = new SKBitmap();
		public SkiaDrawable()
		{
		}
		protected virtual void OnPaint(SKCanvas canvas)
		{
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			ReallocBitmaps();

			OnPaint(_skia_canvas);

			CopyBitmap1();

			e.Graphics.DrawImage(_eto_bitmap, PointF.Empty);
		}
		void ReallocBitmaps()
		{
			if (Size != _eto_bitmap.Size)
			{
				_skia_canvas.Dispose();
				_skia_bitmap.Dispose();
				_eto_bitmap.Dispose();

				_eto_bitmap = new Bitmap(Size, PixelFormat.Format32bppRgba);
				_skia_bitmap = new SKBitmap(Width, Height, false);
				_skia_canvas = new SKCanvas(_skia_bitmap);
			}
		}
		void CopyBitmap1()
		{
			using (var eto = _eto_bitmap.Lock())
			{
				unsafe
				{
					System.Buffer.MemoryCopy(
						_skia_bitmap.GetPixels().ToPointer(), eto.Data.ToPointer(),
						_eto_bitmap.Height * eto.ScanWidth, _skia_bitmap.ByteCount);
				}
			}
		}
#if DEBUG
		void CopyBitmap2()
		{
			using (var eto = _eto_bitmap.Lock())
			{
				unsafe
				{
					var dest = (byte*)eto.Data.ToPointer();
					var src = (byte*)_skia_bitmap.GetPixels();

					var dest_siz = Height * eto.ScanWidth;
					var src_siz = _skia_bitmap.ByteCount;
					var siz = Math.Min(dest_siz, src_siz);

					for (int i = 0; i < siz; ++i)
						dest[i] = src[i];
				}
			}
		}
		void CopyBitmap3()
		{
			using var memory = new System.IO.MemoryStream();

			if (_skia_bitmap.Encode(memory, SKEncodedImageFormat.Png, 100))
				_eto_bitmap = new Bitmap(memory);
		}
		public void Test(int num)
		{
			var sw = new System.Diagnostics.Stopwatch();

			sw.Start();

			for (int i = 0; i < num; ++i)
				CopyBitmap1();

			sw.Stop();

			MessageBox.Show($"copy1 : {sw.ElapsedMilliseconds} ms");

			//

			sw.Start();

			for (int i = 0; i < num; ++i)
				CopyBitmap2();

			sw.Stop();

			MessageBox.Show($"copy2 : {sw.ElapsedMilliseconds} ms");

			//

			sw.Start();

			for (int i = 0; i < num; ++i)
				CopyBitmap3();

			sw.Stop();

			MessageBox.Show($"copy3 : {sw.ElapsedMilliseconds} ms");
		}
#endif
	}
}
