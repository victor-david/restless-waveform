using NAudio.Wave;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Restless.WaveForm
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
        /// Asynchronously Creates a set of images from the specified wave stream using <see cref="MaxPeakProvider"/>.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns> 
        public static async Task<RenderResult> CreateAsync(WaveStream waveStream, Settings settings)
        {
            return await Task.Run(() =>
            {
                return Create(waveStream, new MaxPeakProvider(), settings);
            });
        }

        /// <summary>
        /// Asynchronously creates an image from the specified wave stream and the specified peak provider.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="peakProvider">The peak provider</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static async Task<RenderResult> CreateAsync(WaveStream waveStream, IPeakProvider peakProvider, Settings settings)
        {
            return await Task.Run(() =>
            {
                return Create(waveStream, peakProvider, settings);
            });
        }

        /// <summary>
        /// Creates a set of images from the specified wave stream using <see cref="MaxPeakProvider"/>.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(WaveStream waveStream, Settings settings)
        {
            return Create(waveStream, new MaxPeakProvider(), settings);
        }

        /// <summary>
        /// Creates an image from the specified wave stream and the specified peak provider.
        /// </summary>
        /// <param name="waveStream">The wave stream.</param>
        /// <param name="peakProvider">The peak provider</param>
        /// <param name="settings">The settings</param>
        /// <returns>A render result, images for left channel and right channel</returns>
        public static RenderResult Create(WaveStream waveStream, IPeakProvider peakProvider, Settings settings)
        {
            if (waveStream == null)
            {
                throw new ArgumentNullException(nameof(waveStream));
            }

            if (peakProvider == null)
            {
                throw new ArgumentNullException(nameof(peakProvider));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            int bytesPerSample = waveStream.WaveFormat.BitsPerSample / 8;
            long samples = waveStream.Length / bytesPerSample;
            int samplesPerPixel = Utility.GetEvenIntegerValue(Math.Max((int)(samples / settings.Width), 2));
            int stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
            peakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel * stepSize);
            return Create(peakProvider, settings);
        }

        private static RenderResult Create(IPeakProvider peakProvider, Settings settings)
        {
            if (settings.DecibelScale)
            {
                peakProvider = new DecibelPeakProvider(peakProvider, 48);
            }

            RenderResult result = new(settings.CreateBitmapImage(), settings.CreateBitmapImage(), peakProvider.Channels);

            using (Graphics gleft = Graphics.FromImage(result.ImageLeft))
            {
                using (Graphics gright = Graphics.FromImage(result.ImageRight))
                {
                    gleft.FillRectangle(settings.BackgroundBrush, 0, 0, result.ImageLeft.Width, result.ImageLeft.Height);
                    gright.FillRectangle(settings.BackgroundBrush, 0, 0, result.ImageRight.Width, result.ImageRight.Height);

                    int topStart = settings.TopHeight;
                    int bottomStart = settings.TopHeight + settings.CenterLineHeight + 1;

                    if (settings.CenterLineHeight > 0)
                    {
                        gleft.DrawLine(settings.CenterLinePen, 0, topStart + 1, settings.Width, topStart + 1);
                        gright.DrawLine(settings.CenterLinePen, 0, topStart + 1, settings.Width, topStart + 1);

                        if (settings.CenterLineHeight > 1)
                        {
                            gleft.DrawLine(settings.CenterLinePen, 0, topStart + 2, settings.Width, topStart + 2);
                            gright.DrawLine(settings.CenterLinePen, 0, topStart + 2, settings.Width, topStart + 2);

                        }
                    }

                    int x = 0;
                    PeakInfo currentPeak = peakProvider.GetNextPeak().ApplyNoiseThreshold(settings.NoiseThreshold);

                    while (x < settings.Width)
                    {
                        PeakInfo nextPeak = peakProvider.GetNextPeak().ApplyNoiseThreshold(settings.NoiseThreshold);

                        for (int n = 0; n < settings.PixelsPerPeak; n++)
                        {
                            float lineHeight = settings.TopHeight * currentPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Max);
                            gleft.DrawLine(settings.TopPeakPen, x, topStart, x, topStart - lineHeight);

                            lineHeight = settings.BottomHeight * currentPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Min);
                            gleft.DrawLine(settings.BottomPeakPen, x, bottomStart, x, bottomStart - lineHeight);

                            lineHeight = settings.TopHeight * currentPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Max);
                            gright.DrawLine(settings.TopPeakPen, x, topStart, x, topStart - lineHeight);

                            lineHeight = settings.BottomHeight * currentPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Min);
                            gright.DrawLine(settings.BottomPeakPen, x, bottomStart, x, bottomStart - lineHeight);
                            x++;
                        }

                        if (settings.SpacerPixels > 0)
                        {
                            float minLeft = Math.Max(currentPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Min), nextPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Min));
                            float maxLeft = Math.Min(currentPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Max), nextPeak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Max));

                            float minRight = Math.Max(currentPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Min), nextPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Min));
                            float maxRight = Math.Min(currentPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Max), nextPeak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Max));

                            for (int n = 0; n < settings.SpacerPixels; n++)
                            {
                                float lineHeight = settings.TopHeight * maxLeft;
                                gleft.DrawLine(settings.TopSpacerPen, x, topStart, x, topStart - lineHeight);

                                lineHeight = settings.BottomHeight * minLeft;
                                gleft.DrawLine(settings.BottomSpacerPen, x, bottomStart, x, bottomStart - lineHeight);

                                lineHeight = settings.TopHeight * maxRight;
                                gright.DrawLine(settings.TopSpacerPen, x, topStart, x, topStart - lineHeight);

                                lineHeight = settings.BottomHeight * minRight;
                                gright.DrawLine(settings.BottomSpacerPen, x, bottomStart, x, bottomStart - lineHeight);
                                x++;
                            }
                        }
                        currentPeak = nextPeak;
                    }
                }
            }
            return result;
        }
    }
}