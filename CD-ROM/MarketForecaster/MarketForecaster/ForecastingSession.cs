using System;
using System.Collections.Generic;
using System.IO;

namespace MarketForecaster
{
    /// <summary>
    /// The forecasting seesion.
    /// </summary>
    class ForecastingSession
    {
        #region Private instance fields

        /// <summary>
        /// The forecasting session stream reader.
        /// </summary>
        StreamReader streamReader;

        #endregion // Private insance fields

        #region Public instance properties

        public bool EndOfStream
        {
            get
            {
                return streamReader.EndOfStream;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Create a new forecasting session.
        /// </summary>
        /// <param name="forecastingSessionFileName">The name of the file to load the forecasting session from.</param>
        public ForecastingSession( string forecastingSessionFileName )
        {
            streamReader = new StreamReader( forecastingSessionFileName );
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Reads a forecasting trial.
        /// </summary>
        /// <returns></returns>
        public string[] Read()
        {
            string[] words = streamReader.ReadLine().Trim().Split( ';' );
            for (int i = 0; i < words.Length; i++)
            {
                words[ i ] = words[ i ].Trim();
            }
            return words;
        }

        /// <summary>
        /// Closes the training session.
        /// </summary>
        public void Close()
        {
            streamReader.Close();
        }

        #endregion // Public instance methods
    }
}
