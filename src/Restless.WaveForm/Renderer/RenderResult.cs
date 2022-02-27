using System;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Represents the result of a render operation
    /// </summary>
    public class RenderResult
    {
        /// <summary>
        /// Gets the image for the left channel.
        /// </summary>
        public Image ImageLeft
        {
            get;
        }

        /// <summary>
        /// Gets the image for the right channel.
        /// </summary>
        public Image ImageRight
        {
            get;
        }

        /// <summary>
        /// Gets the number of channels for this result.
        /// </summary>
        /// <remarks>
        /// If there are two channels, <see cref="ImageLeft"/> and <see cref="ImageRight"/>
        /// are different. If only one channel, they are the same.
        /// </remarks>
        public int Channels
        {
            get;
        }

        /// <summary>
        /// Gets the number of samples for this result.
        /// </summary>
        public long SampleCount
        {
            get;
        }

        /// <summary>
        /// From this assembly, initializes a new instance of the <see cref="RenderResult"/> class.
        /// </summary>
        /// <param name="imageLeft">The image for the left channel</param>
        /// <param name="imageRight">The image for the right channel</param>
        /// <param name="samples">The total sample count</param>
        /// <param name="channels">The number of channels</param>
        internal RenderResult(Image imageLeft, Image imageRight, long samples, int channels)
        {
            ImageLeft = imageLeft ?? throw new ArgumentNullException(nameof(imageLeft));
            ImageRight = imageRight ?? throw new ArgumentNullException(nameof(imageRight));
            SampleCount = samples;
            Channels = channels;
        }
    }
}