using System;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace NeuralNetwork.Teachers
{
    /// <remarks>
    /// <para>
    /// A (labeled) training set.
    /// </para>
    /// <para>
    /// Definition: A (labeled) training set is a set of (labeled) training patterns.
    /// </para>
    /// </remarks>
    public class TrainingSet
    {
        #region Private instance fields

        /// <summary>
        /// The input vector length.
        /// </summary>
        private int inputVectorLength;

        /// <summary>
        /// The output vector length.
        /// </summary>
        private int outputVectorLength;

        /// <summary>
        /// The training patterns.
        /// </summary>
        private List< TrainingPattern > trainingPatterns;

        #endregion // Private instacne fields

        #region Public instance properties

        /// <summary>
        /// Gets the training patterns in the training set. 
        /// </summary>
        /// <value>
        /// The training patterns in the training set.
        /// </value>
        public List< TrainingPattern > TrainingPatterns
        {
            get
            {
                return trainingPatterns;
            }
        }

        /// <summary>
        ///  Gets the training patterns in a random order.
        /// </summary>
        /// <value>
        /// The training patterns in a random order.
        /// </value>
        public List< TrainingPattern > TrainingPatternsRandomOrder
        {
            get
            {
                // TODO: Implement the random order training patterns enumerator.
                return trainingPatterns;
            }
        }

        /// <summary>
        /// The training set indexer.
        /// </summary>
        /// <param name="trainingPatternIndex">The training pattern index.</param>
        /// <returns>
        /// The training pattern at the given index.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Condition: <c>trainingPatternIndex</c> is out of range.
        /// </exception>
        public TrainingPattern this[ int trainingPatternIndex ]
        {
            get
            {
                return trainingPatterns[ trainingPatternIndex ];
            }
        }

        /// <summary>
        /// Gets the size of the training set (i.e. the number of training patterns in the training set).
        /// </summary>
        /// <value>
        /// The size of the training set.
        /// Note that this is always always greater then or equal to zero.
        /// </value>
        public int Size
        {
            get
            {
                return trainingPatterns.Count;
            }
        }

        /// <summary>
        /// Gets the length of the input vector.
        /// </summary>
        /// <value>
        /// The length of the input vector.
        /// Note that this is always greater than zero.
        /// </value>
        public int InputVectorLength
        {
            get
            {
                return inputVectorLength;
            }
        }

        /// <summary>
        /// Gets the length of the output vector.
        /// </summary>
        /// <value>
        /// The length of the output vector.
        /// Note that this is always greater than zero.
        /// </value>
        public int OutputVectorLength
        {
            get
            {
                return outputVectorLength;
            }
        }

        #endregion // Public instance properties
        
        #region Public instance constructors

        /// <summary>
        /// Creates a new (labeled) training set.
        /// </summary>
        /// <param name="inputVectorLength">The length of the input vector.</param>
        /// <param name="outputVectorLength">The length of the output vector.</param>
        /// <exception cref="ArgumentException">
        /// Condition 1: <c>inputVectorLength</c> is less than or equal to zero.
        /// Condition 2: <c>outputVectorLength</c> is less than or equal to zero.
        /// </exception>
        public TrainingSet( int inputVectorLength, int outputVectorLength )
        {
            // Validate the input vector length.
            Utilities.NumberPositive( inputVectorLength, "inputVectorLength" );
            this.inputVectorLength = inputVectorLength;

            // Validate the output vector length.
            Utilities.NumberPositive( outputVectorLength, "outputVectorLength" );
            this.outputVectorLength = outputVectorLength;

            trainingPatterns = new List< TrainingPattern >();        
        }

        /// <summary>
        /// Creates a new (labeled) training set from a file.
        /// </summary>
        /// <param name="trainingSetFileName">The name of the file containing the training set.</param>
        public TrainingSet(string trainingSetFileName)
        {
            TextReader textReader = new StreamReader(trainingSetFileName);
            const char separator = ' ';

            //
            // 1. Read the input vector length and the input vector length.
            //

            string line = textReader.ReadLine();
            string[] words = line.Split(separator);

            // Validate the input vector length.
            int inputVectorLength = Int32.Parse(words[0]);
            Utilities.NumberPositive(inputVectorLength, "inputVectorLength");
            this.inputVectorLength = inputVectorLength;

            // Validate the output vector length.
            int outputVectorLength = Int32.Parse(words[1]);
            Utilities.NumberPositive(outputVectorLength, "outputVectorLength");
            this.outputVectorLength = outputVectorLength;

            // Skip the blank line.
            textReader.ReadLine();

            //
            // 2. Create the training patterns.
            //

            trainingPatterns = new List<TrainingPattern>();
            while ((line = textReader.ReadLine()) != null)
            {
                words = line.Split(separator);

                // 2.1. Create the input vector.
                double[] inputVector = new double[inputVectorLength];
                for (int i = 0; i < inputVectorLength; i++)
                {
                    inputVector[i] = Double.Parse(words[i]);
                }

                // 2.2. Create the output vector length.
                double[] outputVector = new double[outputVectorLength];
                for (int i = 0; i < outputVectorLength; i++)
                {
                    outputVector[i] = Double.Parse(words[inputVectorLength + i]);
                }

                // 2.3. Add the training pattern into the training set.
                TrainingPattern trainingPattern = new TrainingPattern(inputVector, outputVector);
                Add(trainingPattern);
            }
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Determines whether the training set contains a training pattern.
        /// </summary>
        /// <param name="trainingPattern">The training sample to check for containment.</param>
        /// <returns>
        /// <c>True</c> if contains, <c>false</c> otherwise.
        /// </returns>
        public bool Contains( TrainingPattern trainingPattern )
        {
            return trainingPatterns.Contains( trainingPattern );
        }

        /// <summary>
        /// <para>
        /// Adds a (supervised) training pattern to the training set.
        /// </para>
        /// <para>
        /// Note that if the training pattern already exists in the set, it will be replaced.
        /// </para>
        /// </summary>
        /// <param name="trainingPattern">The training pattern to add.</param>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>trainingPattern</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Condition: the lengths of input vector or output vector (from the training pattern) differ from their expected lengths (from the training set).
        /// </exception>
        public void Add( TrainingPattern trainingPattern )
        {
            // Validatate the arguments.
            Utilities.ObjectNotNull( trainingPattern, "trainingPattern" );

            // Validate the input vector length.
            if (trainingPattern.InputVector.Length != inputVectorLength)
            {
                throw new ArgumentException( "The input vector must be of size " + inputVectorLength, "trainingPattern" );
            }
            
            // Validate the output vector length.
            if (trainingPattern.OutputVector.Length != outputVectorLength)
            {
                throw new ArgumentException( "The output vector must be of size " + outputVectorLength, "trainingPattern" );
            }

            // Add the training pattern to the training set.
            trainingPatterns.Add( trainingPattern );
        }

        /// <summary>
        /// Adds a given training set to the training set.
        /// </summary>
        /// <param name="trainingSet">The training set to be added.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c>trainingSet</c> is not compatible with this training set.
        /// This happens when either their input vector lengths or their output vector lengths differ.
        /// </exception>
        public void Add( TrainingSet trainingSet )
        {
            // Validate the arguments.
            Utilities.ObjectNotNull( trainingSet, "trainingSet" );

            // Validate the input vector length and the output vector length.
            if ((trainingSet.InputVectorLength != inputVectorLength) || (trainingSet.OutputVectorLength != outputVectorLength))
            {
                // TODO: Incompatible sets.
                throw new ArgumentException();
            }

            // Add all the training patterns contained within the given training set to the training set.
            foreach (TrainingPattern trainingPattern in trainingSet.TrainingPatterns)
            {
                trainingPatterns.Add( trainingPattern );
            }
        }

        /// <summary>
        /// Removes a training pattern from the training set.
        /// </summary>
        /// <param name="trainingPattern">The training sample to remove.</param>
        /// <returns>
        /// <c>True</c> if successful, <c>false</c> otherwise.
        /// </returns>
        public bool Remove( TrainingPattern trainingPattern )
        {
            return trainingPatterns.Remove( trainingPattern );
        }

        /// <summary>
        /// Removes all training patterns from the training set.
        /// </summary>
        public void Clear()
        {
            trainingPatterns.Clear();
        }

        public TrainingSet SeparateTestSet( int index, int size )
        {
            TrainingSet testSet = new TrainingSet( inputVectorLength, outputVectorLength );

            testSet.trainingPatterns.AddRange( this.trainingPatterns.GetRange( index, size ) );
            this.trainingPatterns.RemoveRange( index, size );

            return testSet;
        }

        /// <summary>
        /// Returns a string that represents the training set.
        /// </summary>
        /// 
        /// <returns>
        /// A string that represents the training set.
        /// </returns>
        public override string ToString()
        {
            StringBuilder trainingSetStringBuilder = new StringBuilder();

            //trainingSetStringBuilder.Append( base.ToString() );

            trainingSetStringBuilder.Append( "{\n" );

            int trainingPatternIndex = 0;
            foreach (TrainingPattern trainingPattern in trainingPatterns)
            {
                trainingSetStringBuilder.Append( "\t" + trainingPatternIndex++ + " : " + trainingPattern.ToString() + "\n" );
            }

            // Remove the trailing "\n" if the training set contained no training patterns.
            if (Size == 0)
            {
                trainingSetStringBuilder.Remove( trainingSetStringBuilder.Length - 1, 1 );
            }

            trainingSetStringBuilder.Append( "}" );

            return trainingSetStringBuilder.ToString();
        }

        #endregion // Public instance methods
    }
}
