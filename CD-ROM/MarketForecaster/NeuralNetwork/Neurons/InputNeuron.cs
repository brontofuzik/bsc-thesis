﻿using System;
using System.Collections.Generic;

using NeuralNetwork.Layers;
using NeuralNetwork.Synapses;


namespace NeuralNetwork.Neurons
{
    /// <summary>
    /// An input neuron.
    /// </summary>
    public class InputNeuron
        : INeuron
    {
        #region Private instance fields

        /// <summary>
        /// 
        /// </summary>
        private double output;
        
        /// <summary>
        /// 
        /// </summary>
        private List<ISynapse> targetSynapses;
        
        /// <summary>
        /// 
        /// </summary>
        private InputLayer parentLayer;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets or sets the output of the neuron.
        /// </summary>
        /// 
        /// <value>
        /// The output of the neuron.
        /// </value>
        public double Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ISynapse> TargetSynapses
        {
            get
            {
                return targetSynapses;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILayer ParentLayer
        {
            get
            {
                return parentLayer;
            }
            set
            {
                parentLayer = value as InputLayer;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new input neuron.
        /// </summary>
        /// <param name="parentLayer">The parent layer.</param>
        public InputNeuron(InputLayer parentLayer)
        {
            targetSynapses = new List<ISynapse>();

            // Validate the parent layer.
            Utilities.ObjectNotNull(parentLayer, "parentLayer");
            this.parentLayer = parentLayer;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public void Initialize()
        {
            output = 0.0;
        }

        /// <summary>
        /// Returns a string representation of the input neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the input neuron.
        /// </returns>
        public override string ToString()
        {
            return String.Format( "IN(" + output.ToString( "F2" )+ ")" );
        }

        #endregion // Public instance methods
    }
}
