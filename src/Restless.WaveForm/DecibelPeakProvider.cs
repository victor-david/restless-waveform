using System;
using NAudio.Wave;

namespace Restless.WaveForm
{
    internal class DecibelPeakProvider : IPeakProvider
    {
        #region Private
        private readonly IPeakProvider sourceProvider;
        private readonly double dynamicRange;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the number of channels (obtained from source provider)
        /// </summary>
        public int Channels => sourceProvider.Channels;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DecibelPeakProvider"/> class.
        /// </summary>
        /// <param name="sourceProvider">The source provider</param>
        /// <param name="dynamicRange">The dynamic range</param>
        public DecibelPeakProvider(IPeakProvider sourceProvider, double dynamicRange)
        {
            this.sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
            this.dynamicRange = dynamicRange;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Satisfies the <see cref="IPeakProvider"/> interface but always throws an exception.
        /// </summary>
        /// <param name="provider">The sample provider</param>
        /// <param name="samplesPerPeak">Number of samples per peak</param>
        public void Init(ISampleProvider provider, int samplesPerPeak)
        {
            throw new NotImplementedException();
        }

        public PeakInfo GetNextPeak()
        {
            PeakInfo peak = sourceProvider.GetNextPeak();
            double decibelMaxLeft = 20 * Math.Log10(peak.GetValue(PeakInfo.Channel.Left, PeakInfo.Value.Max));
            decibelMaxLeft = Math.Max(decibelMaxLeft, 0 - dynamicRange);

            double decibelMaxRight = 20 * Math.Log10(peak.GetValue(PeakInfo.Channel.Right, PeakInfo.Value.Max));
            decibelMaxRight = Math.Max(decibelMaxRight, 0 - dynamicRange);

            float linearLeft = (float)((dynamicRange + decibelMaxLeft) / dynamicRange);
            float linearRight = (float)((dynamicRange + decibelMaxRight) / dynamicRange);

            return new PeakInfo(0 - linearLeft, linearLeft, 0 - linearRight, linearRight);
        }
        #endregion
    }
}