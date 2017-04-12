using System;

namespace NeuralNetwork
{
    /// <summary>
    /// This (internal static) class represents a collection of static validation routines.
    /// </summary>
    internal static class Utilities
    {
        #region Private static fields

        /// <summary>
        /// 
        /// </summary>
        private static Random random;

        #endregion // Private static fields

        #region Static constructor

        /// <summary>
        /// 
        /// </summary>
        static Utilities()
        {
            random = new Random();
        }

        #endregion // Static constructor

        #region Internal static methods

		/// <summary>
		/// Validates that an object is not <c>null</c>.
		/// </summary>
		///
		/// <param name="obj">The object to validate.</param>
		/// <param name="objName">The name of the object.</param>
		///
		/// <exception cref="ArgumentNullException">
        /// Condition: <c>obj</c> is <c>null</c>.
        /// </exceptio>
        internal static void ObjectNotNull( object obj, string objName )
        {
            if (obj == null)
            {
                throw new ArgumentNullException( objName );
            }
        }

        /// <summary>
        /// Validates that a number is non-negative.
        /// </summary>
        ///
        /// <param name="number">The number to validate.</param>
        /// <param name="numberName">The name of the number.</param>
        ///
        /// <exception cref="ArgumentException">
        /// Condition: <c>number</c> is negative.
        /// </exception>
        internal static void NumberNonNegative( double number, string numberName )
        {
            if (number < 0)
            {
                throw new ArgumentException( "The argument should be non-negative (greater than or equal to zero)", numberName );
            }
        }

        /// <summary>
        /// Validates that a number is positive.
        /// </summary>
        ///
        /// <param name="number">The value of the number.</param>
        /// <param name="numberName">The name of the number</param>
        ///
        /// <exception cref="ArgumentException">
        /// Condition: <c> number </c> is non-positive.
        /// </exception>
        internal static void NumberPositive( double number, string numberName )
        {
            if (number <= 0)
            {
                throw new ArgumentException( "The argument should be positive (greater than zero)", numberName );
            }
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        ///
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        ///<param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        ///
        /// <returns>
        /// A double-precision floating point number greater than or equal to minValue and less than or equal to maxValue; that is, the range of return values includes minValue and maxValue.
        /// </returns>
        ///
        /// <exception name="ArgumentOutOfRangeException">
        /// Condition: <c>minValue</c> is greater than <c>maxValue</c>. 
        /// </exception>
        internal static double NextDouble( double minValue, double maxValue )
        {
            // Validate the arguments.
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            return (minValue + (maxValue - minValue) * random.NextDouble());
        }

        #endregion // Internal Static Methods
    }
}
