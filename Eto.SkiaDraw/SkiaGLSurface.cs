
namespace Eto.SkiaDraw
{
	using System;
	using Eto.Drawing;
	using Eto.OpenTK;
	using global::OpenTK.Graphics;
	using global::OpenTK.Graphics.ES20;
	using SkiaSharp;

	public class SkiaGLSurface : GLSurface
	{
		private const SKColorType colorType = SKColorType.Rgba8888;

		private GRBackendRenderTarget? renderTarget;
		private GRContext? grContext;
		private SKImageInfo imgInfo;
		private SKSurface? surface;
		private Size lastSize;

		public event EventHandler<SKPaintEventArgs>? Paint;

		public SkiaGLSurface()
		{
		}
		public SkiaGLSurface(GraphicsMode graphicsMode) : base(graphicsMode)
		{
		}
		public SkiaGLSurface(GraphicsMode mode, int major, int minor, GraphicsContextFlags flags) : base(mode, major, minor, flags)
		{
		}
		protected virtual void OnPaint(SKPaintEventArgs e)
		{
			this.Paint?.Invoke(this, e);
		}
		protected override void OnDraw(EventArgs e)
		{
			SwapBuffers();

			if (Size != lastSize)
			{
				RecreateSurface();

				lastSize = Size;
			}

			using (new SKAutoCanvasRestore(surface!.Canvas, true))
			{
				try // start drawing
				{
					OnPaint(new SKPaintEventArgs(surface, imgInfo));
				}
				catch (Exception ex)
				{
					surface!.Canvas.DrawText(ex.ToString(), 20, 20, new SKPaint() { Color = SKColors.Red });
				}
			}

			// update the control
			surface.Canvas.Flush();

			MakeCurrent();

			base.OnDraw(e);
		}

		void RecreateSurface()
		{
			// from https://github.com/mono/SkiaSharp/blob/ccc64cdd4950b5056444be8b915929500f548fee/source/SkiaSharp.Views/SkiaSharp.Views.WindowsForms/SKGLControl.cs#L59

			// create the contexts if not done already
			if (grContext == null)
			{
				var glInterface = GRGlInterface.Create();
				grContext = GRContext.CreateGl(glInterface);
			}

			GL.GetInteger(GetPName.Samples, out var samples);
			GL.GetInteger(GetPName.StencilBits, out var stencil);
			GL.GetInteger(GetPName.FramebufferBinding, out var framebuffer);
			var maxSamples = grContext.GetMaxSurfaceSampleCount(colorType);
			samples = Math.Min(samples, maxSamples);

			var glInfo = new GRGlFramebufferInfo((uint)framebuffer, colorType.ToGlSizedFormat());

			// re-create the render target
			renderTarget?.Dispose();
			renderTarget = new GRBackendRenderTarget(Width, Height, samples, stencil, glInfo);

			// destroy the old surface
			surface?.Dispose();
			surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, colorType);
			imgInfo = new SKImageInfo(Width, Height, colorType, SKAlphaType.Unpremul);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// clean up
			surface?.Dispose();
			surface = null;

			renderTarget?.Dispose();
			renderTarget = null;

			grContext?.Dispose();
			grContext = null;
		}
	}
}
