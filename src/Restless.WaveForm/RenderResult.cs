using System;
using System.Drawing;

namespace Restless.WaveForm
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
        /// Gets the image for the right channel
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

        internal RenderResult(Image imageLeft, Image imageRight, int channels)
        {
            ImageLeft = imageLeft ?? throw new ArgumentNullException(nameof(imageLeft));
            ImageRight = imageRight ?? throw new ArgumentNullException(nameof(imageRight));
            Channels = channels;
        }
    }
}