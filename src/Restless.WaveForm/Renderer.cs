using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Restless.WaveForm
{
    public abstract class Renderer : IRenderer
    {
        public virtual string DisplayName { get; }

        protected Image Image
        {
            get;
            private set;
        }

        protected WaveStream Stream
        {
            get;
            private set;
        }

        protected Settings Settings
        {
            get;
            private set;
        }

        protected ISampleProvider SampleProvider
        {
            get;
            private set;
        }

        protected long TotalSamples
        {
            get;
            private set;
        }

        protected float[] Buffer
        {
            get;
            private set;
        }
        

        protected Renderer()
        {

        }

        public IRenderer Init(Image image, WaveStream stream, Settings settings)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            int bytesPerSample = stream.WaveFormat.BitsPerSample / 8;
            TotalSamples = stream.Length / bytesPerSample;
            SampleProvider = stream.ToSampleProvider();
            Buffer = new float[TotalSamples];
            return this;
        }

        public void Render(Channel channel, Graphics graphics)
        {
            if (Image == null)
            {
                throw new InvalidOperationException("Not initialized");
            }

            graphics.FillRectangle(Settings.BackgroundBrush, 0, 0, Image.Width, Image.Height);

            if (Stream.WaveFormat.Channels == 2)
            {
                switch (channel)
                {
                    case Channel.Left:
                        SampleProvider = SampleProvider.ToMono(0.5f, 0);
                        break;
                    case Channel.Right:
                        SampleProvider = SampleProvider.ToMono(0, 1.5f);
                        break;
                    default:
                        break;
                }
            }

            Stream.Position = 0;
            PerformRender(graphics);
            DrawCenterLine(graphics);
        }

        protected virtual void PerformRender(Graphics graphics)
        {
        }

        private void DrawCenterLine(Graphics graphics)
        {
            
            if (Settings.CenterLineHeight > 0)
            {
                int centerY = Settings.Height + 1;
                graphics.DrawLine(Settings.CenterLinePen, 0, centerY, Image.Width, centerY);

                if (Settings.CenterLineHeight > 1)
                {
                    graphics.DrawLine(Settings.CenterLinePen, 0, centerY + 1, Image.Width, centerY + 1);
                }
            }
        }
    }
}
