using System;
using System.Linq;

namespace Restless.WaveForm
{
    public class AveragePeakProvider : PeakProvider
    {
        #region Private
        private readonly float scale;
        #endregion

        /************************************************************************/

        #region Public fields
        public const float MinScale = 1.0f;
        public const float MaxScale = 32.0f;
        public const float DefaultScale = 8.0f;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AveragePeakProvider"/> class with <see cref="DefaultScale"/>.
        /// </summary>
        public AveragePeakProvider() : this(DefaultScale)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AveragePeakProvider"/> class with the specified scale.
        /// </summary>
        /// <param name="scale">
        /// The scale. This value is clamped between <see cref="MinScale"/> and <see cref="MaxScale"/>.
        /// </param>
        public AveragePeakProvider(float scale)
        {
            DisplayName = "Scaled Average";
            this.scale = Math.Max(Math.Min(scale, MaxScale), MinScale);
        }
        #endregion

        /************************************************************************/

        #region Public methods
        public override PeakInfo GetNextPeak()
        {
            int samplesRead = ReadChannelSamples();
            float sumLeft = samplesRead == 0 ? 0 : Left.Take(samplesRead).Select(s => Math.Abs(s)).Sum();
            float sumRight = samplesRead == 0 ? 0 : Right.Take(samplesRead).Select(s => Math.Abs(s)).Sum();
            float averageLeft = sumLeft / samplesRead;
            float averageRight = sumRight / samplesRead;
            return new PeakInfo(averageLeft * -scale, averageLeft * scale, averageRight * -scale, averageRight * scale);
        }

        public override string ToString()
        {
            return nameof(AveragePeakProvider);
        }
        #endregion
    }
}