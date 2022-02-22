using System;
using System.Drawing;

namespace Restless.WaveForm
{
    public class SineRenderer : Renderer
    {
        public override string DisplayName => "Sine";

        /// <summary>
        /// Initializes a new instance of the <see cref="SineRenderer"/> class.
        /// </summary>
        public SineRenderer()
        {
        }

        protected override void PerformRender(Graphics graphics)
        {
            SampleProvider.Read(Buffer, 0, Buffer.Length);
            float center = Settings.Height;
            int x = 0;
            for (int idx = 0; idx < Buffer.Length - Settings.SampleResolution; idx += Settings.SampleResolution)
            {
                float y1 = center - (Buffer[idx] * Settings.Height);
                float y2 = center - (Buffer[idx + Settings.SampleResolution] * Settings.Height);

                graphics.DrawLine(Pens.Red, x, y1, x + Settings.ZoomX, y2);
                x += Settings.ZoomX;
            }
        }
    }
}