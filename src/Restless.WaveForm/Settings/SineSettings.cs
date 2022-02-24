using System.Drawing;

namespace Restless.WaveForm.Settings
{
    /// <summary>
    /// Represents settings for <see cref="Renderer.SineRenderer"/>
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="RenderSettings"/> to provide a set of default values
    /// that work decently with <see cref="Renderer.SineRenderer"/>.
    /// </remarks>
    public class SineSettings : RenderSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SineSettings"/> class.
        /// </summary>
        public SineSettings()
        {
            DisplayName = "Sine";
            Height = 112;
            PrimaryLineColor = Color.Red;
            LineThickness = 1;
            CenterLineColor = Color.Black;
            ZoomX = 2;
        }
    }
}