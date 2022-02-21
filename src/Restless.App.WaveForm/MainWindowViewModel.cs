using Microsoft.Win32;
using NAudio.Wave;
using Restless.WaveForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Restless.App.Wave
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Private
        private string selectedFile;
        private int rmsBlockSize;
        private double imageWidth;
        private double topHeight;
        private double bottomHeight;
        private bool useDecibelScale;
        private Settings selectedWaveSetting;
        private IPeakProvider selectedPeakProvider;
        private ImageSource fileVisualLeft;
        private ImageSource fileVisualRight;
        private GridLength fileVisualRightRow;
        #endregion

        /************************************************************************/

        #region Public fields
        public const double MinImageWidth = Settings.MinWidth;
        public const double MaxImageWidth = 2400;
        public const double DefaultImageWidth = Settings.DefaultWidth;

        public const double MinHeight = 10;
        public const double MaxHeight = 148;
        public const double DefaultHeight = 72;
        #endregion

        /************************************************************************/

        #region Properties
        /// <summary>
        /// Gets the command to open an audio file for rendering.
        /// </summary>
        public ICommand OpenCommand
        {
            get;
        }

        /// <summary>
        /// Gets the currently selected file name.
        /// </summary>
        public string SelectedFile
        {
            get => selectedFile;
            private set
            {
                SetProperty(ref selectedFile, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets or sets the block size.
        /// </summary>
        public int RmsBlockSize
        {
            get => rmsBlockSize;
            set => SetProperty(ref rmsBlockSize, value);
        }

        /// <summary>
        /// Gets a list of available peak providers.
        /// </summary>
        public List<IPeakProvider> PeakProviders
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected peak provider.
        /// </summary>
        public IPeakProvider SelectedPeakProvider
        {
            get => selectedPeakProvider;
            set
            {
                SetProperty(ref selectedPeakProvider, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets a list of available wave form settings.
        /// </summary>
        public List<Settings> WaveFormSettings
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected wave form setting.
        /// </summary>
        public Settings SelectedWaveFormSetting
        {
            get => selectedWaveSetting;
            set
            {
                SetProperty(ref selectedWaveSetting, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets or sets the image width
        /// </summary>
        public double ImageWidth
        {
            get => imageWidth;
            set
            {
                SetProperty(ref imageWidth, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets or sets the top height
        /// </summary>
        public double TopHeight
        {
            get => topHeight;
            set
            {
                SetProperty(ref topHeight, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets or sets the bottom height
        /// </summary>
        public double BottomHeight
        {
            get => bottomHeight;
            set
            {
                SetProperty(ref bottomHeight, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if a
        /// decibel scale is applied to the peak provider.
        /// </summary>
        public bool UseDecibelScale
        {
            get => useDecibelScale;
            set
            {
                SetProperty(ref useDecibelScale, value);
                CreateVisualization();
            }
        }

        /// <summary>
        /// Gets the image source for the visual representation of the rendered file (left channel).
        /// </summary>
        public ImageSource FileVisualLeft
        {
            get => fileVisualLeft;
            private set => SetProperty(ref fileVisualLeft, value);
        }

        /// <summary>
        /// Gets the image source for the visual representation of the rendered file (right channel).
        /// </summary>
        public ImageSource FileVisualRight
        {
            get => fileVisualRight;
            private set
            {
                SetProperty(ref fileVisualRight, value);
                FileVisualRightRow = fileVisualRight != null ? new GridLength(1.0, GridUnitType.Star) : new GridLength(0.0, GridUnitType.Pixel);
            }
        }

        /// <summary>
        /// Gets the row height for the right channel, either * (when 2 channels) or zero pixels (when single channel)
        /// </summary>
        public GridLength FileVisualRightRow
        {
            get => fileVisualRightRow;
            private set => SetProperty(ref fileVisualRightRow, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        public MainWindowViewModel()
        {
            RmsBlockSize = 128;
            ImageWidth = DefaultImageWidth;
            TopHeight = BottomHeight = DefaultHeight;

            /* Is default null, but the setter sets the right row height to zero when null */
            FileVisualRight = null;

            PeakProviders = new List<IPeakProvider>()
            {
                new MaxPeakProvider(),
                new RmsPeakProvider(),
                new SamplingPeakProvider(),
                new AveragePeakProvider()

            };

            SelectedPeakProvider = PeakProviders.FirstOrDefault();

            WaveFormSettings = new List<Settings>()
            {
                new StandardSettings(),
                new SoundCloudOriginalSettings(),
                SoundCloudBlockSettings.CreateLight(),
                SoundCloudBlockSettings.CreateDark(),
                SoundCloudBlockSettings.CreateOrange(),
            };

            SelectedWaveFormSetting = WaveFormSettings.FirstOrDefault();

            OpenCommand = RelayCommand.Create(RunOpenCommand);
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private void RunOpenCommand(object parm)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Audio Files(*.wav, *.mp3)|*.wav;*.mp3",
                InitialDirectory = Path.GetTempPath()
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFile = dialog.FileName;
            }
        }

        private void CreateVisualization()
        {
            if (!string.IsNullOrEmpty(SelectedFile) && File.Exists(SelectedFile) && SelectedPeakProvider != null)
            {
                CreateVisualization(SelectedPeakProvider, GetRendererSettings());
            }
        }

        private void CreateVisualization(IPeakProvider peakProvider, Settings settings)
        {
            RenderResult images = null;
            try
            {
                using (AudioFileReader waveStream = new(SelectedFile))
                {
                    images = WaveFormRenderer.Create(waveStream, peakProvider, settings);
                }
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() =>
                {
                    FileVisualLeft = CreateBitmapSourceFromGdiBitmap((Bitmap)images.ImageLeft);
                    FileVisualRight = images.Channels == 2 ? CreateBitmapSourceFromGdiBitmap((Bitmap)images.ImageRight) : null;
                }));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        private Settings GetRendererSettings()
        {
            Settings setting = SelectedWaveFormSetting;
            setting.TopHeight = (int)TopHeight;
            setting.BottomHeight = (int)BottomHeight;
            setting.Width = (int)ImageWidth;
            setting.DecibelScale = UseDecibelScale;
            return setting;
        }

        private static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int size = rect.Width * rect.Height * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
        #endregion
    }
}