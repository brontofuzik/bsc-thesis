using System;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.ActivationFunctions;
using NeuralNetwork.Networks;
using NeuralNetwork.Teachers;
using NeuralNetwork.Teachers.AntColonyOptimizationTeacher;
using NeuralNetwork.Teachers.BackpropagationTeacher;
using NeuralNetwork.Teachers.GeneticAlgorithmTeacher;
using NeuralNetwork.Teachers.SimulatedAnnealingTeacher;

using SimulatedAnnealing;

namespace NeuralNetwork
{
    /// <summary>
    /// The (artificial) neural network test suite.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The test harness.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main( string[] args )
        {
            // The training set.
            TrainingSet trainingSet = new TrainingSet( "XOR.trainingSet" );

            // The blueprint of the network.
            NetworkBlueprint networkBlueprint = new NetworkBlueprint(
                new LayerBlueprint( trainingSet.InputVectorLength ),
                new ActivationLayerBlueprint[ 1 ] { new ActivationLayerBlueprint( 2, new LogisticActivationFunction() ) },
                new ActivationLayerBlueprint( trainingSet.OutputVectorLength, new LogisticActivationFunction() )
            );

            // The network.
            INetwork network = new Network( networkBlueprint );

            Test( 1, "A backpropagation (BP) teacher", new BackpropagationTeacher( trainingSet, null, null ), network, 100000, 1e-3 );
            Console.WriteLine();

            Test( 2, "The genetic algorithm (GA) teacher", new GeneticAlgorithmTeacher( trainingSet, null, null ), network, 1000, 1e-3 );
            Console.WriteLine();

            Test( 3, "The simulated annealing (SA) teacher", new SimulatedAnnealingTeacher( trainingSet, null, null ), network, 1000000, 1e-3 );
            Console.WriteLine();
            
            Test( 4, "The ant colony optimization (ACO) teacher", new AntColonyOptimizationTeacher( trainingSet, null, null ), network, 1000, 1e-3 );
            Console.WriteLine();
        }

        /// <summary>
        /// Test a teacher.
        /// </summary>
        /// <param name="testNumber">The number of the test.</param>
        /// <param name="testDescription">The description of the test.</param>
        /// <param name="teacher">The teacher to test.</param>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        private static void Test( int testNumber, string testDescription, ITeacher teacher, INetwork network, int maxIterationCount, double maxTolerableNetworkError )
        {
            // Print the number of the test and its description.
            Console.WriteLine( "Test " + testNumber + " : " + testDescription );

            // Run the teacher (i.e. train the network).
            DateTime startTime = DateTime.Now;
            TrainingLog trainingLog = teacher.Train( network, maxIterationCount, maxTolerableNetworkError );
            DateTime endTime = DateTime.Now;

            // Print the results.
            Console.WriteLine( "Test " + testNumber + " : Duration : " + (endTime - startTime) );
            Console.WriteLine( "Test " + testNumber + " : Number of iterations taken : " + trainingLog.IterationCount );
            Console.WriteLine( "Test " + testNumber + " : Network error achieved : " + trainingLog.NetworkError );
            foreach (TrainingPattern trainingPattern in teacher.TrainingSet.TrainingPatterns)
            {
                double[] outputVector = network.Evaluate( trainingPattern.InputVector );
                Console.WriteLine(
                    TrainingPattern.VectorToString( trainingPattern.InputVector ) +
                    " -> " +
                    TrainingPattern.VectorToString( outputVector )
                );
            }
        }
    }
}
