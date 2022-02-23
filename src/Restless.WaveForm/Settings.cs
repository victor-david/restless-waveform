using System;
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
        private int width;
        private int height;
        private int sampleResolution;
        private int zoomX;
        private float volumeBoost;
        private int lineThickness;
        private int actualLineThickness;
        private int centerLineThickness;

        private int pixelsPerPeak;
        private int spacerPixels;

        private float noiseThreshold;
        private Pen topPeakPen;
        private Pen topSpacerPen;
        private Pen bottomPeakPen;
        private Pen bottomSpacerPen;
        #endregion

        /************************************************************************/

        #region Public consts
        public const int MinWidth = 800;
        public const int MaxWidth = 36000;
        public const int DefaultWidth = 1200;
        public const bool DefaultAutoWidth = true;

        public const int MinHeight = 32;
        public const int MaxHeight = 228;
        public const int DefaultHeight = 76;

        public const int MinSampleResolution = 2;
        public const int MaxSampleResolution = 192;
        public const int DefaultSampleResolution = 8;

        public const int MinZoomX = 1;
        public const int MaxZoomX = 16;
        public const int DefaultZoomX = 4;

        public const float MinVolumeBoost = 1;
        public const float MaxVolumeBoost = 9.5f;
        public const float DefaultVolumeBoost = 1;

        public const int MinLineThickness = 1;
        public const int MaxLineThickness = 8;
        public const int DefaultLineThickness = 1;

        public const int MinCenterLineThickness = 0;
        public const int MaxCenterLineThickness = 5;
        public const int DefaultCenterLineThickness = 1;

        public const int MinPixelsPerPeak = 2;
        public const int MaxPixelsPerPeak = 16;
        public const int DefaultPixelsPerPeak = MinPixelsPerPeak;

        public const int MinSpacerPixels = 0;
        public const int MaxSpacerPixels = 10;
        public const int DefaultSpacerPixels = MinSpacerPixels;

        public const float DefaultNoiseThreshold = 0.001f;
        #endregion

        /************************************************************************/

        #region Properties (Dimensions)
        /// <summary>
        /// Gets or sets the display name of the settings
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the width of the image
        /// This value is clamped between <see cref="MinWidth"/> and <see cref="MaxWidth"/>.
        /// </summary>
        public int Width
        {
            get => width;
            set => width = Utility.Clamp(value, MinWidth, MaxWidth);
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the image width is automatically
        /// set according to the audio being rendered.
        /// </summary>
        public bool AutoWidth
        {
            get;
            set;
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
                height = Utility.ClampEven(value, MinHeight, MaxHeight);
                OnHeightSet();
            }
        }

        /// <summary>
        /// Gets or sets the desired sample resolution
        /// </summary>
        public int SampleResolution
        {
            get => sampleResolution;
            set => sampleResolution = Utility.ClampEven(value, MinSampleResolution, MaxSampleResolution);
        }

        /// <summary>
        /// Gets the actual sample resolution
        /// </summary>
        public int ActualSampleResolution
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the zoom X factor.
        /// This value is clamped between <see cref="MinZoomX"/> and <see cref="MaxZoomX"/>.
        /// </summary>
        public int ZoomX
        {
            get => zoomX;
            set => zoomX = Utility.Clamp(value, MinZoomX, MaxZoomX);
        }

        /// <summary>
        /// Gets the actual zoom X factor
        /// </summary>
        public int ActualZoomX
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value that boost the pparent volume of the rendered audio.
        /// </summary>
        public float VolumeBoost
        {
            get => volumeBoost;
            set => volumeBoost = Utility.Clamp(value, MinVolumeBoost, MaxVolumeBoost);
        }

        /// <summary>
        /// Gets or sets the line thickness.
        /// This value is clamped between <see cref="MinLineThickness"/> and <see cref="MaxLineThickness"/>.
        /// </summary>
        public int LineThickness
        {
            get => lineThickness;
            set => lineThickness = Utility.Clamp(value, MinLineThickness, MaxLineThickness);
        }

        /// <summary>
        /// Gets the actual line thickness.
        /// </summary>
        public int ActualLineThickness
        {
            get => actualLineThickness;
            private set => actualLineThickness = Utility.Clamp(value, MinLineThickness, MaxLineThickness);
        }

        /// <summary>
        /// Gets or sets the center line height.
        /// This value is clamped between <see cref="MinCenterLineThickness"/> and <see cref="MaxCenterLineThickness"/>.
        /// </summary>
        public int CenterLineThickness
        {
            get => centerLineThickness;
            set => centerLineThickness = Utility.Clamp(value, MinCenterLineThickness, MaxCenterLineThickness);
        }
        #endregion

        /************************************************************************/

        #region Properties (Colors)
        /// <summary>
        /// Gets or sets the background color. Default is transparent.
        /// </summary>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the primary line color.
        /// </summary>
        public Color PrimaryLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the secondary line color.
        /// </summary>
        public Color SecondaryLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the center line color.
        /// </summary>
        public Color CenterLineColor
        {
            get;
            set;
        }
        #endregion


        /************************************************************************/


        #region Properties (others)
        /// <summary>
        /// Gets or sets the number of pixels used to display a peak
        /// This value is clamped between <see cref="MinPixelsPerPeak"/> and <see cref="MaxPixelsPerPeak"/>.
        /// </summary>
        public int PixelsPerPeak
        {
            get => pixelsPerPeak;
            set => pixelsPerPeak = Utility.ClampEven(value, MinPixelsPerPeak, MaxPixelsPerPeak);
        }

        /// <summary>
        /// Gets or sets the number of pixels between peaks.
        /// This value is clamped between <see cref="MinSpacerPixels"/> and <see cref="MaxSpacerPixels"/>.
        /// </summary>
        public int SpacerPixels
        {
            get => spacerPixels;
            set => spacerPixels = Utility.ClampEven(value, MinSpacerPixels, MaxSpacerPixels);
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
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        protected Settings()
        {
            DisplayName = "Default";

            height = DefaultHeight;
            Width = DefaultWidth;
            AutoWidth = DefaultAutoWidth;

            SampleResolution = ActualSampleResolution = DefaultSampleResolution;
            ZoomX = ActualZoomX = DefaultZoomX;
            VolumeBoost = DefaultVolumeBoost;
            LineThickness = ActualLineThickness = DefaultLineThickness;
            CenterLineThickness = DefaultCenterLineThickness;

            BackgroundColor = Color.Transparent;
            PrimaryLineColor = Color.Blue;
            SecondaryLineColor = Color.LightSlateGray;
            CenterLineColor = Color.DarkBlue;

            PixelsPerPeak = DefaultPixelsPerPeak;
            SpacerPixels = DefaultSpacerPixels;

            TopPeakPen = Pens.CadetBlue;
            BottomPeakPen = Pens.DodgerBlue;
            TopSpacerPen = Pens.Yellow;
            BottomSpacerPen = Pens.Yellow;
            
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
            // max int:              2,147,483,647
            // max long: 9,223,372,036,854,775,807
            ActualSampleResolution = SampleResolution;
            ActualZoomX = ZoomX;
            ActualLineThickness = LineThickness;
            PrepareForImageWidth(sampleCount, channels);
            long autoWidth = GetClampedAutoImageWidth(sampleCount, channels);

            Bitmap bitmap = new((int)autoWidth, (Height * 2) + CenterLineThickness);
            if (BackgroundColor == Color.Transparent)
            {
                bitmap.MakeTransparent();
            }
            return bitmap;
        }

        /// <summary>
        /// Gets the background brush according to the current settings.
        /// </summary>
        /// <returns></returns>
        public Brush GetBackgroundBrush()
        {
            return new SolidBrush(BackgroundColor);
        }

        /// <summary>
        /// Gets a pen
        /// </summary>
        /// <param name="type">The type of pen</param>
        /// <returns>A pen</returns>
        public Pen GetPen(PenType type)
        {
            return type switch
            {
                PenType.PrimaryLine => new Pen(PrimaryLineColor, ActualLineThickness),
                PenType.SecondaryLine => new Pen(SecondaryLineColor, ActualLineThickness),
                PenType.CenterLine => new Pen(CenterLineColor, CenterLineThickness),
                _ => throw new ArgumentException(nameof(type)),
            };
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

        /// <summary>
        /// Gets a value for <see cref="ActualLineThickness"/>
        /// </summary>
        /// <param name="actualZoomX">The actual zoom x value</param>
        /// <param name="actualLineThickness">The current actual line thickness</param>
        /// <returns>The new actual line thickness</returns>
        /// <remarks>
        /// This method is called during sizing of the image width to enable a derived class
        /// to adjust the actual line thickness in response to a smaller x zoom value.
        /// The base method returns the same line thickness as passed.
        /// </remarks>
        protected virtual int GetActualLineThickness(int actualZoomX, int actualLineThickness)
        {
            return actualLineThickness;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void PrepareForImageWidth(long sampleCount, int channels)
        {
            int maxWidth = AutoWidth ? MaxWidth : Width;
            long width = GetUnClampedAutoImageWidth(sampleCount, channels);
            while (width > maxWidth && ActualZoomX > MinZoomX)
            {
                ActualZoomX--;
                ActualLineThickness = GetActualLineThickness(ActualZoomX, ActualLineThickness);
                width = GetUnClampedAutoImageWidth(sampleCount, channels);
            }

            while (width > maxWidth)
            {
                ActualSampleResolution += 2;
                width = GetUnClampedAutoImageWidth(sampleCount, channels);
            }
        }

        private long GetUnClampedAutoImageWidth(long sampleCount, int channels)
        {
            return Utility.GetEven(Math.Min(sampleCount, int.MaxValue) / channels) / ActualSampleResolution * ActualZoomX;
        }

        private long GetClampedAutoImageWidth(long sampleCount, int channels)
        {
            int maxWidth = AutoWidth ? MaxWidth : Width;
            return Math.Min(Utility.GetEven(Math.Min(sampleCount, int.MaxValue) / channels) / ActualSampleResolution * ActualZoomX, maxWidth);
        }

        private Pen GetPenPropertyValue(Pen desiredPen)
        {
            return desiredPen ?? Pens.Transparent;
        }
        #endregion
    }
}