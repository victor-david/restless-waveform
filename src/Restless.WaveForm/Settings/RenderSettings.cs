using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Restless.WaveForm.Settings
{
    /// <summary>
    /// Provides settings used when rendering a waveform.
    /// This class must be inherited.
    /// </summary>
    public abstract class RenderSettings
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
        private float sampleThreshold;
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

        public const float DefaultSampleThreshold = 0.001f;
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
        /// Gets or sets a value that determines if <see cref="ActualZoomX"/>
        /// is adjusted when creating / scaling the image used for audio render.
        /// The default value is true.
        /// </summary>
        /// <remarks>
        /// If this property is false, <see cref="ActualZoomX"/> will not be affected
        /// during the image preparation operation. Only <see cref="ActualSampleResolution"/>
        /// will be changed if needed.
        /// </remarks>
        protected bool ScaleZoomX
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that boost the apparent volume of the rendered audio.
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
        /// Gets or sets a value that determines if <see cref="SampleThreshold"/> is applied.
        /// The default value is false.
        /// </summary>
        public bool UseSampleThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sample threshold. If <see cref="UseSampleThreshold"/>
        /// is true, absolute values less than this value are set to zero.
        /// </summary>
        public float SampleThreshold
        {
            get => sampleThreshold;
            set
            {
                if (value > 0)
                {
                    sampleThreshold = value;
                }
            }
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderSettings"/> class.
        /// </summary>
        protected RenderSettings()
        {
            DisplayName = "Default";

            height = DefaultHeight;
            Width = DefaultWidth;
            AutoWidth = DefaultAutoWidth;

            SampleResolution = ActualSampleResolution = DefaultSampleResolution;
            ZoomX = ActualZoomX = DefaultZoomX;
            ScaleZoomX = true;

            VolumeBoost = DefaultVolumeBoost;
            LineThickness = ActualLineThickness = DefaultLineThickness;
            CenterLineThickness = DefaultCenterLineThickness;

            BackgroundColor = Color.Transparent;
            PrimaryLineColor = Color.Blue;
            SecondaryLineColor = Color.LightSlateGray;
            CenterLineColor = Color.DarkBlue;
            
            SampleThreshold = DefaultSampleThreshold;
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
        public Pen GetPen(RenderPenType type)
        {
            return type switch
            {
                RenderPenType.PrimaryLine => new Pen(PrimaryLineColor, ActualLineThickness),
                RenderPenType.SecondaryLine => new Pen(SecondaryLineColor, ActualLineThickness),
                RenderPenType.CenterLine => new Pen(CenterLineColor, CenterLineThickness),
                _ => throw new ArgumentException(nameof(type)),
            };
        }
        #endregion

        /************************************************************************/

        #region Protected methods
        /// <summary>
        /// Called when <see cref="Height"/> is set.
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
            if (ScaleZoomX)
            {
                while (width > maxWidth && ActualZoomX > MinZoomX)
                {
                    ActualZoomX--;
                    ActualLineThickness = GetActualLineThickness(ActualZoomX, ActualLineThickness);
                    width = GetUnClampedAutoImageWidth(sampleCount, channels);
                }
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
        #endregion
    }
}