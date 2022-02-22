using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Restless.WaveForm
{
    /// <summary>
    /// Represents a renderer that produces a bar type rendering
    /// </summary>
    public class BarRenderer : Renderer
    {
        public override string DisplayName => "Bar";

        /// <summary>
        /// Initializes a new instance of the bar renderer class
        /// </summary>
        public BarRenderer()
        {
        }

        /// <summary>
        /// Performs the rendering operation
        /// </summary>
        /// <param name="graphics">The graphics object used to draw the rendering</param>
        protected override void PerformRender(Graphics graphics)
        {
            SampleProvider.Read(Buffer, 0, Buffer.Length);
            Pen penTop = Settings.GetPen(PenType.PrimaryLine);
            Pen penBottom = Settings.GetPen(PenType.SecondaryLine);


            int x = 0;
            for (int idx = 0; idx < Buffer.Length - Settings.SampleResolution; idx += Settings.SampleResolution)
            {
                float y1 = CenterY - Math.Abs(Buffer[idx] * Settings.Height);
                float y2 = CenterY + Math.Abs(Buffer[idx] * Settings.Height);

                graphics.DrawLine(penTop, x, CenterY, x, y1);
                graphics.DrawLine(penBottom, x, CenterY + Settings.CenterLineThickness, x, y2);

                x += Settings.ZoomX;
            }
        }
    }
}