﻿using Restless.WaveForm.Settings;
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
            Pen pen = Settings.GetPen(RenderPenType.PrimaryLine);

            int sampleCount = ReadSamples();

            while (sampleCount > 0)
            {
                for (int idx = 0; idx < sampleCount - Settings.ActualSampleResolution; idx += Settings.ActualSampleResolution)
                {
                    float y1 = CenterY - GetAppliedScaledValue(GetCalculatorValue(idx));
                    float y2 = CenterY - GetAppliedScaledValue(GetCalculatorValue(idx + Settings.ActualSampleResolution));

                    graphics.DrawLine(pen, x, y1, x + Settings.ActualXStep, y2);

                    x += Settings.ActualXStep;
                }
                sampleCount = ReadSamples();
            }
        }
    }
}