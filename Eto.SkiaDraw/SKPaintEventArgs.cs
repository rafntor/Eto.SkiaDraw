
namespace Eto.SkiaDraw
{
	using System;
	using SkiaSharp;

	public class SKPaintEventArgs : EventArgs
	{
		public SKSurface Surface { get; private set; }

		public SKImageInfo Info { get; private set; }

		/// <summary>
		/// The region (in surface pixel coordinates) that actually needs to be redrawn this frame.
		/// The surface's backing buffer persists between paints, so content outside this rect from the
		/// previous frame is still valid; consumers can use this to skip work (e.g. cull shapes) that
		/// falls entirely outside it. Always clamped to the surface bounds.
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
