using System.Drawing;

namespace Restless.WaveForm
{
    /// <summary>
    /// Represents a renderer that produces a sine wave type rendering
    /// </summary>
    public class SineRenderer : Renderer
    {
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
            int x = 0;
            Pen pen = Settings.GetPen(PenType.PrimaryLine);

            int sampleCount = ReadSamples();

            while (sampleCount > 0)
            {
                for (int idx = 0; idx < sampleCount - Settings.SampleResolution; idx += Settings.SampleResolution)
                {
                    float y1 = CenterY - (Buffer[idx] * Settings.Height);
                    float y2 = CenterY - (Buffer[idx + Settings.SampleResolution] * Settings.Height);

                    graphics.DrawLine(pen, x, y1, x + Settings.ZoomX, y2);
                    x += Settings.ZoomX;
                    if (x > Image.Width)
                    {
                        return;
                    }
                }
                sampleCount = ReadSamples();
            }
        }
    }
}