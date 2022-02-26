using System.Drawing;

namespace Restless.WaveForm.Settings
{
    /// <summary>
    /// Represents settings for <see cref="Renderer.BarRenderer"/>
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="RenderSettings"/> to provide a set of default values
    /// that work decently with <see cref="Renderer.BarRenderer"/>.
    /// </remarks>
    public class BarSettings : RenderSettings
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BarSettings"/> class.
        /// </summary>
        public BarSettings()
        {
            DisplayName = "Standard Bar";
            Height = 128;
            PrimaryLineColor = Color.Black;
            SecondaryLineColor = Color.Red;
            LineThickness = 4;
            CenterLineColor = Color.DarkSlateGray;
            CenterLineThickness = 1;
            XStep = 5;
        }

        /// <summary>
        /// Returns an instance of the <see cref="BarSettings"/> using a default set of gray colors.
        /// </summary>
        /// <returns>An instance of <see cref="BarSettings"/></returns>
        public static BarSettings CreateFatGray()
        {
            return CreateFat("Fat Bar (Gray)", Color.FromArgb(52, 52, 52), Color.FromArgb(154, 154, 154));
        }

        /// <summary>
        /// Returns an instance of the <see cref="BarSettings"/> using a default set of orange colors.
        /// </summary>
        /// <returns>An instance of <see cref="BarSettings"/></returns>
        public static BarSettings CreateFatOrange()
        {
            return CreateFat("Fat Bar (Orange)", Color.FromArgb(255, 76, 0), Color.FromArgb(255, 171, 141));
        }

        private static BarSettings CreateFat(string displayName, Color primary, Color secondary)
        {
            return new BarSettings()
            {
                DisplayName = displayName,
                Height = 172,
                PrimaryLineColor = primary,
                SecondaryLineColor = secondary,
                LineThickness = 7,
                CenterLineThickness = 0,
                XStep = 8,
                ScaleXStep = false
            };
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Returns an adjusted line thickness in response to a changed zoom x.
        /// </summary>
        /// <param name="actualZoomX">The actual zoom x</param>
        /// <param name="actualLineThickness">The cuurent actual line thickness</param>
        /// <returns>A new line thickness value.</returns>
        protected override float GetActualLineThickness(float actualZoomX, float actualLineThickness)
        {
            return actualZoomX - 1f;
        }
        #endregion
    }
}