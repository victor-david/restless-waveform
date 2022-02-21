using NAudio.Wave;

namespace Restless.WaveForm
{
    public interface IPeakProvider
    {
        int Channels { get; }
        void Init(ISampleProvider reader, int samplesPerPixel);
        PeakInfo GetNextPeak();
    }
}