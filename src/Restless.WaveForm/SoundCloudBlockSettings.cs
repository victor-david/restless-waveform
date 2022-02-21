using System.Drawing;

namespace Restless.WaveForm
{
    /// <summary>
    /// Provides settings for SoundCloud block style.
    /// </summary>
    public class SoundCloudBlockSettings : Settings
    {
        #region Private
        private readonly Color topSpacerEndColor;
        private int lastTopHeight;
        #endregion

        /************************************************************************/

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundCloudBlockSettings"/> class using the specified colors.
        /// </summary>
        /// <param name="topPeakColor">The top peak color.</param>
        /// <param name="topSpacerEndColor">The top spacer end color</param>
        /// <param name="bottomPeakColor">The bottom peak color</param>
        /// <param name="spacerColor">The spacer color</param>
        public SoundCloudBlockSettings(Color topPeakColor, Color topSpacerEndColor, Color bottomPeakColor, Color spacerColor)
        {
            DisplayName = "SoundCloud Block";
            this.topSpacerEndColor = topSpacerEndColor;
            TopPeakPen = new Pen(topPeakColor);
            TopSpacerPen = new Pen(spacerColor);
            BottomPeakPen = new Pen(bottomPeakColor);
            BottomSpacerPen = new Pen(spacerColor);
            PixelsPerPeak = 4;
            SpacerPixels = 2;
        }

        /// <summary>
        /// Returns an instance of the <see cref="SoundCloudBlockSettings"/> using a default set of light colors.
        /// </summary>
        /// <returns>An instance of <see cref="SoundCloudBlockSettings"/></returns>
        public static SoundCloudBlockSettings CreateLight()
        {
            return new SoundCloudBlockSettings(Color.FromArgb(102, 102, 102), Color.FromArgb(103, 103, 103), Color.FromArgb(179, 179, 179), Color.FromArgb(218, 218, 218))
            {
                DisplayName = "SoundCloud Block (Light)"
            };
        }

        /// <summary>
        /// Returns an instance of the <see cref="SoundCloudBlockSettings"/> using a default set of dark colors.
        /// </summary>
        /// <returns>An instance of <see cref="SoundCloudBlockSettings"/></returns>
        public static SoundCloudBlockSettings CreateDark()
        {
            return new SoundCloudBlockSettings(Color.FromArgb(52, 52, 52), Color.FromArgb(55, 55, 55), Color.FromArgb(154, 154, 154), Color.FromArgb(204, 204, 204))
            {
                DisplayName = "SoundCloud Block (Dark)"
            };

        }

        /// <summary>
        /// Returns an instance of the <see cref="SoundCloudBlockSettings"/> using a default set of orange colors.
        /// </summary>
        /// <returns>An instance of <see cref="SoundCloudBlockSettings"/></returns>
        public static SoundCloudBlockSettings CreateOrange()
        {
            return new SoundCloudBlockSettings(Color.FromArgb(255, 76, 0), Color.FromArgb(255, 52, 2), Color.FromArgb(255, 171, 141), Color.FromArgb(255, 213, 199))
            {
                DisplayName = "SoundCloud Block (Orange)"
            };
        }
        #endregion

        protected override void OnHeightSet()
        {
            // Commented out for now because gradient brush isn't working right
            //if (lastTopHeight != TopHeight)
            //{
            //    TopSpacerPen = CreateGradientPen(TopHeight, Color.White, topSpacerEndColor);
            //    lastTopHeight = TopHeight;
            //}
        }
    }
}