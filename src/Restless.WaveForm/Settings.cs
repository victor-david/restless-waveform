using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Restless.WaveForm
{
    /// <summary>
    /// Provides settings used when rendering a waveform.
    /// This class must be inherited.
    /// </summary>
    public abstract class Settings
    {
        #region Private
        private int maxWidth;
        private int height;
        private int sampleResolution;
        private int zoomX;
        private int pixelsPerPeak;
        private int spacerPixels;
        private int centerLineHeight;
        private float noiseThreshold;
        private Pen topPeakPen;
        private Pen topSpacerPen;
        private Pen bottomPeakPen;
        private Pen bottomSpacerPen;
        private Pen centerLinePen;
        private Color centerLineColor;
        #endregion

        /************************************************************************/

        #region Public consts
        public const int MinMaxWidth = 0;
        public const int MaxMaxWidth = int.MaxValue;
        public const int DefaultMaxWidth = 0;

        public const int MinHeight = 32;
        public const int MaxHeight = 228;
        public const int DefaultHeight = 76;

        public const int MinSampleResolution = 1;
        public const int MaxSampleResolution = 128;
        public const int DefaultSampleResolution = 8;

        public const int MinZoomX = 1;
        public const int MaxZoomX = 16;
        public const int DefaultZoomX = 4;

        public const int MinPixelsPerPeak = 2;
        public const int MaxPixelsPerPeak = 16;
        public const int DefaultPixelsPerPeak = MinPixelsPerPeak;

        public const int MinSpacerPixels = 0;
        public const int MaxSpacerPixels = 10;
        public const int DefaultSpacerPixels = MinSpacerPixels;

        public const int MinCenterLineHeight = 0;
        public const int MaxCenterLineHeight = 5;
        public const int DefaultCenterLineHeight = 1;

        public const float DefaultNoiseThreshold = 0.001f;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets or sets the display name of the settings
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the maxium width of the image, zero = no maximum
        /// This value is clamped between <see cref="MinMaxWidth"/> and <see cref="MaxMaxWidth"/>.
        /// </summary>
        public int MaxWidth
        {
            get => maxWidth;
            set => maxWidth = Utility.Clamp(value, MinMaxWidth, MaxMaxWidth);
        }

        /// <summary>
        /// Gets or sets the height of the each portion of the waveform.
        /// This value is clamped between <see cref="MinHeight"/> and <see cref="MaxHeight"/>.
        /// </summary>
        public int Height
        {
            get => height;
            set
            {
                height = Utility.GetEvenValue(value, MinHeight, MaxHeight);
                OnHeightSet();
            }
        }

        /// <summary>
        /// Gets or sets the sample resolution
        /// </summary>
        public int SampleResolution
        {
            get => sampleResolution;
            set => sampleResolution = Utility.GetEvenValue(value, MinSampleResolution, MaxSampleResolution);
        }

        /// <summary>
        /// Gets or sets the zoom X factor
        /// </summary>
        public int ZoomX
        {
            get => zoomX;
            set => zoomX = Utility.Clamp(value, MinZoomX, MaxZoomX);
        }

        /// <summary>
        /// Gets or sets the number of pixels used to display a peak
        /// This value is clamped between <see cref="MinPixelsPerPeak"/> and <see cref="MaxPixelsPerPeak"/>.
        /// </summary>
        public int PixelsPerPeak
        {
            get => pixelsPerPeak;
            set => pixelsPerPeak = Utility.GetEvenValue(value, MinPixelsPerPeak, MaxPixelsPerPeak);
        }

        /// <summary>
        /// Gets or sets the number of pixels between peaks.
        /// This value is clamped between <see cref="MinSpacerPixels"/> and <see cref="MaxSpacerPixels"/>.
        /// </summary>
        public int SpacerPixels
        {
            get => spacerPixels;
            set => spacerPixels = Utility.GetEvenValue(value, MinSpacerPixels, MaxSpacerPixels);
        }

        public Color CenterLineColor
        {
            get => centerLineColor;
            set
            {
                centerLineColor = value;
                CenterLinePen = new Pen(centerLineColor, centerLineHeight);
            }
        }

        /// <summary>
        /// Gets or sets the center line height.
        /// This value is clamped between <see cref="MinCenterLineHeight"/> and <see cref="MaxCenterLineHeight"/>.
        /// </summary>
        public int CenterLineHeight
        {
            get => centerLineHeight;
            set
            {
                centerLineHeight = Utility.Clamp(value, MinCenterLineHeight, MaxCenterLineHeight);
                CenterLinePen = new Pen(centerLineColor, centerLineHeight);
            }
        }

        /// <summary>
        /// Gets or sets the noise threshold.
        /// Absolute values less than this value are converted to zero.
        /// </summary>
        public float NoiseThreshold
        {
            get => noiseThreshold;
            set
            {
                if (value > 0.0)
                {
                    noiseThreshold = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that determines if scale is based upon decibels.
        /// </summary>
        public bool DecibelScale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pen to use for top peaks.
        /// </summary>
        public Pen TopPeakPen
        {
            get => topPeakPen;
            set => topPeakPen = GetPenPropertyValue(value);
        }

        /// <summary>
        /// Gets or sets the pen to use for top spacers.
        /// </summary>
        public Pen TopSpacerPen
        {
            get => topSpacerPen;
            set => topSpacerPen = GetPenPropertyValue(value);
        }

        /// <summary>
        /// Gets or sets the pen to use for bottom peaks.
        /// </summary>
        public Pen BottomPeakPen
        {
            get => bottomPeakPen;
            set => bottomPeakPen = GetPenPropertyValue(value);
        }

        /// <summary>
        /// Gets or sets the pen to use for bottom spacers.
        /// </summary>
        public Pen BottomSpacerPen
        {
            get => bottomSpacerPen;
            set => bottomSpacerPen = GetPenPropertyValue(value);
        }

        /// <summary>
        /// Gets or sets the pen to use for the center line.
        /// </summary>
        public Pen CenterLinePen
        {
            get => centerLinePen;
            private set => centerLinePen = GetPenPropertyValue(value);
        }

        /// <summary>
        /// Gets or sets the background color. Default is transparent.
        /// </summary>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an image to use for the background. Default is null.
        /// </summary>
        public Image BackgroundImage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the brush to use for the background.
        /// </summary>
        public Brush BackgroundBrush => BackgroundImage == null ? new SolidBrush(BackgroundColor) : new TextureBrush(BackgroundImage, WrapMode.Clamp);
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        protected Settings()
        {
            height = DefaultHeight;
            MaxWidth = DefaultMaxWidth;
            SampleResolution = DefaultSampleResolution;
            ZoomX = DefaultZoomX;

            PixelsPerPeak = DefaultPixelsPerPeak;
            SpacerPixels = DefaultSpacerPixels;
            CenterLineHeight = DefaultCenterLineHeight;
            BackgroundColor = Color.Transparent;
            TopPeakPen = Pens.CadetBlue;
            BottomPeakPen = Pens.DodgerBlue;
            TopSpacerPen = Pens.Yellow;
            BottomSpacerPen = Pens.Yellow;

            CenterLineColor = Color.DarkBlue;
            CenterLineHeight = DefaultCenterLineHeight;
            
            NoiseThreshold = DefaultNoiseThreshold;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Creates a bit map object according to the current settings
        /// </summary>
        /// <returns>A Bitmap object sized according to current settings and made transparent if needed</returns>
        public Bitmap CreateBitmapImage(long sampleCount, int channels)
        {
            // 2,147,483,647 - max int
            long autoWidth = Utility.GetEvenValue(Math.Min(sampleCount, int.MaxValue) / channels) / SampleResolution * ZoomX;

            Debug.WriteLine($"Width: {autoWidth}");


            Bitmap bitmap = new((int)autoWidth, (Height * 2) + CenterLineHeight);
            if (BackgroundColor == Color.Transparent)
            {
                bitmap.MakeTransparent();
            }
            return bitmap;
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when either <see cref="Height"/> or <see cref="BottomHeight"/> is set.
        /// </summary>
        /// <remarks>
        /// Override if you need to take action on height changes.
        /// The base implementation does nothing.
        /// </remarks>
        protected virtual void OnHeightSet()
        {
        }

        /// <summary>
        /// From a derived class, creates a pen
        /// </summary>
        /// <param name="height">Height</param>
        /// <param name="startColor">Starting color</param>
        /// <param name="endColor">Ending color</param>
        /// <returns>A gradient pen</returns>
        protected Pen CreateGradientPen(int height, Color startColor, Color endColor)
        {
            LinearGradientBrush brush = new(new Point(0, 0), new Point(0, height), startColor, endColor);
            return new Pen(brush);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private Pen GetPenPropertyValue(Pen desiredPen)
        {
            return desiredPen ?? Pens.Transparent;
        }
        #endregion
    }
}