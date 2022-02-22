using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restless.WaveForm
{
    /// <summary>
    /// Provides static utility methods
    /// </summary>
    public static class Utility
    {
        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static uint Clamp(uint value, uint min, uint max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static long Clamp(long value, long min, long max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static long SampleCount(this WaveStream stream)
        {
            int bytesPerSample = stream.WaveFormat.BitsPerSample / 8;
            return stream.Length / bytesPerSample;
        }

        /// <summary>
        /// Gets an even integer value clamped between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Min allowed.</param>
        /// <param name="max">Max allowed.</param>
        /// <returns>An even integer value clamped</returns>
        public static int GetEvenValue(int value, int min, int max)
        {
            return Math.Max(Math.Min(GetEvenValue(value), GetEvenValue(max)), GetEvenValue(min));
        }

        /// <summary>
        /// Gets an even long integer value clamped between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="min">Min allowed.</param>
        /// <param name="max">Max allowed.</param>
        /// <returns>An even integer value clamped</returns>
        public static long GetEvenValue(long value, long min, long max)
        {
            return Math.Max(Math.Min(GetEvenValue(value), GetEvenValue(max)), GetEvenValue(min));
        }

        /// <summary>
        /// Gets an even integer value from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>If value is even, value; else value+1</returns>
        public static int GetEvenValue(int value)
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
        public static long GetEvenValue(long value)
        {
            if (value % 2 != 0)
            {
                value++;
            }
            return value;
        }
    }
}