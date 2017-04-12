using System;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.ActivationFunctions;
using NeuralNetwork.Networks;
using NeuralNetwork.Neurons;


namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation layer.
    /// </summary>
    internal class BackpropagationLayer
        : ActivationLayerDecorator
    {
        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer">The (activation) layer to be decorated as backpropagation (activation) layer.</param>
        /// <param name="parnetNetwork">The parnet network.</param>
        public BackpropagationLayer(IActivationLayer activationLayer, INetwork parnetNetwork)
            : base(activationLayer,parnetNetwork)
        {
            // Ensure the activation function of the neuron is derivable.
            if (!(ActivationFunction is IDerivableActivationFunction))
            {
                // TODO: Throw an exception informing the client that in order for the neuron to undergo training
                // using the error backpropagation algorithm, its activation function has to be derivable
                // (i.e. it has to implement the IDerivableActivationFunction interface)
                throw new Exception();
            }

            // Decorate the neurons.
            for (int i = 0; i < NeuronCount; i++)
            {
                Neurons[ i ] = new BackpropagationNeuron(Neurons[i],this);
            }
        }

        #endregion // Public instance constructors

        #region Public insatnce methods

        /// <summary>
        /// Calculates the gradients of the neurons in the layer using the backpropagation algorithm.
        /// Evaluates the errors of all neurons comprising the (output) layer using the formula:
        /// </summary>
        /// <param name="desiredOutputVector">The desired output vector.</param>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>desiredOutputVector</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Condition: the length of <c>outputVector</c> does not match the number of neurons.
        /// </exception>
        public void CalculateNeuronGradients( double[] desiredOutputVector )
        {
            // Validate the arguments.
            if (desiredOutputVector == null)
            {
                throw new ArgumentNullException( "desiredOutputVector" );
            }

            // Validate the length of the desired output vector.
            if (desiredOutputVector.Length != NeuronCount)
            {
                throw new ArgumentException( "desiredOutputException" );
            }

            int i = 0;
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.CalculateGradient( desiredOutputVector[ i++ ] );
            }
        }

        /// <summary>
        /// Calculates the gradients of the neurons in the layer using the backpropagation algorithm.
        /// </summary>
        public void CalculateNeuronGradients()
        {
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.CalculateGradient();
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate( double[] desiredOutputVector )
        {
            // Validate the arguments.
            if (desiredOutputVector == null)
            {
                throw new ArgumentNullException( "desiredOutputVector" );
            }

            // Validate the length of the desired output vector.
            if (desiredOutputVector.Length != NeuronCount)
            {
                throw new ArgumentException( "desiredOutputException" );
            }

            int i = 0;
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.Backpropagate( desiredOutputVector[ i++ ] );
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            foreach (BackpropagationNeuron neuron in Neurons)
            {
                neuron.Backpropagate();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation layer.
        /// </summary>
        /// <returns>
        /// A string representation of the backpropagation layer.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public insatnce methods
    }
}