// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

using System;

namespace Accord.Statistics.Kernels
{
    /// <summary>
    ///   Tensor Product Combination of Kernels
    /// </summary>
    public class Tensor : IKernel
    {
        private IKernel[] kernels;

        /// <summary>
        ///   Constructs a new additive kernel.
        /// </summary>
        /// <param name="kernels">Kernels to combine.</param>
        public Tensor(params IKernel[] kernels)
        {
            this.kernels = kernels;
        }

        /// <summary>
        ///   Tensor Product Kernel Combination function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double product = 1.0;

            for (int i = 0; i < kernels.Length; i++)
            {
                product *= kernels[i].Function(x, y);
            }

            return product;
        }

        /// <summary>
        ///   Computes the distance in input space
        ///   between two points given in feature space.
        /// </summary>
        /// <param name="x">Vector x in feature (kernel) space.</param>
        /// <param name="y">Vector y in feature (kernel) space.</param>
        /// <returns>Distance between x and y in input space.</returns>
        public double Distance(double[] x, double[] y)
        {
            throw new NotImplementedException();
        }

    }
}
