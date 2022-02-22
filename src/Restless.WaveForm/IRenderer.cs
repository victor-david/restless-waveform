using NAudio.Wave;
using System.Drawing;

namespace Restless.WaveForm
{
    /// <summary>
    /// Describes methods and properties that a renderer must implement
    /// </summary>
    public interface IRenderer
    {
        string DisplayName { get; }
        IRenderer Init(Image image, WaveStream stream, Settings settings);
        void Render(Channel channel, Graphics graphics);
    }
}
