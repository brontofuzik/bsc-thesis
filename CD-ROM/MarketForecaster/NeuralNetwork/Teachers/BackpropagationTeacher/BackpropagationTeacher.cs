using System;
using System.IO;

using NeuralNetwork.Networks;


namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation teacher.
    /// </summary>
    public class BackpropagationTeacher
        : Teacher
    {
        #region Public instance properties

        /// <summary>
        /// Gets the name of the teacher.
        /// </summary>
        /// <value>
        /// The name of the teacher.
        /// </value>
        public override string Name
        {
            get
            {
                return "BackpropagationTeacher";
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new backpropagation teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        public BackpropagationTeacher( TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet )
            : base( trainingSet, validationSet, testSet )
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxRunCount">The maximum number of runs.</param>
        /// <param name="maxIterationCout">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        public TrainingLog Train( INetwork network, int maxRunCount, int maxIterationCout, double maxTolerableNetworkError )
        {
            // The backpropagation parameters.
            int networkErrorUpdateInterval = 100;
            bool accumulatedLearning = true;

            // Setup:
            // Decorate the network as backpropagation network.
            BackpropagationNetwork backpropagationNetwork = new BackpropagationNetwork( network );

            int bestIterationCount = 0;
            double bestNetworkError = backpropagationNetwork.CalculateError( trainingSet );
            double[] bestWeights = backpropagationNetwork.GetWeights();

            int runCount = 0;
            while ((runCount < maxRunCount) && (bestNetworkError > maxTolerableNetworkError))
            {
                int iterationCount;
                double networkError;
                Train( backpropagationNetwork, maxIterationCout, out iterationCount, maxTolerableNetworkError, out networkError, networkErrorUpdateInterval, accumulatedLearning );

                if (networkError < bestNetworkError)
                {
                    bestIterationCount = iterationCount;
                    bestNetworkError = networkError;
                    bestWeights = backpropagationNetwork.GetWeights();
                }

                runCount++;

                // DEBUG
                Console.Write( "." );
            }

            // DEBUG
            Console.WriteLine();

            backpropagationNetwork.SetWeights( bestWeights );

            // Teardown:
            // Undecorate the backpropagation network as network.
            network = backpropagationNetwork.GetDecoratedNetwork();

            // LOGGING
            // -------

            // Create the training log and log the training data.
            TrainingLog trainingLog = new TrainingLog( runCount, bestIterationCount, bestNetworkError );
            
            // Log the network statistics.
            LogNetworkStatistics( trainingLog, network );

            return trainingLog;
        }

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        public override TrainingLog Train( INetwork network, int maxIterationCount, double maxTolerableNetworkError )
        {
            return Train( network, 1, maxIterationCount, maxTolerableNetworkError ); 
        }

        #endregion // Public instance methods

        #region Private instance methods

        /// <summary>
        /// Trains a backpropagation network.
        /// </summary>
        /// <param name="backpropagationNetwork">The backpropagation network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="usedIterationCount">The number of used iterations.</param>
        /// <param name="maxTolerableNetoworError">The maximum tolerable network error.</param>
        /// <param name="minAchievedNetworkError">The minimum achieved network error.</param>
        /// <param name="networkErrorUpdateInterval">The interaval of network error update.</param>
        /// <param name="accumulatedLearning">The accumulated learning flag.</param>
        private void Train( BackpropagationNetwork backpropagationNetwork,
            int maxIterationCount, out int usedIterationCount,
            double maxTolerableNetworkError, out double minAchievedNetworkError,
            int networkErrorUpdateInterval, bool accumulatedLearning
        )
        {
            // 1.
            // Put the discrete time of adaptation t := 0.
            usedIterationCount = 0;

            // 2.
            // Choose randomly w_ji^(0) e <-1, 1>.
            backpropagationNetwork.Initialize();

            // Calculate the error of the network.
            minAchievedNetworkError = backpropagationNetwork.CalculateError( trainingSet );

            while ((usedIterationCount < maxIterationCount) && (minAchievedNetworkError > maxTolerableNetworkError))
            {
                // 3.
                // Increment t := 1 + 1.
                usedIterationCount++;

                // 4.
                // Put E'_ji := 0 for each synapse from i to j.
                if (accumulatedLearning)
                {
                    backpropagationNetwork.ResetSynapseErrors();
                }

                // 5.
                // For each training pattern k = 1, ..., p (taken from the training set in random order) do:
                foreach (TrainingPattern trainingPattern in trainingSet.TrainingPatternsRandomOrder)
                {
                    if (!accumulatedLearning)
                    {
                        backpropagationNetwork.ResetSynapseErrors();
                    }

                    // (a)
                    // Calculate the output of the network y(w^(t - 1), x_k), i.e. the states (outputs) and the inner potentials (inputs)
                    // of all neurons (active regime) for the input x_k of the k-th training pattern using formulas (2.6) and (2.8).
                    backpropagationNetwork.Evaluate( trainingPattern.InputVector );

                    // (b)
                    // By error back-propagation calculate for each non-input neuron j /e X the partial derivation dE_k / dy_j (w^(t - 1))
                    // of the partial error function of the k-th training pattern according to the state (output) of the neuron j in point w^(t - 1)
                    // using formulas (2.18) and (2.19) (use the values of the states and the inner potentials calculated in step 5a).
                    //backpropagationNetwork.CalculateNeuronGradients( trainingPattern.OutputVector );

                    // (c)
                    // Calculate gradient of the partial error function dE_k / dw_ji (w^(t - 1)) in point w^(t - 1) using formula (2.17)
                    // using the partial derivations (errors) dE_k / dy_j (w^(t - 1)) calculated in step 5b.
                    //backpropagationNetwork.CalculateSynapseGradients();

                    // (d)
                    // Add E'_ji := E'_ji + dE_k / dw_ji (w^(t - 1)).
                    //backpropagationNetwork.UpdateSynapseErrors();

                    // Replaces three steps - (b), (c) and (d) - with one.
                    backpropagationNetwork.Backpropagate( trainingPattern.OutputVector );

                    if (!accumulatedLearning)
                    {
                        backpropagationNetwork.UpdateSynapseWeights();
                    }
                }

                // 6.
                // {The following is valid E'_ji = dE / dw_ji (w^(t - 1)).}
                // According to (2.12) put delta w_ji^(t) := -learning_rate * E'_ji.

                // 7.
                // According to (2.11) put w_ji^(t) := w_ji^(t - 1) + delta w_ji^(t).
                if (accumulatedLearning)
                {
                    backpropagationNetwork.UpdateSynapseWeights();
                }

                // Update the error of the network.
                if (usedIterationCount % networkErrorUpdateInterval == 0)
                {
                    minAchievedNetworkError = backpropagationNetwork.CalculateError( trainingSet );
                }
            }
        }

        #endregion // Private instance methods
    }
}