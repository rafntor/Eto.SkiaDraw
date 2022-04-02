# Eto.SkiaDraw

[![Build](https://github.com/rafntor/Eto.SkiaDraw/actions/workflows/build.yml/badge.svg)](https://github.com/rafntor/Eto.SkiaDraw/actions/workflows/build.yml)
[![NuGet](http://img.shields.io/nuget/v/Eto.SkiaDraw.svg)](https://www.nuget.org/packages/Eto.SkiaDraw/)
[![License](https://img.shields.io/github/license/rafntor/Eto.SkiaDraw)](LICENSE)

Provides 3 different [**Eto.Forms**](https://github.com/picoe/Eto) controls that
allows using [**SkiaSharp**](https://github.com/mono/SkiaSharp)
drawing in Eto:

* `Eto.SkiaDraw.SkiaDrawable` forwards Skia graphics for rendering to screen by `Eto.Drawing` functionality.
* `Eto.SkiaDraw.SkiaGLSurface` works like the SkiaDrawable control but renders to [OpenGL](https://www.opengl.org/) via [Eto.OpenTK](https://www.nuget.org/packages/Eto.OpenTK/).
* `Eto.SkiaDraw.SkiaPanel` tries to create a SkiaGLSurface but falls back to SkiaDrawable if OpenTK is not available.


Demo applications : https://nightly.link/rafntor/Eto.SkiaDraw/workflows/build/master

## Quickstart

Use NuGet to install [`Eto.SkiaDraw`](https://www.nuget.org/packages/Eto.SkiaDraw/), then subclass a `Eto.SkiaDraw.SkiaPanel` and override the `OnPaint` event like in the example below.  
For OpenGL support you also need to add a platform-specific [Eto.OpenTK](https://www.nuget.org/packages?q=eto+opentk) package.
```csharp
class TestView : SkiaPanel
{
   protected override void OnPaint(SKPaintEventArgs e)
   {
      SKColor [] colors = { SKColors.DeepPink, SKColors.DeepSkyBlue };

      for (int i = 0; i < 5; ++i)
         e.Surface.Canvas.DrawOval(Width/2, Height/2, Width/(2+i), Height/(2+i), new SKPaint() { Color = colors[i % 2], IsAntialias = true });
   }
}
```
![](./quickstart.png)  
