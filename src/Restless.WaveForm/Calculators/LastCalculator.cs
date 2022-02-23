namespace Restless.WaveForm.Calculators
{
    /// <summary>
    /// Represents a sample calculator that returns the last value in the range.
    /// </summary>
    public class LastCalculator : ISampleCalculator
    {
        public string DisplayName => "Last";

        /// <summary>
        /// Returns a value based upon a range of values
        /// </summary>
        /// <param name="buffer">The buffer that holds the values</param>
        /// <param name="startIdx">The starting index to examine, inclusive</param>
        /// <param name="endIdx">The ending index to examine, exclusive</param>
        /// <returns>A value calculated from the range</returns>
        public float Calculate(float[] buffer, int startIdx, int endIdx)
        {
            return buffer[endIdx];
        }
    }
}