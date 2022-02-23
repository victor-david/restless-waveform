using NAudio.Wave;
using Restless.WaveForm.Settings;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Provides static methods to render audio
    /// </summary>
    /// <remarks>
    /// The methods of this class return two images, one for the left channel and one for the right.
    /// If the wave stream is a single channel, both images are the same. The consumer
    /// can decide based on the result (which contains the number of channels) whether to use one image or both.
    /// </remarks>
    public static class WaveFormRenderer
    {
        /// <summary>
        /// Asynchronously Creates a set of images from the specified wave stream using <see cref="SineRenderer"/>.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns> 
        public static async Task<RenderResult> CreateAsync(WaveStream waveStream, RenderSettings settings)
        {
            return await Task.Run(() =>
            {
                return Create(waveStream, new SineRenderer(), settings);
            });
        }

        /// <summary>
        /// Asynchronously creates an image from the specified wave stream and the specified renderer.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="renderer">The renderer</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static async Task<RenderResult> CreateAsync(WaveStream waveStream, IRenderer renderer, RenderSettings settings)
        {
            return await Task.Run(() =>
            {
                return Create(waveStream, renderer, settings);
            });
        }

        /// <summary>
        /// Creates a set of images from the specified wave stream using <see cref="SineRenderer"/>.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(WaveStream waveStream, RenderSettings settings)
        {
            return Create(waveStream, new SineRenderer(), settings);
        }

        /// <summary>
        /// Creates an image from the specified wave stream and the specified peak renderer.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="renderer">The renderer</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(WaveStream waveStream, IRenderer renderer, RenderSettings settings)
        {
            if (waveStream == null)
            {
                throw new ArgumentNullException(nameof(waveStream));
            }

            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            //int bytesPerSample = waveStream.WaveFormat.BitsPerSample / 8;
            //long samples = waveStream.Length / bytesPerSample;
            //int samplesPerPixel = Utility.GetEvenIntegerValue(Math.Max((int)(samples / settings.Width), 2));
            //int stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
            //peakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel * stepSize);
            return Create(renderer, waveStream, settings);
        }

        // 
        private static RenderResult Create(IRenderer renderer, WaveStream stream, RenderSettings settings)
        {
            //if (settings.DecibelScale)
            //{
            //    peakProvider = new DecibelPeakProvider(peakProvider, 48);
            //}

            long samples = stream.SampleCount();
            int channels = stream.WaveFormat.Channels;

            RenderResult result = new(settings.CreateBitmapImage(samples, channels), settings.CreateBitmapImage(samples, channels), channels);

            using (Graphics gleft = Graphics.FromImage(result.ImageLeft))
            {
                using (Graphics gright = Graphics.FromImage(result.ImageRight))
                {
                    renderer.Init(result.ImageLeft, stream, settings).Render(Channel.Left, gleft);
                    renderer.Init(result.ImageRight, stream, settings).Render(Channel.Right, gright);
                }
            }
            return result;
        }
    }
}