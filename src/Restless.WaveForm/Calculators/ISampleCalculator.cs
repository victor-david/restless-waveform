namespace Restless.WaveForm.Calculators
{
    /// <summary>
    /// Defines properties and methods that a sample calculator must implement
    /// </summary>
    public interface ISampleCalculator
    {
        /// <summary>
        /// Gets a display name for the calculator.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Returns a value based upon a range of values
        /// </summary>
        /// <param name="buffer">The buffer that holds the values</param>
        /// <param name="startIdx">The starting index to examine, inclusive</param>
        /// <param name="endIdx">The ending index to examine, exclusive</param>
        /// <returns>A value calculated from the range</returns>
        float Calculate(float[] buffer, int startIdx, int endIdx);
    }
}