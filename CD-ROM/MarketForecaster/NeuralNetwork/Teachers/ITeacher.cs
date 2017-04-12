using NeuralNetwork.Networks;

namespace NeuralNetwork.Teachers
{
    /// <summary>
    /// <para>
    /// An interface of a teacher.
    /// </para>
    /// <para>
    /// Definition: A teacher is ...
    /// </para>
    /// </summary>
    public interface ITeacher
    {
        #region Properties

        /// <summary>
        /// Gets the name of the teacher.
        /// </summary>
        /// <value>
        /// The name of the teacher.
        /// </value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the training set.
        /// </summary>
        /// <value>
        /// The training set.
        /// </value>
        TrainingSet TrainingSet
        {
            get;
        }

        /// <summary>
        /// Gets the validation set.
        /// </summary>
        /// <value>
        /// The validation set.
        /// </value>
        TrainingSet ValidationSet
        {
            get;
        }

        /// <summary>
        /// Gets the test set.
        /// </summary>
        /// <value>
        /// The test set.
        /// </value>
        TrainingSet TestSet
        {
            get;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        TrainingLog Train( INetwork network, int maxIterationCount, double maxTolerableNetworkError );

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        TrainingLog Train( INetwork network, int maxIterationCount );

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        TrainingLog Train( INetwork network, double maxTolerableNetworkError );

        #endregion // Methods
    }
}
