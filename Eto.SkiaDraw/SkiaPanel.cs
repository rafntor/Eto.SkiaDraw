
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
				UseOpenGL = false;
			}
		}

		public bool UseOpenGL
		{
			get => !(Content is SkiaDrawable);
			set
			{
				if (value)
					createOpenTK();
				else
					createDrawable();
			}
		}
		protected virtual void OnPaint(SKPaintEventArgs e)
		{
			this.Paint?.Invoke(this, e);
		}
		void createOpenTK()
		{
			if (!(Content is Skia.Veldrid.SkiaGLSurface))
			{
				Skia.Veldrid.SkiaGLSurface skia;

				try
				{
					skia = new Skia.Veldrid.SkiaGLSurface();
				}
				catch (Exception ex)
				{
					throw new Exception("Failed to create SkiaGLSurface control", ex);
				}

				skia.Paint += (o, e) => OnPaint(new SKPaintEventArgs(e.Surface, e.Info));

				Content?.Dispose();
				Content = skia;
			}
		}
		void createDrawable()
		{
			if (!(Content is SkiaDrawable))
			{
				var skia = new SkiaDrawable();

				skia.Paint += (o, e) => OnPaint(e);

				Content?.Dispose();
				Content = skia;
			}
		}
	}
}
