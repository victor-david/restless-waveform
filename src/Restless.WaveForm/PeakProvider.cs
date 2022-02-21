using NAudio.Wave;
using System;

namespace Restless.WaveForm
{
    public abstract class PeakProvider : IPeakProvider
    {
        #region Private
        private float[] rawReadBuffer;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the sample provider
        /// </summary>
        protected ISampleProvider Provider
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the samples per peak
        /// </summary>
        protected int SamplesPerPeak
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of channels
        /// </summary>
        public int Channels => Provider != null ? Provider.WaveFormat.Channels : 0;

        /// <summary>
        /// Gets the left channel buffer
        /// </summary>
        protected float[] Left
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the right channel buffer
        /// </summary>
        protected float[] Right
        {
            get;
            private set;
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PeakProvider"/> class.
        /// </summary>
        public PeakProvider()
        {
        }
        #endregion

        /************************************************************************/

        #region Public methods

        public abstract PeakInfo GetNextPeak();

        public void Init(ISampleProvider provider, int samplesPerPeak)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            SamplesPerPeak = samplesPerPeak;
            rawReadBuffer = new float[samplesPerPeak];
            Left = new float[samplesPerPeak];
            Right = new float[samplesPerPeak];
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected int ReadChannelSamples()
        {
            ClearSampleBuffers();
            int samplesRead = Provider.Read(rawReadBuffer, 0, rawReadBuffer.Length);

            if (Channels == 1)
            {
                for (int idx = 0; idx < samplesRead; idx++)
                {
                    Left[idx] = Right[idx] = rawReadBuffer[idx];
                }
            }
            else
            {
                for (int idx = 0; idx < samplesRead - 2; idx += 2)
                {
                    Left[idx] = rawReadBuffer[idx];
                    Right[idx] = rawReadBuffer[idx + 1];
                }
            }
            return samplesRead;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void ClearSampleBuffers()
        {
            for (int idx = 0; idx < rawReadBuffer.Length; idx++)
            {
                Left[idx] = Right[idx] = rawReadBuffer[idx] = 0;
            }
        }
        #endregion
    }
}