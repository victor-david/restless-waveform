using System.Drawing;

namespace Restless.WaveForm
{
    /// <summary>
    /// Represents settings for <see cref="SineRenderer"/>
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="Settings"/> to provide a set of default values
    /// that work decently with <see cref="SineRenderer"/>. You can still modify to customize the render result.
    /// </remarks>
    public class SineSettings : Settings
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