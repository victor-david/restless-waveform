using System;

namespace Restless.WaveForm
{
    public class RmsPeakProvider : PeakProvider
    {
        #region Private
        private readonly int blockSize;
        #endregion

        public const int DefaultBlockSize = 128;

        /// <summary>
        /// Initializes a new instance of the <see cref="RmsPeakProvider"/> class with <see cref="DefaultBlockSize"/>.
        /// </summary>
        public RmsPeakProvider() : this(DefaultBlockSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RmsPeakProvider"/> class with the specified block size.
        /// </summary>
        /// <param name="blockSize">Desired block size</param>
        public RmsPeakProvider(int blockSize)
        {
            DisplayName = "Max RMS Value";
            this.blockSize = blockSize;
        }

        public override PeakInfo GetNextPeak()
        {
            int samplesRead = ReadChannelSamples();

            float maxLeft = 0.0f;
            float maxRight = 0.0f;

            for (int x = 0; x < samplesRead; x += blockSize)
            {
                double totalLeft = 0.0;
                double totalRight = 0.0;

                for (int y = 0; y < blockSize && x + y < samplesRead; y++)
                {
                    totalLeft += Left[x + y] * Left[x + y];
                    totalRight += Right[x + y] * Right[x + y];
                }

                float rmsLeft = (float)Math.Sqrt(totalLeft / blockSize);
                float rmsRight = (float)Math.Sqrt(totalRight / blockSize);

                maxLeft = Math.Max(maxLeft, rmsLeft);
                maxRight = Math.Max(maxRight, rmsRight);
            }

            return new PeakInfo(0 - maxLeft, maxLeft, 0 - maxRight, maxRight);
        }

        public override string ToString()
        {
            return nameof(RmsPeakProvider);
        }
    }
}