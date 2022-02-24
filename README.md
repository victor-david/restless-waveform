# Restless Waveform

**Restless WaveForm** enables you to render waveforms of audio files. Uses [NAudio](https://github.com/naudio/naudio) to extract the peaks and **System.Drawing** to render the images.

This project was inspired by Mark Heath's [Wave Form Renderer](https://github.com/naudio/NAudio.WaveFormRenderer), but has been refactored 
in several different ways.

## Features

- Render mono and stereo audio
- Supports several calculation strategies (max, min, average, RMS)
- Built-in rendering styles, or create your own.
 
## Screen shots

### Stereo (bar renderer)
![Restless Waveform Screenshot #1](/screen/restless.waveform.1.jpg)

### Stereo (sine renderer)
![Restless Waveform Screenshot #2](/screen/restless.waveform.2.jpg)

### Mono (fat bar renderer)
![Restless Waveform Screenshot #3](/screen/restless.waveform.3.jpg)

### Mono(bar renderer with sine style)
![Restless Waveform Screenshot #4](/screen/restless.waveform.4.jpg)

## Projects
The solution includes two projects:

- **Restless.WaveForm** - The library used for audio file rendering
- **Restless.App.WaveForm** - A WPF demonstration app.
