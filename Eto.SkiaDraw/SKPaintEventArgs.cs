
namespace Eto.SkiaDraw
{
	using System;
	using SkiaSharp;

	public class SKPaintEventArgs : EventArgs
	{
		public SKSurface Surface { get; private set; }

		/// <summary>
		/// Describes the true backing pixel buffer, in device pixels. On a HiDPI/Retina display this will
		/// be larger than the control's own Width/Height, since <see cref="Surface"/>'s canvas is
		/// pre-scaled so drawing commands can still be issued in the control's logical coordinate space.
		/// </summary>
		public SKImageInfo Info { get; private set; }

		/// <summary>
		/// The region, in the same logical (unscaled) coordinate space as the control's own Width/Height,
		/// that actually needs to be redrawn this frame. The surface's backing buffer persists between
		/// paints, so content outside this rect from the previous frame is still valid; consumers can use
		/// this to skip work (e.g. cull shapes) that falls entirely outside it. Always clamped to the
		/// control's bounds.
		/// </summary>
		public SKRect ClipRect { get; private set; }

		public SKPaintEventArgs(SKSurface surface, SKImageInfo info, SKRect clipRect)
		{
			this.Surface = surface;
			this.Info = info;
			this.ClipRect = clipRect;
		}
	}
}
