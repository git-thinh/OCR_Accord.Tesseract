// Accord Math Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Math
{
    /// <summary>
    ///   Static class Distance. Defines a set of extension methods defining distance measures.
    /// </summary>
    public static class Distance
    {
        /// <summary>
        ///   Gets the Square Mahalanobis distance between two points.
        /// </summary>
        /// <param name="x">A point in space.</param>
        /// <param name="y">A point in space.</param>
        /// <param name="precision">
        ///   The inverse of the covariance matrix of the distribution for the two points x and y.
        /// </param>
        /// <returns>The Square Mahalanobis distance between x and y.</returns>
        public static double SquareMahalanobis(this double[] x, double[] y, double[,] precision)
        {
            double[] d = new double[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                d[i] = x[i] - y[i];
            }

            return d.Multiply(precision.Multiply(d));
        }

        /// <summary>
        ///   Gets the Mahalanobis distance between two points.
        /// </summary>
        /// <param name="x">A point in space.</param>
        /// <param name="y">A point in space.</param>
        /// <param name="precision">
        ///   The inverse of the covariance matrix of the distribution for the two points x and y.
        /// </param>
        /// <returns>The Mahalanobis distance between x and y.</returns>
        public static double Mahalanobis(this double[] x, double[] y, double[,] precision)
        {
            return System.Math.Sqrt(SquareMahalanobis(x, y, precision));
        }

        /// <summary>
        ///   Gets the Manhattan distance between two points.
        /// </summary>
        /// <param name="x">A point in space.</param>
        /// <param name="y">A point in space.</param>
        /// <returns>The manhattan distance between x and y.</returns>
        public static double Manhattan(this double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += System.Math.Abs(x[i] - y[i]);
            }
            return sum;
        }

        /// <summary>
        ///   Gets the Square Euclidean distance between two points.
        /// </summary>
        /// <param name="x">A point in space.</param>
        /// <param name="y">A point in space.</param>
        /// <returns>The Square Euclidean distance between x and y.</returns>
        public static double SquareEuclidean(this double[] x, double[] y)
        {
            double d = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                double u = x[i] - y[i];
                d += u * u;
            }

            return d;
        }

        /// <summary>
        ///   Gets the Euclidean distance between two points.
        /// </summary>
        /// <param name="x">A point in space.</param>
        /// <param name="y">A point in space.</param>
        /// <returns>The Euclidean distance between x and y.</returns>
        public static double Euclidean(this double[] x, double[] y)
        {
            return System.Math.Sqrt(SquareEuclidean(x, y));
        }

        /// <summary>
        ///   Gets the Modulo-m distance between two integers a and b.
        /// </summary>
        public static int Modular(int a, int b, int modulo)
        {
            return System.Math.Min(Tools.Mod(a - b, modulo), Tools.Mod(b - a, modulo));
        }
    }
}
