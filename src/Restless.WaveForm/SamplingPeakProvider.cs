using System;

namespace Restless.WaveForm
{
    public class SamplingPeakProvider : PeakProvider
    {
        #region Private
        private readonly int sampleInterval;
        #endregion

        /************************************************************************/

        #region Public fields
        public const int MinSampleInterval = 2;
        public const int MaxSampleInterval = 128;
        public const int DefaultSampleInterval = 32;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingPeakProvider"/> class with <see cref="DefaultSampleInterval"/>.
        /// </summary>
        public SamplingPeakProvider() : this(DefaultSampleInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplingPeakProvider"/> class with the specified sampling interval
        /// </summary>
        /// <param name="sampleInterval">
        /// The interval. Clamped between <see cref="MinSampleInterval"/> and <see cref="MaxSampleInterval"/>
        /// </param>
        public SamplingPeakProvider(int sampleInterval) 
        {
            DisplayName = "Sampled Peaks";
            this.sampleInterval = Utility.ClampEven(sampleInterval, MinSampleInterval, MaxSampleInterval);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public override PeakInfo GetNextPeak()
        {
            int samplesRead = ReadChannelSamples();
            float minLeft = 0.0f;
            float maxLeft = 0.0f;
            float minRight = 0.0f;
            float maxRight = 0.0f;

            for (int x = 0; x < samplesRead; x += sampleInterval)
            {
                minLeft = Math.Min(minLeft, Left[x]);
                maxLeft = Math.Max(maxLeft, Left[x]);
                minRight = Math.Min(minRight, Right[x]); 
                maxRight = Math.Max(maxRight, Right[x]);
            }

            return new PeakInfo(minLeft, maxLeft, minRight, maxRight);
        }

        public override string ToString()
        {
            return nameof(SamplingPeakProvider);
        }
        #endregion
    }
}