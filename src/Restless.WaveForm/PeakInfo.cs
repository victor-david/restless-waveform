using System;

namespace Restless.WaveForm
{
    /// <summary>
    /// Represents inform about audio peaks
    /// </summary>
    public class PeakInfo
    {
        #region Private
        private float leftMin;
        private float leftMax;
        private float rightMin;
        private float rightMax;
        #endregion

        /************************************************************************/

        #region Public enums
        /// <summary>
        /// Enumeration used by the <see cref="GetValue(Channel, Value)"/> method.
        /// </summary>
        public enum Channel
        {
            Left,
            Right,
        }

        /// <summary>
        /// Enumeration used by the <see cref="GetValue(Channel, Value)"/> method.
        /// </summary>
        public enum Value
        {
            Min,
            Max
        }
        #endregion

        /************************************************************************/

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PeakInfo"/> class.
        /// </summary>
        /// <param name="leftMin">Minimum value for left channel</param>
        /// <param name="leftMax">Maximum value for left channel</param>
        /// <param name="rightMin">Minimum value for right channel</param>
        /// <param name="rightMax">Maximum value for right channel</param>
        public PeakInfo(float leftMin, float leftMax, float rightMin, float rightMax)
        {
            this.leftMin = leftMin;
            this.leftMax = leftMax;
            this.rightMin = rightMin;
            this.rightMax = rightMax;
        }
        #endregion

        /************************************************************************/

        #region Public methods
        /// <summary>
        /// Gets the specified value.
        /// </summary>
        /// <param name="channel">Channel to get the value for.</param>
        /// <param name="value">The value type (min, max)</param>
        /// <returns>The value that corresponds to the specified channel and value type</returns>
        public float GetValue(Channel channel, Value value)
        {
            return channel switch
            {
                Channel.Left => value == Value.Min ? leftMin : leftMax,
                Channel.Right => value == Value.Min ? rightMin : rightMax,
                _ => throw new ArgumentException("Invalid channel")
            };
        }
        
        /// <summary>
        /// Applies the specified noise threshold to all values.
        /// </summary>
        /// <param name="threshold">The noise threshold value.</param>
        /// <returns>This instance</returns>
        public PeakInfo ApplyNoiseThreshold(float threshold)
        {
            leftMin = ValueWithNoiseThreshold(leftMin, threshold);
            leftMax = ValueWithNoiseThreshold(leftMax, threshold);

            rightMin = ValueWithNoiseThreshold(rightMin, threshold);
            rightMax = ValueWithNoiseThreshold(rightMax, threshold);

            return this;
        }

        public override string ToString()
        {
            return $"Min(L): {leftMin}, Max(L): {leftMax}, Min(R): {rightMin}, Max(R): {rightMax}";
        }
        #endregion

        /************************************************************************/

        #region Private methods
        private float ValueWithNoiseThreshold(float value, float threshold)
        {
            return Math.Abs(value) > threshold ? value : 0;
        }
        #endregion
    }
}