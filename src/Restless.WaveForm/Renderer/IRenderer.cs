﻿using NAudio.Wave;
using Restless.WaveForm.Settings;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Describes methods and properties that a renderer must implement
    /// </summary>
    public interface IRenderer
    {
        string DisplayName { get; }
        IRenderer Init(Image image, WaveStream stream, RenderSettings settings);
        void Render(Channel channel, Graphics graphics);
    }
}