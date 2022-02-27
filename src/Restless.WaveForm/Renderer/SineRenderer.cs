using Restless.WaveForm.Settings;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Represents a renderer that produces a sine wave type rendering
    /// </summary>
    public class SineRenderer : Renderer
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        public override string DisplayName => "Sine";

        /// <summary>
        /// Initializes a new instance of the <see cref="SineRenderer"/> class.
        /// </summary>
        public SineRenderer()
        {
        }

        /// <summary>
        /// Performs the rendering operation
        /// </summary>
        /// <param name="graphics">The graphics object used to draw the rendering</param>
        protected override void Render(Graphics graphics)
        {
            float x = 0;
            float y1;
            float y2 = 0;

            Pen pen = Settings.GetPen(RenderPenType.PrimaryLine);
            int count = 0;

            int sampleCount = ReadSamples();

            while (sampleCount > 0)
            {
                for (int idx = 0; idx < sampleCount - Settings.ActualSampleResolution; idx += Settings.ActualSampleResolution)
                {
                    y1 = count == 0 ? CenterY - GetAppliedScaledValue(GetCalculatorValue(idx)) : y2;
                    y2 = CenterY - GetAppliedScaledValue(GetCalculatorValue(idx + Settings.ActualSampleResolution));
                    graphics.DrawLine(pen, x, y1, x + Settings.ActualXStep, y2);
                    count++;
                    x += Settings.ActualXStep;
                }
                sampleCount = ReadSamples();
            }
        }
    }
}