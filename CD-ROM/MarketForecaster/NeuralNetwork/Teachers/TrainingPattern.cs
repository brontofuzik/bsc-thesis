using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;



namespace NeuralNetwork.Teachers
{
	/// <remarks>
    /// <para>
	/// A (labeled) training pattern.
	/// </para>
    /// <para>
    /// Definition: A training pattern is a pair <em>(inputVector, outputVector)</em> where
    /// <em>inputVector</em> is the input vector and <em>outputVector</em> is the desired output vector of the pattern.
    /// </para>
	/// </remarks>
    public class TrainingPattern
    {
        #region Private instance fields

        /// <summary>
        /// The input vector.
        /// </summary>
        private double[] inputVector;

        /// <summary>
        /// The output vector.
        /// </summary>
        private double[] outputVector;

        /// <summary>
        /// The normalized input vector.
        /// </summary>
        private double[] normalizedInputVector;

        /// <summary>
        /// The normalized output vector.
        /// </summary>
        private double[] normalizedOutputVector;

        #endregion // Private instance fields

        #region Public instance properties

		/// <summary>
		/// Gets the input vector.
		/// </summary>
		///
		/// <value>
        /// The input vector.
        /// </value>
        public double[] InputVector
        {
            get
            {
                return inputVector;
            }
        }

		/// <summary>
		/// Gets the output vector.
		/// </summary>
		///
		/// <value>
        /// The output vector.
        /// </value>
        public double[] OutputVector
        {
            get
            {
                return outputVector;
            }
        }

        /// <summary>
        /// Gets the normalized input vector.
        /// </summary>
        /// 
        /// <value>
        /// The normalized input vector.
        /// </value>
        public double[] NormalizedInputVector
        {
            get
            {
                return normalizedInputVector;
            }
        }

        /// <summary>
        /// Gets the normalized output vector.
        /// </summary>
        ///
        /// <value>
        /// The normalized output vector.
        /// </value>
        public double[] NormalizedOutputVector
        {
            get
            {
                return normalizedOutputVector;
            }
        }

        #endregion // Public instacne properties

        #region Public instance constructors

		/// <summary>
		/// Creates a new (labeled) training pattern.
		/// </summary>
		///
		/// <param name="inputVector">The input vector.</param>
		/// <param name="outputVector">The output vector.</param>
		///
		/// <exception cref="ArgumentNullException">
		/// Condition 1: <c>inputVector</c> is <c>null</c>.
        /// Condition 2: <c>outputVector</c> is <c>null</c>.
		/// </exception>
        public TrainingPattern( double[] inputVector, double[] outputVector )
        {
            // Validate the arguments.
            Utilities.ObjectNotNull( inputVector, "inputVector" );
            Utilities.ObjectNotNull( outputVector, "outputVector" );

            // Initialize the instance fields.
            this.inputVector = inputVector;
            this.outputVector = outputVector;

            // Normalize the input and output vectors.
            normalizedInputVector = NormalizeVector( inputVector );
            normalizedOutputVector = NormalizeVector( outputVector );
        }

        #endregion // Public instance constructors   

        #region Public static methods

        /// <summary>
        /// Normalizes a vector of numbers to the given magnitude.
        /// </summary>
        ///
        /// <param name="vector">The vector to be normalized.</param>
        /// <param name="magnitude">The magnitude of the normalized vector.</param>
        ///
        /// <returns>
        /// The normalized vector of the given magnitude.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Condition: <c>magnitude</c> is negative. 
        /// </exception>
        public static double[] NormalizeVector(double[] vector, double magnitude)
        {
            // Validate the arguments
            Utilities.ObjectNotNull(vector, "vector");
            Utilities.NumberNonNegative(magnitude, "magnitude");

            // Calculate the sum of squares
            double sumOfSquares = 0d;
            for (int i = 0; i < vector.Length; i++)
            {
                sumOfSquares += vector[i] * vector[i];
            }

            // Calculate the normalization factor
            double factor = (sumOfSquares != 0) ? Math.Sqrt(magnitude / sumOfSquares) : 0;

            // Calculate the normalized vector 
            double[] normalizedVector = new double[vector.Length];
            for (int i = 0; i < normalizedVector.Length; i++)
            {
                normalizedVector[i] = vector[i] * factor;
            }

            return normalizedVector;
        }

        /// <summary>
        /// Normalizes a vector of numbers to the magnitude of 1.
        /// </summary>
        ///
        /// <param name="vector">The vector to be normalized.</param>
        ///
        /// <returns>
        /// The normalized vector of the magnitude 1.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        public static double[] NormalizeVector(double[] vector)
        {
            return NormalizeVector(vector, 1d);
        }

        /// <summary>
        /// Returns a string that represents the given vector.
        /// </summary>
        /// <param name="vector">The vector to be represented by string.</param>
        /// <returns>
        /// A string that represents the given vector.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        public static string VectorToString( double[] vector )
        {
            // Validate the arguments.
            Utilities.ObjectNotNull( vector, "vector" );

            StringBuilder vectorStringBuilder = new StringBuilder();

            vectorStringBuilder.Append( "[" );
            foreach (double element in vector)
            {
                vectorStringBuilder.Append( element.ToString( "F2" ) + ", " );
            }
            if (vector.Length > 0)
            {
                vectorStringBuilder.Remove( vectorStringBuilder.Length - 2, 2 );
            }
            vectorStringBuilder.Append( "]" );

            return vectorStringBuilder.ToString();
        }

        #endregion // Public static methods

        #region Public instance methods

        /// <summary>
        /// Converts the training pattern to its string representation.
        /// </summary>
        /// 
        /// <returns>
        /// The training pattern's string representation.
        /// </returns>
        public override string ToString()
        {
            return "(" + VectorToString( inputVector ) + ", " + VectorToString( outputVector ) + ")";
            
            // alternatively:
            // return (base.ToString() + "(" + VectorToString( inputVector ) + "," + VectorToString( outputVector ) + ")");
        }

        #endregion // Public insatnce methods
    }
}
