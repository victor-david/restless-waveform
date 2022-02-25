using System;

namespace Restless.WaveForm.Calculators
{
    /// <summary>
    /// Represents a sample calculator that calculates the RMS of the values in the range.
    /// </summary>
    public class RmsCalculator : ISampleCalculator
    {
        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName => "Rms";

        /// <summary>
        /// Returns a value based upon a range of values
        /// </summary>
        /// <param name="buffer">The buffer that holds the values</param>
        /// <param name="startIdx">The starting index to examine, inclusive</param>
        /// <param name="endIdx">The ending index to examine, exclusive</param>
        /// <returns>A value calculated from the range</returns>
        public float Calculate(float[] buffer, int startIdx, int endIdx)
        {
            startIdx = Math.Min(Math.Max(0, startIdx), buffer.Length - 1);
            endIdx = Math.Min(Math.Max(0, endIdx), buffer.Length - 1);

            double sum = 0.0;
            for (int idx = startIdx; idx < endIdx; idx++)
            {
                sum += buffer[idx] * buffer[idx];
            }

            double average = sum / (endIdx - startIdx);
            return (float)Math.Sqrt(average);
        }
    }
}