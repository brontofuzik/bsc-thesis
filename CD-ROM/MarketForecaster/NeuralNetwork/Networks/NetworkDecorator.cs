using System.Collections.Generic;

using NeuralNetwork.Connectors;
using NeuralNetwork.Layers;
using NeuralNetwork.Teachers;


namespace NeuralNetwork.Networks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NetworkDecorator
        : INetwork
    {
        #region Protected instance fields

        /// <summary>
        /// 
        /// </summary>
        protected INetwork decoratedNetwork;

        #endregion // Protected instance fields

        #region Public insatnce properties

        /// <summary>
        /// Gets the network blueprint.
        /// </summary>
        /// 
        /// <value>
        /// The network blueprint.
        /// </value>
        public virtual NetworkBlueprint Blueprint
        {
            get
            {
                return decoratedNetwork.Blueprint;
            }
        }

        /// <summary>
        /// Gets the bias layer.
        /// </summary>
        /// 
        /// <value>
        /// The bias layer.
        /// </value>
        public virtual InputLayer BiasLayer
        {
            get
            {
                return decoratedNetwork.BiasLayer;
            }
        }

        /// <summary>
        /// Gets the input layer.
        /// </summary>
        /// 
        /// <value>
        /// The input layer.
        /// </value>
        public virtual InputLayer InputLayer
        {
            get
            {
                return decoratedNetwork.InputLayer;
            }
        }

        /// <summary>
        /// Gets the hidden layers.
        /// </summary>
        /// 
        /// <value>
        /// The layers.
        /// </value>
        public virtual List<IActivationLayer> HiddenLayers
        {
            get
            {
                return decoratedNetwork.HiddenLayers;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int HiddenLayerCount
        {
            get
            {
                return decoratedNetwork.HiddenLayerCount;
            }
        }

        /// <summary>
        /// Gets the output layer.
        /// </summary>
        /// 
        /// <value>
        /// The output layer.
        /// </value>
        public virtual IActivationLayer OutputLayer
        {
            get
            {
                return decoratedNetwork.OutputLayer;
            }
            set
            {
                decoratedNetwork.OutputLayer = value;
            }
        }

        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        ///
        /// <value>
        /// The number of layers.
        /// </value>
        public virtual int LayerCount
        {
            get
            {
                return decoratedNetwork.LayerCount;
            }
        }

        /// <summary>
        /// Gets the list of connectors comrprising this network.
        /// </summary>
        /// 
        /// <value>
        /// The list of connectors comrprising this network.
        /// </value>
        public virtual List<IConnector> Connectors
        {
            get
            {
                return decoratedNetwork.Connectors;
            }
        }

        /// <summary>
        /// Gets the number of connectors in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of connectors in this network.
        /// </value>
        public virtual int ConnectorCount
        {
            get
            {
                return decoratedNetwork.ConnectorCount;
            }
        }

        /// <summary>
        /// Gets the number of synapses in this network.
        /// </summary>
        ///
        /// <value>
        /// The number of synapses in this network.
        /// </value>
        public virtual int SynapseCount
        {
            get
            {
                return decoratedNetwork.SynapseCount;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoratedNetwork"></param>
        public NetworkDecorator(INetwork decoratedNetwork)
        {
            this.decoratedNetwork = decoratedNetwork;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Connects the network.
        /// 
        /// A network is said to be connected if:
        /// 
        /// 1. its layers are connected and
        /// 2. its connectors are connected.
        /// </summary>
        public virtual void Connect()
        {
            decoratedNetwork.Connect();
        }

        /// <summary>
        /// Disconnects the network.
        /// 
        /// A network is said to be disconnected if:
        /// 
        /// 1. its layers are disconnected and
        /// 2. its connectors are disconnected.
        /// </summary>
        public virtual void Disconnect()
        {
            decoratedNetwork.Disconnect();
        }

        /// <summary>
        /// Returns the decorated network.
        /// </summary>
        /// 
        /// <returns>
        /// The decorated network.
        /// </returns>
        public virtual INetwork GetDecoratedNetwork()
        {
            // 1. Disconnect the netowork.
            Disconnect();

            // 2. Undecorate the network components.
            // 2.1. Undecorate the layers.
            // 2.1.1. Undecorate the bias layer.
            BiasLayer.ParentNetwork = decoratedNetwork;
            
            // 2.1.2. Undecorate the input layer.
            InputLayer.ParentNetwork = decoratedNetwork;

            // 2.1.3. Undecorate the hidden layers.
            for (int i = 0; i < HiddenLayerCount; i++)
            {
                HiddenLayers[ i ] = (HiddenLayers[ i ] as ActivationLayerDecorator).GetDecoratedActivationLayer(decoratedNetwork);
            }

            // 2.1.4. Undecorate the output layer.
            OutputLayer = (OutputLayer as ActivationLayerDecorator).GetDecoratedActivationLayer(decoratedNetwork);
            
            // 2.2. Undecorate the connectors and their synapses.
            for (int i = 0; i < ConnectorCount; i++)
            {
                Connectors[ i ] = (Connectors[ i ] as ConnectorDecorator).GetDecoratedConnector(decoratedNetwork);
            }

            // 3. Connect the network.
            Connect();

            return decoratedNetwork;
        }

        /// <summary>
        /// Gets the layer (specified by its index).
        /// </summary>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <returns>
        /// The layer.
        /// </returns>
        public virtual ILayer GetLayerByIndex(int layerIndex)
        {
            return decoratedNetwork.GetLayerByIndex(layerIndex);
        }

        /// <summary>
        /// Initializes the network.
        /// </summary>
        public virtual void Initialize()
        {
            decoratedNetwork.Initialize();
        }

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>
        /// The output vector.
        /// </returns>
        public virtual double[] Evaluate(double[] inputVector)
        {
            return decoratedNetwork.Evaluate(inputVector);
        }

        /// <summary>
        /// Calculate the error of the network.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <returns>
        /// The error of the network.
        /// </returns>
        public virtual double CalculateError( TrainingSet trainingSet )
        {
            return decoratedNetwork.CalculateError( trainingSet );
        }

        /// <summary>
        /// Saves the weights of the network to a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to save the weights to.</param>
        public virtual void SaveWeights(string weightsFileName)
        {
            decoratedNetwork.SaveWeights(weightsFileName);
        }

        /// <summary>
        /// Loads the weights of the network from a file.
        /// </summary>
        /// 
        /// <param name="weightsFileName">The name of the file to load the weights from.</param>
        public virtual void LoadWeights(string weightsFileName)
        {
            decoratedNetwork.LoadWeights(weightsFileName);
        }

        /// <summary>
        /// Gets the weights of the network (as an array).
        /// </summary>
        /// <returns>
        /// The weights of the network (as an array).
        /// </returns>
        public virtual double[] GetWeights()
        {
            return decoratedNetwork.GetWeights();
        }

        /// <summary>
        /// Sets the weights of the network (as an  array).
        /// </summary>
        /// <param name="weights">The weights of the network (as an array).</param>
        public virtual void SetWeights( double[] weights )
        {
            decoratedNetwork.SetWeights( weights );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return decoratedNetwork.ToString();
        }

        #endregion // Public instance methods
    }
}
