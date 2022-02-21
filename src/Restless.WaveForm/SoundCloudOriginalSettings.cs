using System.Drawing;
using System.Drawing.Drawing2D;

namespace Restless.WaveForm
{
    /// <summary>
    /// Provides settings for SoundCloud original style.
    /// </summary>
    public class SoundCloudOriginalSettings : Settings
    {
        #region Private
        private int lastTopHeight;
        private int lastBottomHeight;
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundCloudOriginalSettings"/> class.
        /// </summary>
        public SoundCloudOriginalSettings()
        {
            DisplayName = "SoundCloud (Original)";
            PixelsPerPeak = MinPixelsPerPeak;
            SpacerPixels = MinSpacerPixels;
        }
        #endregion

        /************************************************************************/

        #region Protected methods

        protected override void OnHeightSet()
        {
            if (lastTopHeight != TopHeight)
            {
                TopPeakPen = CreateGradientPen(TopHeight, Color.FromArgb(120, 120, 120), Color.FromArgb(50, 50, 50));
                lastTopHeight = TopHeight;
            }

            if (lastBottomHeight != BottomHeight || lastTopHeight != TopHeight)
            {
                BottomPeakPen = CreateSoundCloudBottomPen();
                lastBottomHeight = BottomHeight;
                lastTopHeight = TopHeight;
            }
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private Pen CreateSoundCloudBottomPen()
        {
            LinearGradientBrush brush = new
                 (
                     new Point(0, TopHeight + CenterLineHeight + 1),
                     new Point(0, TopHeight + BottomHeight + CenterLineHeight + 1),
                     Color.FromArgb(16, 16, 16),
                     Color.FromArgb(150, 150, 150)
                 );
            ColorBlend colorBlend = new(3);
            colorBlend.Colors[0] = Color.FromArgb(16, 16, 16);
            colorBlend.Colors[1] = Color.FromArgb(142, 142, 142);
            colorBlend.Colors[2] = Color.FromArgb(150, 150, 150);
            colorBlend.Positions[0] = 0;
            colorBlend.Positions[1] = 0.1f;
            colorBlend.Positions[2] = 1.0f;
            brush.InterpolationColors = colorBlend;
            return new Pen(brush);
        }
        #endregion
    }
}