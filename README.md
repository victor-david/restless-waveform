# Restless Waveform

**Restless WaveForm** enables you to render waveforms of audio files. Uses [NAudio](https://github.com/naudio/naudio) to extract the peaks and **System.Drawing** to render the images.

This project is based on Mark Heath's [Wave Form Renderer](https://github.com/naudio/NAudio.WaveFormRenderer), but has been considerably refactored 
to provide the ability to render stereo audio files as well as other enhancements.

## Features

- Can render mono and stereo audio
- Supports several peak calculation strategies (max, average, sampled, RMS, decibels)
- Supports different colors and different sizes for the top and bottom half
- Several built-in rendering styles
 
## Projects
The solution includes two projects:

- Restless.WaveForm - The library used for audio file rendering
- Restless.App.WaveForm - A WPF demonstration app.
