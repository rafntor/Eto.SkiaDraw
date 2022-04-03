
namespace Eto.Skia.Veldrid
{
	using System;
	using SkiaSharp;

	public class SKPaintEventArgs : EventArgs
	{
		public SKSurface Surface { get; private set; }
		public SKImageInfo Info { get; private set; }

		public SKPaintEventArgs(SKSurface surface, SKImageInfo info)
		{
			this.Surface = surface;
			this.Info = info;
		}
	}
}
