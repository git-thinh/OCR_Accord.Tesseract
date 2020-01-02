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
    ///   ANOVA Kernel.
    /// </summary>
    /// <remarks>
    ///   The ANOVA kernel is a radial basis function kernel, just as the Gaussian
    ///   and Laplacian kernels. It is said to perform well in multidimensional
    ///   regression problems (Hofmann, 2008).
    /// </remarks>
    public class ANOVA : IKernel
    {
        private int degree;
        private double sigma;

        /// <summary>
        ///   Constructs a new Multiquadric Kernel.
        /// </summary>
        /// <param name="degree">The ANOVA degree.</param>
        /// <param name="sigma">The slope sigma.</param>
        public ANOVA(int degree, double sigma)
        {
            this.sigma = sigma;
            this.degree = degree;
        }

        /// <summary>
        ///   ANOVA Kernel function.
        /// </summary>
        /// <param name="x">Vector x in input space.</param>
        /// <param name="y">Vector y in input space.</param>
        /// <returns>Dot product in feature (kernel) space.</returns>
        public double Function(double[] x, double[] y)
        {
            double a = 0.0;
            double d = 0.0;

            for (int k = 0; k < x.Length; k++)
            {
                d = x[k] - y[k];
                a += System.Math.Exp(-sigma * d * d);
            }

            return System.Math.Pow(a, degree);
        }


    }
}
