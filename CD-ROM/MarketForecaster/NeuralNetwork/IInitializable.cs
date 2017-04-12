// NeuralNetworkDotNet
// Copyright 2009 by Lukas Kudela
//
// This class is released under the:
// GNU Lesser General Public License (LGPL)
// http://www.gnu.org/copyleft/lesser.html

using NeuralNetworkDotNet.Utilities;

namespace NeuralNetworkDotNet
{
    /// <summary>
    /// An interface (mixin) of an initializable object.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// 
        /// <param name="range">The range of randomness.</param>
        void Initialize( Range range );
    }
}
