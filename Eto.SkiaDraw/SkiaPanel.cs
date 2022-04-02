
namespace Eto.SkiaDraw
{
	using System;
	using Eto.Forms;

	public class SkiaPanel : Panel
	{
		public event EventHandler<SKPaintEventArgs>? Paint;

		public SkiaPanel()
		{
			try
			{
				UseOpenGL = true;
			}
			catch // eat it and stay in non-gl-mode
			{
			}
		}

		public bool UseOpenGL
		{
			get => Content is SkiaGLSurface;
			set
			{
				if (value && !UseOpenGL)
				{
					Content?.Dispose();
					Content = null;

					try
					{
						var skia = new SkiaGLSurface();
						skia.Paint += (o, e) => OnPaint(e);
						Content = skia;
					}
					catch (Exception ex)
					{
						UseOpenGL = false;
						throw new Exception("Failed to create SkiaGLSurface control", ex);
					}
				}

				if (!value && !(Content is SkiaDrawable))
				{
					var skia = new SkiaDrawable();
					skia.Paint += (o, e) => OnPaint(e);
					Content?.Dispose();
					Content = skia;
				}
			}
		}
		protected virtual void OnPaint(SKPaintEventArgs e)
		{
			this.Paint?.Invoke(this, e);
		}
	}
}
