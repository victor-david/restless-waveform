using System;
using System.Drawing;

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
        protected override void Render(Graphics graphics)
        {
            int x = 0;
            Pen penTop = Settings.GetPen(PenType.PrimaryLine);
            Pen penBottom = Settings.GetPen(PenType.SecondaryLine);

            int sampleCount = ReadSamples();

            while (sampleCount > 0)
            {
                for (int idx = 0; idx < sampleCount - Settings.ActualSampleResolution; idx += Settings.ActualSampleResolution)
                {
                    float y1 = CenterY - Math.Abs(Buffer[idx] * Settings.Height * Settings.VolumeBoost);
                    float y2 = CenterY + Math.Abs(Buffer[idx] * Settings.Height * Settings.VolumeBoost);

                    graphics.DrawLine(penTop, x, CenterY, x, y1);
                    graphics.DrawLine(penBottom, x, CenterY + Settings.CenterLineThickness, x, y2);

                    x += Settings.ActualZoomX;
                }
                sampleCount = ReadSamples();
            }
        }
    }
}