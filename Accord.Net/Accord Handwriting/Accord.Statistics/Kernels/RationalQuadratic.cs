﻿// Accord Statistics Library
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
    ///   Rational Quadratic Kernel.
    /// </summary>
    /// <remarks>
    ///   The Rational Quadratic kernel is less computationally intensive than
    ///   the Gaussian kernel and can be used as an alternative when using the
    ///   Gaussian becomes too expensive.
    /// </remarks>
    public class RationalQuadratic : IKernel
    {
        double constant;

        /// <summary>
        ///   Constructs a new Rational Quadratic Kernel.
        /// </summary>
        /// <param name="theta">The constant term theta.</param>
        public RationalQuadratic(double theta)
        {
            this.constant = theta;
        }

        /// <summary>
        ///   Rational Quadratic Kernel Function
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

            return 1.0 - (norm / (norm - constant));
        }

    }
}
