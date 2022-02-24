using NAudio.Wave;
using Restless.WaveForm.Calculators;
using Restless.WaveForm.Settings;
using System.Drawing;

namespace Restless.WaveForm.Renderer
{
    /// <summary>
    /// Describes methods and properties that a renderer must implement
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// Initializes the renderer
        /// </summary>
        /// <param name="image">The image on which to render</param>
        /// <param name="stream">The wave stream</param>
        /// <param name="calculator">The sample calculator</param>
        /// <param name="settings">Settings</param>
        /// <returns>An instance of of <see cref="IRenderer"/>.</returns>
        IRenderer Init(Image image, WaveStream stream, ISampleCalculator calculator, RenderSettings settings);
        /// <summary>
        /// Performs the render operation
        /// </summary>
        /// <param name="channel">The channel to render</param>
        /// <param name="graphics">The graphics object to draw the render</param>
        void Render(Channel channel, Graphics graphics);
    }
}