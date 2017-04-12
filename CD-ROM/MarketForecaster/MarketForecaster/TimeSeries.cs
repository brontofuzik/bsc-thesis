using System;
using System.Collections.Generic;
using System.IO;

using NeuralNetwork.Teachers;

namespace MarketForecaster
{
    /// <summary>
    /// The time series.
    /// </summary>
    class TimeSeries
    {
        #region Private instance fields

        /// <summary>
        /// The time series data.
        /// </summary>
        private double[] data;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets an element at the given index (indexer).
        /// </summary>
        /// <param name="index">The index of the element to get.</param>
        /// <returns>
        /// The element at the given index.
        /// </returns>
        public double this[ int index ]
        {
            get
            {
                return data[ index ];
            }
        }

        /// <summary>
        /// Gets the length of the time series (i.e. the number of elements).
        /// </summary>
        /// <value>
        /// The length of the time series.
        /// </value>
        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new time series.
        /// </summary>
        /// <param name="fileName">The name of the file contaiing the time series.</param>
        /// <param name="delimiter">The character separating the time series members (a.k.a. elements, terms).</param>
        public TimeSeries( string fileName, char delimiter )
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader( fileName );

                List< double > dataList = new List< double >();

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine().Trim();
                    
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    string[] words = line.Split( delimiter );

                    foreach (string word in words)
                    {
                        dataList.Add( Double.Parse( word ) );
                    }
                }

                data = dataList.ToArray();
            }
            catch
            {
                throw new ArgumentException( "fileName" );
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }
        }

        #endregion // Public insatnce constructors

        #region Public instance methods

        /// <summary>
        /// Builds a training set.
        /// </summary>
        /// <param name="lags"></param>
        /// <param name="leaps"></param>
        /// <returns>
        /// The training set.
        /// </returns>
        public TrainingSet BuildTrainingSet( int[] lags, int[] leaps  )
        {
            TrainingSet trainingSet = new TrainingSet( lags.Length, leaps.Length );

            // The following assumes that lags and leaps are ordered in ascending fashion.
            int maxLag = lags[ 0 ];
            int maxLeap = leaps[ leaps.Length - 1 ];

            // Add training patterns into the training set.
            for (int i = -maxLag; i < Length - maxLeap; i++)
            {
                // Build the input vector.
                double[] inputVector = new double[ lags.Length ];
                for (int j = 0; j < inputVector.Length; j++)
                {
                    inputVector[ j ] = data[ i + lags[ j ] ];
                }

                // Build the output vector.
                double[] outputVector = new double[ leaps.Length ];
                for (int j = 0; j < outputVector.Length; j++)
                {
                    outputVector[ j ] = data[ i + leaps[ j ] ];
                }

                // Build a training pattern and add it to the training set.
                TrainingPattern trainingPattern = new TrainingPattern( inputVector, outputVector );
                trainingSet.Add( trainingPattern );
            }

            return trainingSet;
        }

        #endregion // Public instance methods
    }
}
