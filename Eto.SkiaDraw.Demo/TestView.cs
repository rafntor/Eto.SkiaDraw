using Eto.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eto.SkiaDraw.Demo
{
	class TestView : SkiaDrawable
	{
		public TestView()
		{
		}

		protected override void OnPaint(SKCanvas canvas)
		{
			SKColor [] colors= { SKColors.DeepPink, SKColors.DeepSkyBlue };

			for (int i=0;i<5; ++i)
				canvas.DrawOval(Width / 2, Height / 2, Width/(3+i), Height / (3+i), new SKPaint() { Color = colors[i % 2], IsAntialias=true });
		}
	}
}
