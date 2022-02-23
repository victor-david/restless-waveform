# Restless Waveform

**Restless WaveForm** enables you to render waveforms of audio files. Uses [NAudio](https://github.com/naudio/naudio) to extract the peaks and **System.Drawing** to render the images.

This project was inspired by Mark Heath's [Wave Form Renderer](https://github.com/naudio/NAudio.WaveFormRenderer), but has been refactored 
in several different ways.

## Features

- Render mono and stereo audio
- Supports several calculation strategies (max, min, average, RMS)
- Built-in rendering styles, or create your own.
 
## Projects
The solution includes two projects:

- **Restless.WaveForm** - The library used for audio file rendering
- **Restless.App.WaveForm** - A WPF demonstration app.
