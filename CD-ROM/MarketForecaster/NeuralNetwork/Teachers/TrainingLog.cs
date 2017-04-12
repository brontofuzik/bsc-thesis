using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NeuralNetwork.Networks;

namespace NeuralNetwork.Teachers
{
    public class TrainingLog
    {
        #region Private instance fields

        /// <summary>
        /// The number of runs used.
        /// </summary>
        private int runCount;

        /// <summary>
        /// The number of iterations used.
        /// </summary>
        private int iterationCount;

        /// <summary>
        /// The minimum network error achieved.
        /// </summary>
        private double networkError;

        /// <summary>
        /// The residual sum of squares (within-sample).
        /// </summary>
        private double rss_trainingSet;

        /// <summary>
        /// The residual standard deviation (within-sample).
        /// </summary>
        private double rsd_trainingSet;

        /// <summary>
        /// The Akaike information criterion.
        /// </summary>
        private double aic;

        /// <summary>
        /// The bias-corrected Akaike information criterion.
        /// </summary>
        private double aicc;

        /// <summary>
        /// The Bayesian information criterion.
        /// </summary>
        private double bic;

        /// <summary>
        /// The Schwarz Bayesian criterion.
        /// </summary>
        private double sbc;

        /// <summary>
        /// The residual sum of squares (out-of-sample).
        /// </summary>
        private double rss_testSet;

        /// <summary>
        /// The residual standard deviation (out-of-sample).
        /// </summary>
        private double rsd_testSet;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets or sets the number of runs used.
        /// </summary>
        /// <value>
        /// The number of runs used.
        /// </value>
        public int RunCount
        {
            get
            {
                return runCount;
            }
            set
            {
                runCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of iterations used.
        /// </summary>
        /// <value>
        /// The number of iterations used.
        /// </value>
        public int IterationCount
        {
            get
            {
                return iterationCount;
            }
            set
            {
                iterationCount = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum network error achieved.
        /// </summary>
        /// <value>
        /// The minimum network error achieved.
        /// </value>
        public double NetworkError
        {
            get
            {
                return networkError;
            }
            set
            {
                networkError = value;
            }
        }

        /// <summary>
        /// Gets the residual sum of squares (within-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (within-sample).
        /// </value>
        public double RSS_TrainingSet
        {
            get
            {
                return rss_trainingSet;
            }
        }

        /// <summary>
        /// Gets the residual standard deviation (within-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (within-sample).
        /// </value>
        public double RSD_TrainingSet
        {
            get
            {
                return rsd_trainingSet;
            }
        }

        /// <summary>
        /// Gets the Akaike information criterion.
        /// </summary>
        /// <value>
        /// The Akaike information criterion.
        /// </value>
        public double AIC
        {
            get
            {
                return aic;
            }
        }

        /// <summary>
        /// Gets the bias-corrected Akaike information criterion.
        /// </summary>
        /// <value>
        /// The bias-corrected Akaike information criterion.
        /// </value>
        public double AICC
        {
            get
            {
                return aicc;
            }
        }

        /// <summary>
        /// Gets the Bayesian information criterion.
        /// </summary>
        /// <value>
        /// The Bayesian information criterion.
        /// </value>
        public double BIC
        {
            get
            {
                return bic;
            }
        }

        /// <summary>
        /// Gets the Schwarz Bayesian criterion.
        /// </summary>
        /// <value>
        /// The Schwarz Bayesian criterion.
        /// </value>
        public double SBC
        {
            get
            {
                return sbc;
            }
        }

        /// <summary>
        /// Gets the residual sum of squares (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (out-of-sample).
        /// </value>
        public double RSS_TestSet
        {
            get
            {
                return rss_testSet;
            }
        }

        /// <summary>
        /// Gets the residual standard deviation (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (out-of-sample).
        /// </value>
        public double RSD_TestSet
        {
            get
            {
                return rsd_testSet;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new training log.
        /// </summary>
        /// <param name="runCount">The number of runs used.</param>
        /// <param name="iterationCount">The number of iterations used (in the best case).</param>
        /// <param name="networkError">The minim network error achieved (in the best case).</param>
        public TrainingLog( int runCount, int iterationCount, double networkError )
        {
            this.runCount = runCount;
            this.iterationCount = iterationCount;
            this.networkError = networkError;
        }

        /// <summary>
        /// Creates a new training log.
        /// </summary>
        /// <param name="iterationCount">The number of iterations used (in the best case).</param>
        /// <param name="networkError">The minim network error achieved (in the best case).</param>
        public TrainingLog( int iterationCount, double networkError )
            : this( 1, iterationCount, networkError )
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Calculates (and logs) the network's measures of fit.
        /// </summary>
        /// <param name="network">The network whose measures of fit are to be calculated (and logged).</param>
        /// <param name="trainingSet">The training set.</param>
        public void CalculateMeasuresOfFit( INetwork network, TrainingSet trainingSet )
        {
            int n = trainingSet.Size;
            int p = network.SynapseCount;

            // Calculate (and log) the residual sum of squares (within-sample).
            rss_trainingSet = CalculateRSS( network, trainingSet );

            // Calculate (and log) the residual standard deviation (within-sample).
            rsd_trainingSet = Math.Sqrt( rss_trainingSet / (double)n );

            // Calculate (and log) the Akaike information criterion.
            aic = n * Math.Log( rss_trainingSet / (double)n ) + 2 * p;

            // Calcuolate (and log) the bias-corrected Akaike information criterion.
            aicc = aic + (2 * (p + 1) * (p + 2)) / (n - p - 2);

            // Calculate (and log) the Bayesian information criterion.
            bic = n * Math.Log( rss_trainingSet / (double)n ) + p + p * Math.Log( n );

            // Calculate (and log) the Schwarz Bayesian criterion.
            sbc = n * Math.Log( rss_trainingSet / (double)n ) + p * Math.Log( n );
        }

        /// <summary>
        /// Calculates (and logs) the network's forecast accuracy.
        /// </summary>
        /// <param name="network">The network whose forecast accuracy is to be calculated (and logged).</param>
        /// <param name="testSet">The test set.</param>
        public void CalculateForecastAccuracy( INetwork network, TrainingSet testSet )
        {
            int n = testSet.Size;
            int p = network.SynapseCount;

            // Calculate the residual sum of squares (out-of-sample).
            rss_testSet = CalculateRSS( network, testSet );

            // Calculate the residual standard deviation (out-of-sample).
            rsd_testSet = Math.Sqrt( rss_testSet / (double)n );
        }

        #endregion // Public instance methods

        #region Private instance methods

        /// <summary>
        /// Calculates the residual sum of squares (RSS) of a given network WRT a given training set.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The RSS.
        /// </returns>
        private double CalculateRSS( INetwork network, TrainingSet trainingSet )
        {
            double rss = 0.0;
            foreach (TrainingPattern trainingPattern in trainingSet.TrainingPatterns)
            {
                double[] outputVector = network.Evaluate( trainingPattern.InputVector );
                double[] desiredOutputVector = trainingPattern.OutputVector;

                rss += Math.Pow( (outputVector[ 0 ] - desiredOutputVector[ 0 ]), 2 );
            }
            return rss;
        }

        #endregion // Private insatnce methods
    }
}
