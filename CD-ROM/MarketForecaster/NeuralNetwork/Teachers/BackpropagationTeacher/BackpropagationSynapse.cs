using NeuralNetwork.Connectors;
using NeuralNetwork.Layers;
using NeuralNetwork.Neurons;
using NeuralNetwork.Synapses;


namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation synapse.
    /// </summary>
    internal class BackpropagationSynapse
        : SynapseDecorator
    {
        #region Private instance fields

        /// <summary>
        /// The gradient of the synapse.
        /// </summary>
        private double gradient;

        /// <summary>
        /// The error of the synapse.
        /// </summary>
        private double error;

        /// <summary>
        /// The change of weight.
        /// </summary>
        private double weightChange;

        /// <summary>
        /// The previous change of weight (used with momentum modification).
        /// </summary>
        private double previousWeightChange;

        /// <summary>
        /// The learning rate of the synapse.
        /// </summary>
        private double learningRate;

        private double k;
        
        #endregion // Private instance fields

        #region Public instance constructors

        /// <summary>
        /// Creates a new backpropagation synapse by decorating a synapse.
        /// </summary>
        /// <param name="synapse">The synapse to be doecorated as backpropagation synapse.</param>
        public BackpropagationSynapse( ISynapse synapse,IConnector parentConnector )
            : base( synapse, parentConnector )
        {
            k = 1.01;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Connects the (backpropagation) synapse.
        /// 
        /// A synapse is said to be connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        public override void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// Disconnects the (backpropagation) synapse.
        /// 
        /// A synapse is said to be disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        public override void Disconnect()
        {
            Disconnect( this );
        }

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            gradient = 0.0;
            error = 0.0;
            weightChange = 0.0;
            previousWeightChange = 0.0;
            learningRate = 0.001;
        }

        /// <summary>
        /// Resets the error of the synapse.
        /// </summary>
        public void ResetError()
        {
            error = 0.0;
        }

        /// <summary>
        /// Calculatates the gradient of the synapse using the backpropagation algorithm.
        /// <em>(2.17)</em>
        /// </summary>
        public void CalculateGradient()
        {
            BackpropagationNeuron targetNeuron = TargetNeuron as BackpropagationNeuron;
            gradient = targetNeuron.Gradient * targetNeuron.Derivative * SourceNeuron.Output;
        }

        /// <summary>
        /// Updates the error of the synapse.
        /// </summary>
        public void UpdateError()
        {
            error += gradient;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            BackpropagationNeuron targetNeuron = TargetNeuron as BackpropagationNeuron;
            gradient = targetNeuron.Gradient * targetNeuron.Derivative * SourceNeuron.Output;
            error += gradient;
        }

        /// <summary>
        /// Updates the weight.
        /// </summary>
        public void UpdateWeight()
        {
            // Update the weight change and the previous weight change.
            previousWeightChange = weightChange;
            weightChange = -learningRate * error;

            // Update the weight.
            Weight += -learningRate * error + (ParentConnector as BackpropagationConnector).Momentum * previousWeightChange;

            // Update the learning rate.
            if (weightChange * previousWeightChange > 0)
            {
                learningRate *= k;
            }
            else
            {
                learningRate /= 2.0;
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation synapse.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation synapse.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public instance methods
    }
}