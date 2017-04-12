using System;
using System.IO;

using System.Collections.Generic;

namespace MarketForecaster
{
    /// <summary>
    /// The forecasting log.
    /// </summary>
    class ForecastingLog
    {
        #region Private instance fields

        /// <summary>
        /// The forecasting log stream writer.
        /// </summary>
        StreamWriter streamWriter;

        /// <summary>
        /// The header of the forecasting log.
        /// </summary>
        string[] header;

        /// <summary>
        /// The entries of the forecasting log.
        /// </summary>
        private List< string[] > entries;

        #endregion // Private instance fields

        #region Public instance constructors

        /// <summary>
        /// Creates a new forecasting log.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="columnTitles">The header of the forecasting.</param>
        public ForecastingLog( string fileName, string[] header )
        {
            streamWriter = new StreamWriter( fileName );
            this.header = header;

            // Log the header.
            entries = new List< string[] >();
            entries.Add( header );
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Writes an entry to the training log.
        /// </summary>
        /// <param name="words"></param>
        public void Write( params string[] entry )
        {
            // Validate the size of the log entry against the size of the log header.
            if (entry.Length != header.Length)
            {
                throw new ArgumentException( "The size of the log entry (" + entry.Length + ") does not match the size of the log header (" + header.Length + ")." );
            }

            // Log the entry.
            entries.Add( entry );
        }

        /// <summary>
        /// Closes the training log.
        /// </summary>
        public void Close()
        {
            int[] columnWidths = new int[ header.Length ];

            // For each column ...
            for (int columnIndex = 0; columnIndex < header.Length; columnIndex++)
            {
                // ... determine the maximmum width.
                int maxWidth = Int32.MinValue;
                foreach (string[] entry in entries)
                {
                    if (entry[ columnIndex ].Length > maxWidth)
                    {
                        maxWidth = entry[ columnIndex ].Length;
                    }
                }
                columnWidths[ columnIndex ] = maxWidth;
            }

            // Print the entries
            for (int entryIndex = 0; entryIndex < entries.Count; entryIndex++ )
            {
                for (int columnIndex = 0; columnIndex < header.Length; columnIndex++)
                {
                    // Write data ...
                    streamWriter.Write( entries[ entryIndex ][ columnIndex ] );

                    if (columnIndex == header.Length - 1)
                    {
                        continue;
                    }

                    // ... and spacing.
                    int spacingWidth = columnWidths[ columnIndex ] - entries[ entryIndex ][ columnIndex ].Length + 1;
                    String spacing = new string( ' ', spacingWidth );
                    streamWriter.Write( ";" + spacing );
                }

                if (entryIndex == entries.Count - 1)
                {
                    continue;
                }

                streamWriter.WriteLine();
            }

            // Close the forecasting log.
            streamWriter.Close();
        }

        #endregion // Public instance methods
    }
}
