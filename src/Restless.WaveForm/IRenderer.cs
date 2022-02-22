using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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
