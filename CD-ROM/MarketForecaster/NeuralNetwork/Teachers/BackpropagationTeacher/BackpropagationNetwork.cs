using System;

using NeuralNetwork.Connectors;
using NeuralNetwork.Layers;
using NeuralNetwork.Networks;
using NeuralNetwork.Neurons;
using NeuralNetwork.Synapses;

namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation network.
    /// </summary>
    public class BackpropagationNetwork
        : NetworkDecorator
    {
        #region Public insatnce constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        public BackpropagationNetwork(INetwork network)
            : base(network)
        {
            // 1. Disconnect the network.
            Disconnect();

            // 2. Decorate the network components.
            // 2.1. Decorate the layers.
            // 2.1.1. Decorate the bias layer.
            BiasLayer.ParentNetwork = this;

            // 2.1.2. Decorate the input layer.
            InputLayer.ParentNetwork = this;

            // 2.1.3. Decorate the hidden layers.
            for (int i = 0; i < HiddenLayerCount; i++)
            {
                HiddenLayers[i] = new BackpropagationLayer(HiddenLayers[i], this);
            }

            // 2.1.4. Decorate the output layer.
            OutputLayer = new BackpropagationLayer(OutputLayer, this);

            // 2.2. Decorate the connectors.
            for (int i = 0; i < ConnectorCount; i++)
            {
                Connectors[i] = new BackpropagationConnector(Connectors[i], this);
            }

            // 3. Connect the network.
            Connect();
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Resets the error of the network.
        /// </summary>
        public void ResetSynapseErrors()
        {
            // Reset the error of the connectors.
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.ResetSynapseErrors();
            }
        }

        /// <summary>
        /// Caluclates the gradients of the neurons in the network using the backpropagation algorithm.
        /// </summary>
        /// <param name="desiredOutputVector"></param>
        public void CalculateNeuronGradients( double[] desiredOutputVector )
        {
            // Output layer.
            (OutputLayer as BackpropagationLayer).CalculateNeuronGradients( desiredOutputVector );

            // Hidden layers (backwards).
            for (int i = HiddenLayers.Count - 1; i >= 0; i--)
            {
                (HiddenLayers[ i ] as BackpropagationLayer).CalculateNeuronGradients();
            }
        }

        /// <summary>
        /// Caluclates the gradients of the synapses in the network using the backpropagation algorithm.
        /// </summary>
        public void CalculateSynapseGradients()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.CalculateSynapseGradients();
            }
        }

        /// <summary>
        /// Updates the error of all synapses in the network.
        /// </summary>
        public void UpdateSynapseErrors()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseErrors();
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate( double[] desiredOutputVector )
        {
            // Output layer.
            (OutputLayer as BackpropagationLayer).Backpropagate( desiredOutputVector );

            // Hidden layers (backwards).
            for (int i = HiddenLayers.Count - 1; i >= 0; i--)
            {
                (HiddenLayers[i] as BackpropagationLayer).Backpropagate();
            }
        }

        /// <summary>
        /// Updates the weights of all synapses in the network.
        /// </summary>
        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseWeights();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation network.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation network.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public insatnce methods
    }
}

