using System.Drawing;

namespace Restless.WaveForm
{
    /// <summary>
    /// Represents settings for <see cref="BarRenderer"/>
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="Settings"/> to provide a set of default values
    /// that work decently with <see cref="BarRenderer"/>. You can still modify to customize the render result.
    /// </remarks>
    public class BarSettings : Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarSettings"/> class.
        /// </summary>
        public BarSettings()
        {
            DisplayName = "Bar";
            Height = 128;
            PrimaryLineColor = Color.Black;
            SecondaryLineColor = Color.Red;
            LineThickness = 4;
            CenterLineColor = Color.DarkSlateGray;
            CenterLineThickness = 1;
            ZoomX = 5;
        }

        /// <summary>
        /// Returns ab adjusted line thickness in response to a changed zoom x.
        /// </summary>
        /// <param name="actualZoomX">The actual zoom x</param>
        /// <param name="actualLineThickness">The cuurent actual line thickness</param>
        /// <returns>A new line thickness value.</returns>
        protected override int GetActualLineThickness(int actualZoomX, int actualLineThickness)
        {
            return actualZoomX - 1;
        }
    }
}