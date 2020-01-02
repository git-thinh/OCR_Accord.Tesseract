// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accord.Statistics.Kernels
{
    /// <summary>
    ///   Cauchy Kernel.
    /// </summary>
    /// <remarks>
    ///   The Cauchy kernel comes from the Cauchy distribution (Basak, 2008). It is a
    ///   long-tailed kernel and can be used to give long-range influence and sensitivity
    ///   over the high dimension space.
    /// </remarks>
    public class Cauchy : IKernel
    {
        private double sigma;

        /// <summary>
        ///   Constructs a new Cauchy Kernel.
        /// </summary>
        /// <param name="sigma">The value for sigma.</param>
        public Cauchy(double sigma)
        {
            this.sigma = sigma;
        }

        /// <summary>
        ///   Cauchy Kernel Function
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double norm = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                double d = x[i] - y[i];
                norm += d * d;
            }

            return (1.0 / (1.0 + norm / sigma));
        }

    }
}
