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
        private float xStepX;
        private float actualXStep;
        private float volumeBoost;
        private float lineThickness;
        private float actualLineThickness;
        private int centerLineThickness;
        private float sampleThreshold;
        #endregion

        /************************************************************************/

        #region Public consts
        /// <summary>
        /// The minimum value for <see cref="Width"/>
        /// </summary>
        public const int MinWidth = 800;
        /// <summary>
        /// The maximum value for <see cref="Width"/>
        /// </summary>
        public const int MaxWidth = 36000;
        /// <summary>
        /// The default value for <see cref="Width"/>
        /// </summary>
        public const int DefaultWidth = 1200;
        /// <summary>
        /// The default value for <see cref="AutoWidth"/>.
        /// </summary>
        public const bool DefaultAutoWidth = true;

        /// <summary>
        /// The minimum value for <see cref="Height"/>
        /// </summary>
        public const int MinHeight = 32;
        /// <summary>
        /// The maximum value for <see cref="Height"/>
        /// </summary>
        public const int MaxHeight = 228;
        /// <summary>
        /// The default value for <see cref="Height"/>
        /// </summary>
        public const int DefaultHeight = 76;

        /// <summary>
        /// The minimum value for <see cref="SampleResolution"/>
        /// </summary>
        public const int MinSampleResolution = 2;
        /// <summary>
        /// The maximum value for <see cref="SampleResolution"/>
        /// </summary>
        public const int MaxSampleResolution = 192;
        /// <summary>
        /// The default value for <see cref="SampleResolution"/>
        /// </summary>
        public const int DefaultSampleResolution = 8;

        /// <summary>
        /// The minimum value for <see cref="XStep"/>
        /// </summary>
        public const float MinXStep = 0.01f;
        /// <summary>
        /// The maximum value for <see cref="XStep"/>
        /// </summary>
        public const float MaxXStep = 8;
        /// <summary>
        /// The default value for <see cref="XStep"/>
        /// </summary>
        public const float DefaultXStep = 2;
        /// <summary>
        /// The number of decimal points for <see cref="XStep"/>
        /// </summary>
        public const int XStepDecimals = 2;

        /// <summary>
        /// The minimum value for <see cref="VolumeBoost"/>
        /// </summary>
        public const float MinVolumeBoost = 1;
        /// <summary>
        /// The maximum value for <see cref="VolumeBoost"/>
        /// </summary>
        public const float MaxVolumeBoost = 9.5f;
        /// <summary>
        /// The default value for <see cref="VolumeBoost"/>
        /// </summary>
        public const float DefaultVolumeBoost = 1;

        /// <summary>
        /// The minimum value for <see cref="LineThickness"/>
        /// </summary>
        public const float MinLineThickness = 1;
        /// <summary>
        /// The maximum value for <see cref="LineThickness"/>
        /// </summary>
        public const float MaxLineThickness = 8;
        /// <summary>
        /// The default value for <see cref="LineThickness"/>
        /// </summary>
        public const float DefaultLineThickness = 1;

        /// <summary>
        /// The minimum value for <see cref="CenterLineThickness"/>
        /// </summary>
        public const int MinCenterLineThickness = 0;
        /// <summary>
        /// The maximum value for <see cref="CenterLineThickness"/>
        /// </summary>
        public const int MaxCenterLineThickness = 5;
        /// <summary>
        /// The default value for <see cref="CenterLineThickness"/>
        /// </summary>
        public const int DefaultCenterLineThickness = 1;

        /// <summary>
        /// The default value for <see cref="SampleThreshold"/>
        /// </summary>
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
            set => width = value.Clamp(MinWidth, MaxWidth);
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
            set => height = value.Clamp(MinHeight, MaxHeight);
        }

        /// <summary>
        /// Gets or sets the desired sample resolution
        /// </summary>
        public int SampleResolution
        {
            get => sampleResolution;
            set => sampleResolution = value.ClampEven(MinSampleResolution, MaxSampleResolution);
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
        /// Gets or sets the desired X-axis step value.
        /// This value is clamped between <see cref="MinXStep"/> and <see cref="MaxXStep"/>.
        /// </summary>
        public float XStep
        {
            get => xStepX;
            set => xStepX = value.Clamp(MinXStep, MaxXStep).Round(XStepDecimals);
        }

        /// <summary>
        /// Gets the actual X-axis step value
        /// </summary>
        public float ActualXStep
        {
            get => actualXStep;
            private set => actualXStep = value.Clamp(MinXStep, MaxXStep).Round(XStepDecimals);
        }

        /// <summary>
        /// Gets or sets a value that determines if <see cref="ActualXStep"/>
        /// is adjusted when creating / scaling the image used for audio render.
        /// The default value is true.
        /// </summary>
        /// <remarks>
        /// If this property is false, <see cref="ActualXStep"/> will not be affected
        /// during the image preparation operation. Only <see cref="ActualSampleResolution"/>
        /// will be changed if needed.
        /// </remarks>
        public bool ScaleXStep
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
            set => volumeBoost = value.Clamp(MinVolumeBoost, MaxVolumeBoost);
        }

        /// <summary>
        /// Gets or sets the line thickness.
        /// This value is clamped between <see cref="MinLineThickness"/> and <see cref="MaxLineThickness"/>.
        /// </summary>
        public float LineThickness
        {
            get => lineThickness;
            set => lineThickness = value.Clamp(MinLineThickness, MaxLineThickness);
        }

        /// <summary>
        /// Gets the actual line thickness.
        /// </summary>
        public float ActualLineThickness
        {
            get => actualLineThickness;
            private set => actualLineThickness = value.Clamp(MinLineThickness, MaxLineThickness);
        }

        /// <summary>
        /// Gets or sets the center line height.
        /// This value is clamped between <see cref="MinCenterLineThickness"/> and <see cref="MaxCenterLineThickness"/>.
        /// </summary>
        public int CenterLineThickness
        {
            get => centerLineThickness;
            set => centerLineThickness = value.Clamp(MinCenterLineThickness, MaxCenterLineThickness);
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
            XStep = ActualXStep = DefaultXStep;
            ScaleXStep = true;

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
            ActualXStep = XStep;
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
        protected virtual float GetActualLineThickness(float actualZoomX, float actualLineThickness)
        {
            return actualLineThickness;
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void PrepareForImageWidth(long sampleCount, int channels)
        {
            int maxWidth = AutoWidth ? MaxWidth : Width;
            ActualXStep = 0.5f;
            long width = GetUnClampedAutoImageWidth(sampleCount, channels);
            if (ScaleXStep)
            {
                while (width > maxWidth && ActualXStep > MinXStep)
                {
                    ActualXStep = (ActualXStep - 0.01f).Round(XStepDecimals);
                    ActualLineThickness = GetActualLineThickness(ActualXStep, ActualLineThickness);
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
            float rawValue = Utility.GetEven(Math.Min(sampleCount, int.MaxValue) / channels) / ActualSampleResolution * ActualXStep;
            return (long)rawValue;

            /// return (long)Utility.GetEven(Math.Min(sampleCount, int.MaxValue) / channels) / ActualSampleResolution * ActualZoomX;
        }

        private long GetClampedAutoImageWidth(long sampleCount, int channels)
        {
            int maxWidth = AutoWidth ? MaxWidth : Width;
            float rawValue = Utility.GetEven(Math.Min(sampleCount, int.MaxValue) / channels) / ActualSampleResolution * ActualXStep;

            return Math.Min((long)rawValue, maxWidth);
        }
        #endregion
    }
}