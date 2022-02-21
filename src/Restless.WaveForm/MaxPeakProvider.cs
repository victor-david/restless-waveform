using System.Diagnostics;
using System.Linq;

namespace Restless.WaveForm
{
    public class MaxPeakProvider : PeakProvider
    {
        public MaxPeakProvider()
        {
            DisplayName = "Max Absolute Value";
        }

        public override PeakInfo GetNextPeak()
        {
            int samplesRead = ReadChannelSamples();
            return new PeakInfo
                (
                    samplesRead == 0 ? 0 : Left.Take(samplesRead).Min(),
                    samplesRead == 0 ? 0 : Left.Take(samplesRead).Max(),
                    samplesRead == 0 ? 0 : Right.Take(samplesRead).Min(),
                    samplesRead == 0 ? 0 : Right.Take(samplesRead).Max()
                );
        }

        public override string ToString()
        {
            return nameof(MaxPeakProvider);
        }
    }
}