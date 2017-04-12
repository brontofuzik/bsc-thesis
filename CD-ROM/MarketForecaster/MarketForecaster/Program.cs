using System;
using System.IO;
using System.Text;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.ActivationFunctions;
using NeuralNetwork.Networks;
using NeuralNetwork.Teachers;
using NeuralNetwork.Teachers.BackpropagationTeacher;

namespace MarketForecaster
{
    class Program
    {
        /// <summary>
        /// The time series.
        /// </summary>
        private static TimeSeries timeSeries;

        /// <summary>
        /// The forecasting session.
        /// </summary>
        private static ForecastingSession forecastingSession;

        /// <summary>
        /// The forecasting log.
        /// </summary>
        private static ForecastingLog forecastingLog;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">
        /// args[ 0 ] : timeSeriesFileName : The name of the file to load the time series from.
        /// args[ 1 ] : forecastingSessionFileName : The name of the file to load the training session from.
        /// args[ 2 ] : forecastingLogFileName : The name of the file to save the forecasting log into.
        /// </param>
        public static void Main( string[] args )
        {
            try
            {
                // Create the time series.
                timeSeries = new TimeSeries( args[ 0 ], ' ' );

                // Read the forecasting session.
                forecastingSession = new ForecastingSession( args[ 1 ] );

                // The header of the forecasting log.
                string[] forecastingLogHeader = new string[ 10 ] {
                    "Lags",
                    "Number of effective observations (n)",
                    "Number of hidden neurons",
                    "Number of parameters (p)",
                    "RSS (within-sample)",
                    "RSD (within-sample)",
                    "AIC",
                    "BIC",
                    "RSS (out-of-sample)",
                    "RSD (out-of-sample)"
                };

                // Write the forecasting log.
                forecastingLog = new ForecastingLog( args[ 2 ], forecastingLogHeader );

                while (!forecastingSession.EndOfStream)
                {
                    Forecast( forecastingSession, forecastingLog );
                }
            }
            catch
            {
                Console.WriteLine( "Usage: MarketForecaster .timeseries .forecastingsession .forecastinglog" );
            }
            finally
            {
                // Close the forecasting session.
                if (forecastingSession != null)
                {
                    forecastingSession.Close();
                }

                // Close the forecasting log.
                if (forecastingLog != null)
                {
                    forecastingLog.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forecastingSession"></param>
        /// <param name="forecastingLog"></param>
        private static void Forecast( ForecastingSession forecastingSession, ForecastingLog forecastingLog )
        {
            // Step 0 : Read from the Forecasting Session
            // ------------------------------------------

            string[] words = forecastingSession.Read();

            // The size of the test set.
            string testSetSizeString = words[ 0 ].Trim();
            int testSetSize = Int32.Parse( testSetSizeString );

            // The lags.
            string lagsString = words[ 1 ].Trim();
            string[] lagStrings = lagsString.Split( ',' );
            int[] lags = new int[ lagStrings.Length ];
            for (int i = 0; i < lags.Length; i++)
            {
                lags[ i ] = Int32.Parse( lagStrings[ i ] );
            }

            // The leaps.
            string leapsString = words[ 2 ].Trim();
            string[] leapStrings = leapsString.Split( ',' );
            int[] leaps = new int[ leapStrings.Length ];
            for (int i = 0; i < leaps.Length; i++)
            {
                leaps[ i ] = Int32.Parse( leapStrings[ i ] );
            }
            
            // The numner of hidden neurons.
            string hiddenNeuronCountString = words[ 3 ].Trim();
            int hiddenNeuronCount = Int32.Parse( hiddenNeuronCountString );

            // DEBUG : "Lags; Number of hidden neurons"
            Console.WriteLine( lagsString + "; " + hiddenNeuronCountString );

            // Step 1 : Alternative A : Building a training set (and a testing set) manually
            // -----------------------------------------------------------------------------

            // The training set.
            TrainingSet trainingSet = timeSeries.BuildTrainingSet( lags, leaps );

            // The testing set.
            TrainingSet testSet = trainingSet.SeparateTestSet( trainingSet.Size - testSetSize, testSetSize );

            // Step 2 : Building a blueprint of a network
            // ------------------------------------------

            // The input layer blueprint.
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint( lags.Length );

            // The hidden layer blueprint.
            ActivationLayerBlueprint hiddenlayerBlueprint = new ActivationLayerBlueprint( hiddenNeuronCount );

            // The output layer blueprint.
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint( leaps.Length, new LinearActivationFunction() );

            // The network blueprint.
            NetworkBlueprint networkBlueprint = new NetworkBlueprint( inputLayerBlueprint, hiddenlayerBlueprint, outputLayerBlueprint );

            // Step 3 : Building a network
            // ---------------------------

            // The network.
            Network network = new Network( networkBlueprint );

            // Step 4 : Building a teacher
            // ---------------------------

            BackpropagationTeacher teacher = new BackpropagationTeacher( trainingSet, null, testSet );

            // Step 5 : Training the network
            // -----------------------------

            int maxRunCount = 10;
            int maxIterationCount = 10000;
            double maxTolerableNetworkError = 0.0;
            TrainingLog tl = teacher.Train( network, maxRunCount, maxIterationCount, maxTolerableNetworkError );

            // Step 6 : Write into the Forecasting Log
            // ---------------------------------------

            words = new string[ 10 ] {
                lagsString,
                trainingSet.Size.ToString(),
                hiddenNeuronCountString,
                network.SynapseCount.ToString(),
                tl.RSS_TrainingSet.ToString(),
                tl.RSD_TrainingSet.ToString(),
                tl.AIC.ToString(),
                tl.BIC.ToString(),
                tl.RSS_TestSet.ToString(),
                tl.RSD_TestSet.ToString()
            };
            forecastingLog.Write( words );

            // DEBUG : "RSS (within-sample); RSD (within-sample); AIC; BIC; RSS (out-of-sample); RSD (out-of-sample)"
            Console.WriteLine( tl.RSS_TrainingSet.ToString() + "; " + tl.RSD_TrainingSet.ToString() + "; " + tl.AIC.ToString() + "; " + tl.BIC.ToString() + "; " + tl.RSS_TestSet.ToString() + "; " + tl.RSD_TestSet.ToString() );
        }
    }
}
