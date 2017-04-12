using NeuralNetwork.Connectors;
using NeuralNetwork.Layers;
using NeuralNetwork.Networks;
using NeuralNetwork.Synapses;


namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation connector.
    /// </summary>
    internal class BackpropagationConnector
        : ConnectorDecorator
    {
        #region Private instance fields

        /// <summary>
        /// 
        /// </summary>
        private double momentum;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// 
        /// </summary>
        public double Momentum
        {
            get
            {
                return momentum;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new backpropagation connector by decorating a connector.
        /// </summary>
        /// <param name="connector">The connector to be decorated as backpropagation connector.</param>
        /// <param name="parentNetwork">The parent network.</param>
        public BackpropagationConnector(IConnector connector,INetwork parentNetwork)
            : base( connector, parentNetwork )
        {
            // Decorate the synapses.
            for (int i = 0; i < SynapseCount; i++)
            {
                Synapses[i] = new BackpropagationSynapse(Synapses[i],this);
            }

            momentum = 0.9;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Connects the (backpropagation) connector.
        /// 
        /// A connector is said to be connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        public override void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// Disconnects the (backpropagation) connector.
        /// 
        /// A connector is said to be disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        public override void Disconnect()
        {
            Disconnect(this);
        }

        /// <summary>
        /// Resets the error of all synapses in the connector.
        /// </summary>
        public void ResetSynapseErrors()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.ResetError();
            }
        }

        /// <summary>
        /// Calculates the gradient of all synapses in the connector using the backpropagation algorithm.
        /// </summary>
        public void CalculateSynapseGradients()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.CalculateGradient();
            }
        }

        /// <summary>
        /// Updates the error of all synapses in the connector.
        /// </summary>
        public void UpdateSynapseErrors()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateError();
            }
        }

        /// <summary>
        /// Updates the weights of all synapses in the connector.
        /// </summary>
        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateWeight();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation connector.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation connector.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public instance methods
    }
}

