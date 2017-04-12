using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NeuralNetwork.Connectors;
using NeuralNetwork.Layers;
using NeuralNetwork.Neurons;
using NeuralNetwork.Synapses;
using NeuralNetwork.Teachers;


namespace NeuralNetwork.Networks
{
    /// <summary>
    /// A (multilayer feedforward) neural network.
    /// </summary>
    public class Network
        : INetwork
    {
        #region Private instance fields

        /// <summary>
        /// The blueprint of the network.
        /// </summary>
        private NetworkBlueprint blueprint;

        /// <summary>
        /// The bias layer of the network.
        /// </summary>
        private InputLayer biasLayer;

        /// <summary>
        /// The input layer of the network.
        /// </summary>
        private InputLayer inputLayer;

        /// <summary>
        /// The hidden layers of the network.
        /// </summary>
        private List<IActivationLayer> hiddenLayers;

        /// <summary>
        /// The output layer of the network.
        /// </summary>
        private IActivationLayer outputLayer;

        /// <summary>
        /// The connectors of the network.
        /// </summary>
        private List<IConnector> connectors;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the network blueprint.
        /// </summary>
        /// 
        /// <value>
        /// The network blueprint.
        /// </value>
        public NetworkBlueprint Blueprint
        {
            get
            {
                return blueprint;
            }
        }

        /// <summary>
        /// Gets the bias layer.
        /// </summary>
        /// 
        /// <value>
        /// The bias layer.
        /// </value>
        public InputLayer BiasLayer
        {
            get
            {
                return biasLayer;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// 
        /// <value>
        /// The input layer.
        /// </value>
        public InputLayer InputLayer
        {
            get
            {
                return inputLayer;
            }
        }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        /// 
        /// <value>
        /// The layers.
        /// </value>
        public List<IActivationLayer> HiddenLayers
        {
            get
            {
                return hiddenLayers;
            }
        }

        /// <summary>
        /// Gets the number of hidden layers.
        /// </summary>
        /// 
        /// <value>
        /// The numner of hidden layers.
        /// </value>
        public int HiddenLayerCount
        {
            get
            {
                return hiddenLayers.Count;
            }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// 
        /// <value>
        /// The output layer.
        /// </value>
        public IActivationLayer OutputLayer
        {
            get
            {
                return outputLayer;
            }
            set
            {
                outputLayer = value;
            }
        }

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        ///
        /// <value>
        /// The number of layers.
        /// </value>
        public int LayerCount
        {
            get
            {
                return 1 + hiddenLayers.Count + 1;
            }
        }

        /// <summary>
        /// Gets the list of connectors comrprising this network.
        /// </summary>
        /// 
        /// <value>
        /// The list of connectors comrprising this network.
        /// </value>
        public List<IConnector> Connectors
        {
            get
            {
                return connectors;
            }
        }

        /// <summary>
        /// Gets the number of connectors in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of connectors in this network.
        /// </value>
        public int ConnectorCount
        {
            get
            {
                return connectors.Count;
            }
        }

        /// <summary>
        /// Gets the number of synapses in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of synapses in this network.
        /// </value>
        public int SynapseCount
        {
            get
            {
                int synapseCount = 0;
                foreach (IConnector connector in connectors)
                {
                    synapseCount += connector.SynapseCount;
                }
                return synapseCount;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new (artificial neural) network (from its blueprint).
        /// </summary>
        /// 
        /// <param name="blueprint">The blueprint of the network.</param>
        public Network(NetworkBlueprint blueprint)
        {
            // 0. Validate the blueprint.
            Utilities.ObjectNotNull( blueprint, "blueprint" );
            this.blueprint = blueprint;

            // 1. Create the network components.
            // 1.1. Create the layers.
            // 1.1.1. Create the bias layer.
            biasLayer = new InputLayer( this.blueprint.BiasLayerBlueprint, this );
            biasLayer.SetInputVector( new double[] { 1.0 } );

            // 1.1.2. Create the input layer.
            inputLayer = new InputLayer( this.blueprint.InputLayerBlueprint, this );

            // 1.1.3. Create the hidden layers.
            hiddenLayers = new List< IActivationLayer >( this.blueprint.HiddenLayerCount );
            foreach (ActivationLayerBlueprint hiddenLayerBlueprint in this.blueprint.HiddenLayerBlueprints )
            {
                IActivationLayer hiddenLayer = new ActivationLayer( hiddenLayerBlueprint, this );
                hiddenLayers.Add( hiddenLayer );
            }

            // 1.1.4. Create the output layer.
            outputLayer = new ActivationLayer( this.blueprint.OutputLayerBlueprint, this );

            // 1.2 Create the connectors.
            connectors = new List< IConnector >( this.blueprint.ConnectorCount );
            foreach (ConnectorBlueprint connectorBlueprint in this.blueprint.ConnectorBlueprints)
            {
                IConnector connector = new Connector(connectorBlueprint, this);
                connectors.Add(connector);
            }

            // 2. Connect the network.
            Connect();
        }

        #endregion // Public Instance Constructors

        #region Public insatnce methods

        /// <summary>
        /// Connects the network.
        /// 
        /// A network is said to be connected if:
        /// 
        /// 1. its connectors are connected.
        /// </summary>
        public void Connect()
        {
            // 1. Connect the connectors.
            foreach (IConnector connector in connectors)
            {
                connector.Connect();
            }
        }

        /// <summary>
        /// Disconnects the network.
        /// 
        /// A network is said to be disconnected if:
        /// 
        /// 1. its connectors are disconnected.
        /// </summary>
        public void Disconnect()
        {
            // 1. Disconnect the connectors.
            foreach (IConnector connector in connectors)
            {
                connector.Disconnect();
            }
        }

        /// <summary>
        /// Gets the layer (specified by its index).
        /// </summary>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <returns>
        /// The layer.
        /// </returns>
        public ILayer GetLayerByIndex( int layerIndex )
        {
            if (layerIndex == -1)
            {
                return biasLayer;
            }
            else if (layerIndex == 0)
            {
                return inputLayer;
            }
            else if (layerIndex == LayerCount - 1)
            {
                return outputLayer;
            }
            else
            {
                return hiddenLayers[ layerIndex - 1 ];
            }
        }

        /// <summary>
        /// Initializes the network.
        /// </summary>
        public void Initialize()
        {
            biasLayer.Initialize();
            inputLayer.Initialize();
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                hiddenLayer.Initialize();
            }
            outputLayer.Initialize();

            foreach (IConnector connector in connectors)
            {
                connector.Initialize();
            }
        }

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>
        /// The output vector.
        /// </returns>
        public double[] Evaluate( double[] inputVector )
        {
            SetInputVector(inputVector);

            Evaluate();

            return GetOutputVector();
        }

        /// <summary>
        /// Calculate the error of the network.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public double CalculateError( TrainingSet trainingSet )
        {
            // Calculate the network error with respect to all training patterns (the whole training set).
            double trainingSetError = 0.0;
            foreach (TrainingPattern trainingPattern in trainingSet.TrainingPatterns)
            {
                double[] outputVector = Evaluate( trainingPattern.InputVector );
                double[] desiredOutputVector = trainingPattern.OutputVector;

                // Calculate the network error with respect to one training pattern.
                double trainingPatternError = 0;
                for (int i = 0; i < outputVector.Length; i++)
                {
                    trainingPatternError += Math.Pow( (outputVector[ i ] - desiredOutputVector[ i ]), 2 );
                }

                trainingSetError += 0.5 * trainingPatternError;
            }
            return trainingSetError;
        }
 
        /// <summary>
        /// Saves the weights of the network to a file.
        /// </summary>
        /// 
        /// <param name="fileName">The name of the file to save the weights to.</param>
        public void SaveWeights( string fileName )
        {
            // Open the file for writing.
            TextWriter fileWriter = new StreamWriter( fileName );

            // Write the line containing the numbers of neurons in the layers.
            StringBuilder lineSB = new StringBuilder();
            lineSB.Append( inputLayer.NeuronCount + " " );
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                lineSB.Append( hiddenLayer.NeuronCount + " " );
            }
            lineSB.Append( outputLayer.NeuronCount );
            string line = lineSB.ToString();

            fileWriter.WriteLine( line );

            // Write the blank line.
            fileWriter.WriteLine();

            // 1. Save the weights of source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    lineSB = new StringBuilder();
                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        lineSB.Append( hiddenNeuron.SourceSynapses[ i ].Weight + " " );
                    }
                    if (hiddenNeuron.SourceSynapses.Count != 0)
                    {
                        lineSB.Remove( lineSB.Length - 1, 1 );
                    }
                    line = lineSB.ToString();

                    fileWriter.WriteLine( line );
                }

                // Write the blank line.
                fileWriter.WriteLine();
            }

            // 2. Save the weights of source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in outputLayer.Neurons)
            {
                lineSB = new StringBuilder();
                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    lineSB.Append( outputNeuron.SourceSynapses[ i ].Weight + " " );
                }
                if (outputNeuron.SourceSynapses.Count != 0)
                {
                    lineSB.Remove( lineSB.Length - 1, 1 );
                }
                line = lineSB.ToString();

                fileWriter.WriteLine( line );
            }

            // Close the weights file.
            fileWriter.Close();
        }

        /// <summary>
        /// Loads the weights of the network from a file.
        /// </summary>
        /// 
        /// <param name="fileName">The name of the file to load the weights from.</param>
        public void LoadWeights( string fileName )
        {
            // Open the weights file for reading.
            TextReader fileReader = new StreamReader( fileName );
            const char separator = ' ';

            // Read the line containing the numbers of neurons in the layers.
            string line = fileReader.ReadLine();
            string[] words = line.Split( separator );

            // Read the blank line.
            fileReader.ReadLine();

            //
            // 1. Load the weights of the hidden neurons.
            //

            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    line = fileReader.ReadLine();
                    words = line.Split( separator );

                    for (int i = 0; i < hiddenNeuron.SourceSynapses.Count; i++)
                    {
                        hiddenNeuron.SourceSynapses[ i ].Weight = Double.Parse( words[ i ] );
                    }
                }

                // Read the blank line.
                fileReader.ReadLine();
            }

            //
            // 2. Load the weights of the output neurons.
            //

            foreach (IActivationNeuron outputNeuron in outputLayer.Neurons)
            {
                line = fileReader.ReadLine();
                words = line.Split(separator);

                for (int i = 0; i < outputNeuron.SourceSynapses.Count; i++)
                {
                    outputNeuron.SourceSynapses[ i ].Weight = Double.Parse( words[ i ] );
                }
            }

            // Close the weights file.
            fileReader.Close();
        }

        /// <summary>
        /// Gets the weights of the network (as an array).
        /// </summary>
        /// <returns>
        /// The weights of the network (as an array).
        /// </returns>
        public double[] GetWeights()
        {
            List< double > weights = new List< double >();

            // Get the weights of the source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        weights.Add( sourceSynapse.Weight );
                    }
                }
            }

            // Get the weights of the source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in outputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    weights.Add( sourceSynapse.Weight );
                }
            }

            return weights.ToArray();
        }

        /// <summary>
        /// Sets the weights of the network (as an  array).
        /// </summary>
        /// <param name="weights">The weights of the network (as an array).</param>
        public void SetWeights( double[] weights )
        {
            int i = 0;

            // Set the weights of the source synapses of the hidden neurons.
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                foreach (IActivationNeuron hiddenNeuron in hiddenLayer.Neurons)
                {
                    foreach (ISynapse sourceSynapse in hiddenNeuron.SourceSynapses)
                    {
                        sourceSynapse.Weight = weights[ i++ ];
                    }
                }
            }

            // Set the weights of the source synapses of the output neurons.
            foreach (IActivationNeuron outputNeuron in outputLayer.Neurons)
            {
                foreach (ISynapse sourceSynapse in outputNeuron.SourceSynapses)
                {
                    sourceSynapse.Weight = weights[ i++ ];
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the network.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the network.
        /// </returns>
        public override string ToString()
        {
            StringBuilder networkSB = new StringBuilder();
            networkSB.Append( "MLP\n[\n" );
            int layerIndex = 0;

            // The input layer.
            networkSB.Append( layerIndex++ + " : " + inputLayer + "\n" );

            // The hidden layers.
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                networkSB.Append( layerIndex++ + " : " + hiddenLayer + "\n" );
            }

            // The output layer.
            networkSB.Append( layerIndex + " : " + outputLayer + "\n" );

            networkSB.Append( "]" );
            return networkSB.ToString();
        }

        #endregion // Public instance methods

        #region  Private instance methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputVector"></param>
        private void SetInputVector(double[] inputVector)
        {            
            inputLayer.SetInputVector(inputVector);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Evaluate()
        {
            // Evaluate the hidden layers.
            foreach (IActivationLayer hiddenLayer in hiddenLayers)
            {
                hiddenLayer.Evaluate();
            }

            // Evaluate the output layer.
            outputLayer.Evaluate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double[] GetOutputVector()
        {
            return outputLayer.GetOutputVector();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        private void Jitter(double jitterNoiseLimit)
        {
            foreach (Connector connector in connectors)
            {
                connector.Jitter(jitterNoiseLimit);
            }
        }

        #endregion // Private instance methods
    }
}