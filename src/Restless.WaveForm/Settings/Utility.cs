using NAudio.Wave;
using System;

namespace Restless.WaveForm.Settings
{
    /// <summary>
    /// Provides static utility methods
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Returns a value clamped between <paramref name="min"/> and <paramref name="max"/>, inclusive.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The clamped value</returns>
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Returns a value clamped between <paramref name="min"/> and <paramref name="max"/>, inclusive.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The clamped value</returns>
        public static uint Clamp(this uint value, uint min, uint max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Returns a value clamped between <paramref name="min"/> and <paramref name="max"/>, inclusive.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The clamped value</returns>
        public static long Clamp(long value, long min, long max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Returns a value clamped between <paramref name="min"/> and <paramref name="max"/>, inclusive.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Minimum allowed</param>
        /// <param name="max">Maximum allowed</param>
        /// <returns>The clamped value</returns>
        public static float Clamp(this float value, float min, float max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        /// <summary>
        /// Returns a value rounded to the specified number of decimal points
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="decimals">Number of decimal points (clamped 0..12, inclusive)</param>
        /// <returns>The rounded value</returns>
        public static float Round(this float value, int decimals)
        {
            return (float)Math.Round(value, decimals.Clamp(0, 12));
        }

        /// <summary>
        /// Gets the sample count for the stream
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>The total number of samples for the stream</returns>
        public static long SampleCount(this WaveStream stream)
        {
            int bytesPerSample = stream.WaveFormat.BitsPerSample / 8;
            return stream.Length / bytesPerSample;
        }

        /// <summary>
        /// Returns an even integer value clamped between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Min allowed.</param>
        /// <param name="max">Max allowed.</param>
        /// <returns>An even integer value clamped</returns>
        public static int ClampEven(this int value, int min, int max)
        {
            return Math.Max(Math.Min(GetEven(value), GetEven(max)), GetEven(min));
        }

        /// <summary>
        /// Returns an even long integer value clamped between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Min allowed.</param>
        /// <param name="max">Max allowed.</param>
        /// <returns>An even long value clamped</returns>
        public static long ClampEven(this long value, long min, long max)
        {
            return Math.Max(Math.Min(GetEven(value), GetEven(max)), GetEven(min));
        }

        /// <summary>
        /// Gets an even integer value from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If value is even, value; else value+1</returns>
        public static int GetEven(int value)
        {
            if (value % 2 != 0)
            {
                value++;
            }
            return value;
        }

        /// <summary>
        /// Gets an even long integer value from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If value is even, value; else value+1</returns>
        public static long GetEven(long value)
        {
            if (value % 2 != 0)
            {
                value++;
            }
            return value;
        }
    }
}