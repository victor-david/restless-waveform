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
        private bool autoImageWidth;
        private bool useDecibelScale;
        private bool isControlPanelEnabled;
        private Settings selectedSetting;
        private IRenderer selectedRenderer;
        private ImageSource fileVisualLeft;
        private ImageSource fileVisualRight;
        private GridLength fileVisualRightRow;
        private Visibility renderTextVisibility;
        #endregion

        /************************************************************************/

        #region Public fields
        public const double MinImageWidth = Settings.MinWidth;
        public const double MaxImageWidth = Settings.MaxWidth;
        public const double DefaultImageWidth = Settings.DefaultWidth;

        public const double MinHeight = Settings.MinHeight;
        public const double MaxHeight = Settings.MaxHeight;
        public const double DefaultHeight = Settings.DefaultHeight;
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
        /// Gets a list of available renderers.
        /// </summary>
        public List<IRenderer> Renderers
        {
            get;
        }

        /// <summary>
        /// Gets or sets the selected renderer
        /// </summary>
        public IRenderer SelectedRenderer
        {
            get => selectedRenderer;
            set
            {
                SetProperty(ref selectedRenderer, value);
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
            get => selectedSetting;
            set
            {
                SetProperty(ref selectedSetting, value);
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
                if (!AutoImageWidth)
                {
                    CreateVisualization();
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean value that determines if the image width is automatically
        /// set according to the audio being rendered.
        /// </summary>
        public bool AutoImageWidth
        {
            get => autoImageWidth;
            set
            {
                SetProperty(ref autoImageWidth, value);
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

        /// <summary>
        /// Gets a boolean value that determines is the control panel is enabled.
        /// </summary>
        public bool IsControlPanelEnabled
        {
            get => isControlPanelEnabled;
            private set => SetProperty(ref isControlPanelEnabled, value);
        }

        /// <summary>
        /// Gets a value that determines if the rendering text nessage is visible.
        /// </summary>
        public Visibility RenderTextVisibility
        {
            get => renderTextVisibility;
            private set => SetProperty(ref renderTextVisibility, value);
        }
        #endregion

        /************************************************************************/

        #region Constructor
        public MainWindowViewModel()
        {
            RmsBlockSize = 128;
            ImageWidth = DefaultImageWidth;
            AutoImageWidth = Settings.DefaultAutoWidth;
            RenderTextVisibility = Visibility.Collapsed;
            IsControlPanelEnabled = true;

            /* Is default null, but the setter sets the right row height to zero when null */
            FileVisualRight = null;

            Renderers = new List<IRenderer>()
            {
                new SineRenderer(),
                new BarRenderer()
            };

            SelectedRenderer = Renderers.FirstOrDefault();

            WaveFormSettings = new List<Settings>()
            {
                new SineSettings(),
                new BarSettings(),
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
#if DEBUG
            string dir = @"D:\Development\Visual_Studio\Projects\Restless.WaveForm\samples";
            if (Directory.Exists(dir))
            {
                dialog.InitialDirectory = dir;
            }
#endif
            if (dialog.ShowDialog() == true)
            {
                SelectedFile = dialog.FileName;
            }
        }

        private void CreateVisualization()
        {
            if (!string.IsNullOrEmpty(SelectedFile) && File.Exists(SelectedFile) && SelectedRenderer != null)
            {
                CreateVisualization(SelectedRenderer, GetRendererSettings());
            }
        }

        private async void CreateVisualization(IRenderer peakProvider, Settings settings)
        {
            RenderResult images = null;
            try
            {
                SetRenderInProgress(true);
                using (AudioFileReader waveStream = new(SelectedFile))
                {
                    images = await WaveFormRenderer.CreateAsync(waveStream, peakProvider, settings);
                }
                
                FileVisualLeft = CreateBitmapSourceFromGdiBitmap((Bitmap)images.ImageLeft);
                FileVisualRight = images.Channels == 2 ? CreateBitmapSourceFromGdiBitmap((Bitmap)images.ImageRight) : null;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
            finally
            {
                SetRenderInProgress(false);
            }
        }

        private Settings GetRendererSettings()
        {
            Settings setting = SelectedWaveFormSetting;
            setting.Width = (int)ImageWidth;
            setting.AutoWidth = AutoImageWidth;
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

        private void SetRenderInProgress(bool inProgress)
        {
            if (inProgress)
            {
                FileVisualLeft = null;
                FileVisualRight = null;
                RenderTextVisibility = Visibility.Visible;
                IsControlPanelEnabled = false;
            }
            else
            {
                RenderTextVisibility = Visibility.Collapsed;
                IsControlPanelEnabled = true;
            }
        }
        #endregion
    }
}