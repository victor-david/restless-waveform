using NAudio.Wave;
using Restless.WaveForm.Calculators;
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
        /// Asynchronously creates a set of images from the specified wave stream using <see cref="SineRenderer"/>
        /// and <see cref="AverageCalculator"/>
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns> 
        public static async Task<RenderResult> CreateAsync(WaveStream waveStream, RenderSettings settings)
        {
            return await Task.Run(() =>
            {
                return Create(new SineRenderer(), waveStream, new AverageCalculator(), settings);
            });
        }

        /// <summary>
        /// Asynchronously creates an image from the specified wave stream using the specified renderer and calculator.
        /// </summary>
        /// <param name="renderer">The renderer</param>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="calculator">The sample calculator</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static async Task<RenderResult> CreateAsync(IRenderer renderer, WaveStream waveStream, ISampleCalculator calculator, RenderSettings settings)
        {
            return await Task.Run(() =>
            {
                return Create(renderer, waveStream, calculator, settings);
            });
        }

        /// <summary>
        /// Creates a set of images from the specified wave stream using <see cref="SineRenderer"/>
        /// and <see cref="AverageCalculator"/>
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(WaveStream waveStream, RenderSettings settings)
        {
            return Create(new SineRenderer(), waveStream, new AverageCalculator(), settings);
        }

        /// <summary>
        /// Creates an image from the specified wave stream using the specified renderer and calculator.
        /// </summary>
        /// <param name="renderer">The renderer</param>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="calculator">The sample calculator</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(IRenderer renderer, WaveStream waveStream, ISampleCalculator calculator, RenderSettings settings)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            if (waveStream == null)
            {
                throw new ArgumentNullException(nameof(waveStream));
            }

            if (calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return CreatePrivate(renderer, waveStream, calculator, settings);
        }

        private static RenderResult CreatePrivate(IRenderer renderer, WaveStream stream, ISampleCalculator calculator, RenderSettings settings)
        {
            long samples = stream.SampleCount();
            int channels = stream.WaveFormat.Channels;

            RenderResult result = new(settings.CreateBitmapImage(samples, channels), settings.CreateBitmapImage(samples, channels), samples, channels);

            using (Graphics gleft = Graphics.FromImage(result.ImageLeft))
            {
                using (Graphics gright = Graphics.FromImage(result.ImageRight))
                {
                    renderer.Init(result.ImageLeft, stream, calculator, settings).Render(Channel.Left, gleft);
                    if (channels == 2)
                    {
                        renderer.Init(result.ImageRight, stream, calculator, settings).Render(Channel.Right, gright);
                    }
                }
            }
            return result;
        }
    }
}