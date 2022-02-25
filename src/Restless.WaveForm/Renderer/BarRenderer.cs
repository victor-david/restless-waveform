using Restless.WaveForm.Settings;
using System;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Represents a renderer that produces a bar type rendering
    /// </summary>
    public class BarRenderer : Renderer
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => "Bar";

        /// <summary>
        /// Initializes a new instance of the <see cref="BarRenderer"/> class.
        /// </summary>
        public BarRenderer()
        {
        }

        /// <summary>
        /// Performs the rendering operation.
        /// </summary>
        /// <param name="graphics">The graphics object used to draw the rendering</param>
        protected override void Render(Graphics graphics)
        {
            int x = 0;
            Pen penTop = Settings.GetPen(RenderPenType.PrimaryLine);
            Pen penBottom = Settings.GetPen(RenderPenType.SecondaryLine);

            int sampleCount = ReadSamples();

            while (sampleCount > 0)
            {
                for (int idx = 0; idx < sampleCount - Settings.ActualSampleResolution; idx += Settings.ActualSampleResolution)
                {
                    float value = Math.Abs(GetAppliedScaledValue(GetCalculatorValue(idx)));
                    float y1 = CenterY - value;
                    float y2 = CenterY + value;

                    graphics.DrawLine(penTop, x, CenterY, x, y1);
                    graphics.DrawLine(penBottom, x, CenterY + Settings.CenterLineThickness, x, y2);

                    x += Settings.ActualZoomX;
                }
                sampleCount = ReadSamples();
            }
        }
    }
}