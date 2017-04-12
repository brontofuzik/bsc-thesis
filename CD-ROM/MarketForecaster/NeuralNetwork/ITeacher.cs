using NeuralNetworkDotNet.Networks;

namespace NeuralNetworkDotNet.Teachers
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
        #region Methods

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// 
        /// <param name="trainingCycleCount">The number of training cycles.</param>
        void Train(int trainingCycleCount);

        #endregion // Methods
    }
}
