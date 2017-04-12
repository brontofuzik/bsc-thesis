using System;

using NeuralNetwork.Layers;
using NeuralNetwork.Layers.ActivationFunctions;
using NeuralNetwork.Neurons;


namespace NeuralNetwork.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation neuron.
    /// </summary>
    internal class BackpropagationNeuron
        : ActivationNeuronDecorator
    {
        #region Private instance fields

        /// <summary>
        /// The gradient of the neuron.
        /// </summary>
        private double gradient;
        
        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the gradient of the neuron.
        /// </summary>
        public double Gradient
        {
            get
            {
                return gradient;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Derivative
        {
            get
            {
                return ((ParentLayer as IActivationLayer).ActivationFunction as IDerivableActivationFunction).EvaluateDerivative( Input );
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neuron">The (activation) neuron to be decorated as backpropagation (activation) neuron.</param>
        /// <param name="parentLayer">The parent layer.</param>
        public BackpropagationNeuron(IActivationNeuron activationNeuron,IActivationLayer parentLayer)
            : base(activationNeuron,parentLayer)
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            gradient = 0.0;
        }
        
        /// <summary>
        /// Calculates the gradient using the backpropagation algorithm.
        /// <em>(2.18)</em>
        /// </summary>
        /// <param name="desiredOutput">The desired output.</param>
        public void CalculateGradient( double desiredOutput )
        {
            gradient = Output - desiredOutput;
        }

        /// <summary>
        /// Calculates the gradient using the backpropagation algorithm.
        /// <em>(2.19)</em>
        /// </summary>
        public void CalculateGradient()
        {
            // Initialize the error.
            gradient = 0.0;

            // Back-propagate the weighted error from the target neuron to the source neuron via this synapse. 
            foreach (BackpropagationSynapse targetSynapse in TargetSynapses)
            {
                BackpropagationNeuron targetNeuron = targetSynapse.TargetNeuron as BackpropagationNeuron;
                gradient += targetNeuron.Gradient * targetNeuron.Derivative * targetSynapse.Weight;
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate( double desiredOutput )
        {
            gradient = Output - desiredOutput;

            foreach (BackpropagationSynapse synapse in SourceSynapses)
            {
                synapse.Backpropagate();
            }
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            // Initialize the error.
            gradient = 0.0;

            // Back-propagate the weighted error from the target neuron to the source neuron via this synapse. 
            foreach (BackpropagationSynapse targetSynapse in TargetSynapses)
            {
                BackpropagationNeuron targetNeuron = targetSynapse.TargetNeuron as BackpropagationNeuron;
                gradient += targetNeuron.Gradient * targetNeuron.Derivative * targetSynapse.Weight;
            }

            foreach (BackpropagationSynapse synapse in SourceSynapses)
            {
                synapse.Backpropagate();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the backpropagation neuron.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public instance methods
    }
}