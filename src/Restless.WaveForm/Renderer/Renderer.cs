using NAudio.Wave;
using Restless.WaveForm.Settings;
using System;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Base class for renderers
    /// </summary>
    public abstract class Renderer : IRenderer
    {
        #region Private
        private ISampleProvider sampleProvider;
        #endregion

        /************************************************************************/

        #region Properties / fields
        /// <summary>
        /// Buffer size, 2^20
        /// </summary>
        public const int BufferSize = 1048576;

        /// <summary>
        /// Gets the display name
        /// </summary>
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

        protected RenderSettings Settings
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the vertical center
        /// </summary>
        protected float CenterY => Settings.Height;


        protected long TotalSamples
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the buffer, fixed size of <see cref="BufferSize"/>
        /// </summary>
        protected float[] Buffer
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class.
        /// </summary>
        protected Renderer()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Initializes the renderer
        /// </summary>
        /// <param name="image">The image on which to render</param>
        /// <param name="stream">The wave stream</param>
        /// <param name="settings">Settings</param>
        /// <returns>This instance of <see cref="Renderer"/>.</returns>
        public IRenderer Init(Image image, WaveStream stream, RenderSettings settings)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            TotalSamples = stream.SampleCount();
            sampleProvider = stream.ToSampleProvider();
            Buffer = new float[BufferSize];
            return this;
        }

        /// <summary>
        /// Performs the rendering operation
        /// </summary>
        /// <param name="channel">The channel to render</param>
        /// <param name="graphics">The graphics object</param>
        public void Render(Channel channel, Graphics graphics)
        {
            if (Image == null)
            {
                throw new InvalidOperationException("Not initialized");
            }

            if (graphics == null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            graphics.FillRectangle(Settings.GetBackgroundBrush(), 0, 0, Image.Width, Image.Height);

            if (Stream.WaveFormat.Channels == 2)
            {
                switch (channel)
                {
                    case Channel.Left:
                        sampleProvider = sampleProvider.ToMono(0.5f, 0);
                        break;
                    case Channel.Right:
                        sampleProvider = sampleProvider.ToMono(0, 1.5f);
                        break;
                    default:
                        break;
                }
            }

            Stream.Position = 0;
            
            Render(graphics);
            DrawCenterLine(graphics);
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Performs the rendering operation
        /// </summary>
        /// <param name="graphics">The graphics object used to draw the rendering</param>
        protected abstract void Render(Graphics graphics);

        /// <summary>
        /// Reads from the stream sample provider into the buffer
        /// </summary>
        /// <returns>The number of samples read</returns>
        protected int ReadSamples()
        {
            return sampleProvider.Read(Buffer, 0, BufferSize);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void DrawCenterLine(Graphics graphics)
        {
            if (Settings.CenterLineThickness > 0)
            {
                float centerY = CenterY + Settings.CenterLineThickness;
                Pen pen = Settings.GetPen(RenderPenType.CenterLine);
                graphics.DrawLine(pen, 0, centerY, Image.Width, centerY);

            }
        }
        #endregion
    }
}