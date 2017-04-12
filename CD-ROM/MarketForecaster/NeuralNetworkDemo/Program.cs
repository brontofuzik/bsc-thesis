using System;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.ActivationFunctions;
using NeuralNetwork.Networks;
using NeuralNetwork.Teachers;
using NeuralNetwork.Teachers.AntColonyOptimizationTeacher;
using NeuralNetwork.Teachers.BackpropagationTeacher;
using NeuralNetwork.Teachers.GeneticAlgorithmTeacher;

namespace NeuralNetworkDemo
{
    class Program
    {
        static void Main( string[] args )
        {
            Console.WriteLine("{0:.10}, {1}", "Hello", "World");

            // Step 1 : Alternative A : Building a training set manually
            // ---------------------------------------------------------

            int inputVectorLength = 2;
            int outputVectorLength = 1;

            TrainingSet trainingSet = new TrainingSet( inputVectorLength, outputVectorLength );

            TrainingPattern trainingPattern = new TrainingPattern( new double[ 2 ] { 0.0,  0.0 }, new double[ 1 ] { 0.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new TrainingPattern( new double[ 2 ] { 0.0,  1.0 }, new double[ 1 ] { 1.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new TrainingPattern( new double[ 2 ] { 1.0,  0.0 }, new double[ 1 ] { 1.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new TrainingPattern( new double[ 2 ] { 1.0,  1.0 }, new double[ 1 ] { 0.0 } );
            trainingSet.Add( trainingPattern );

            // Step 2 : Building a blueprint of a network
            // ------------------------------------------

            LayerBlueprint inputLayerBlueprint = new LayerBlueprint( inputVectorLength );
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[ 1 ];
            hiddenLayerBlueprints[ 0 ] = new ActivationLayerBlueprint( 2, new LogisticActivationFunction() );
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint( outputVectorLength, new LogisticActivationFunction() );

            NetworkBlueprint networkBlueprint = new NetworkBlueprint( inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint );

            // Step 3 : Building a network
            // ---------------------------

            Network network = new Network( networkBlueprint );
            Console.WriteLine( network.ToString() );

            // Step 4 : Building a teacher
            // ---------------------------

            ITeacher teacher = new AntColonyOptimizationTeacher( trainingSet, null, null );

            // Step 5 : Training the network
            // -----------------------------

            int maxIterationCount = 10000;
            double maxTolerableNetworkError = 1e-3;
            TrainingLog trainingLog = teacher.Train( network, maxIterationCount, maxTolerableNetworkError );

            Console.WriteLine( "Number of runs used : " +trainingLog.RunCount );
            Console.WriteLine( "Number of iterations used : " + trainingLog.IterationCount );
            Console.WriteLine( "Minimum network error achieved : " + trainingLog.NetworkError );

            // Step 6 : Using the trained network
            // ----------------------------------

            foreach (TrainingPattern tp in trainingSet.TrainingPatterns)
            {
                double[] inputVector = tp.InputVector;
                double[] outputVector = network.Evaluate( inputVector );
                Console.WriteLine( tp.ToString() + " -> " + TrainingPattern.VectorToString( outputVector ) );
            }
        }
    }
}