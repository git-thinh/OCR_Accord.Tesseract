// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

using System;

using Accord.Math;
using System.Collections.Generic;


namespace Accord.Statistics
{

    public enum Estimation
    {
        Sample,
        Population,
    }

    /// <summary>
    ///     Set of statistics functions
    /// </summary>
    /// 
    /// <remarks>
    ///     This class represents collection of functions used
    ///     in statistics. Every Matrix function assumes data is organized
    ///     in a table-like model, where Columns represents variables and
    ///     Rows represents a observation of each variable.
    /// </remarks>
    /// 
    public static class Tools
    {

        #region Arrays

        /// <summary>Computes the Mean of the given values.</summary>
        /// <param name="vector">A double array containing the vector members.</param>
        /// <returns>The mean of the given data.</returns>
        public static double Mean(this double[] values)
        {
            double sum = 0.0;
            double n = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum / n;
        }

        /// <summary>Computes the Standard Deviation of the given values.</summary>
        /// <param name="vector">A double array containing the vector members.</param>
        /// <returns>The standard deviation of the given data.</returns>
        public static double StandardDeviation(this double[] values)
        {
            return StandardDeviation(values, Mean(values));
        }

        /// <summary>Computes the Standard Deviation of the given values.</summary>
        /// <param name="vector">A double array containing the vector members.</param>
        /// <param name="mean">The mean of the vector, if already known.</param>
        /// <returns>The standard deviation of the given data.</returns>
        public static double StandardDeviation(this double[] values, double mean)
        {
            return System.Math.Sqrt(Variance(values, mean));
        }

        /// <summary>
        ///   Computes the Standard Error for a sample size, which estimates the
        ///   standard deviation of the sample mean based on the population mean.
        /// </summary>
        /// <param name="samples">The sample size.</param>
        /// <param name="standardDeviation">The sample standard deviation.</param>
        /// <returns>The standard error for the sample.</returns>
        public static double StandardError(int samples, double standardDeviation)
        {
            return standardDeviation / System.Math.Sqrt(samples);
        }

        /// <summary>
        ///   Computes the Standard Error for a sample size, which estimates the
        ///   standard deviation of the sample mean based on the population mean.
        /// </summary>
        /// <param name="vector">A double array containing the samples.</param>
        /// <returns>The standard error for the sample.</returns>
        public static double StandardError(double[] values)
        {
            return StandardError(values.Length, StandardDeviation(values));
        }

        /// <summary>Computes the Median of the given values.</summary>
        /// <param name="vector">A double array containing the vector members.</param>
        /// <returns>The median of the given data.</returns>
        public static double Median(double[] values)
        {
            return Median(values, false);
        }

        /// <summary>Computes the Median of the given values.</summary>
        /// <param name="values">An integer array containing the vector members.</param>
        /// <param name="alreadySorted">A boolean parameter informing if the given values have already been sorted.</param>
        /// <returns>The median of the given data.</returns>
        public static double Median(double[] values, bool alreadySorted)
        {
            double[] data = new double[values.Length];
            values.CopyTo(data, 0); // Creates a copy of the given values,

            if (!alreadySorted) // So we can sort it without modifying the original array.
                Array.Sort(data);

            int N = data.Length;

            if ((N % 2) == 0)
                return (data[N / 2] + data[(N / 2) + 1]) * 0.5; // N is even 
            else return data[(N + 1) / 2];                      // N is odd
        }


        /// <summary>Computes the Variance of the given values.</summary>
        /// <param name="vector">A double precision number array containing the vector members.</param>
        /// <returns>The variance of the given data.</returns>
        public static double Variance(double[] values)
        {
            return Variance(values, Mean(values));
        }

        /// <summary>Computes the Variance of the given values.</summary>
        /// <param name="vector">A number array containing the vector members.</param>
        /// <param name="mean">The mean of the array, if already known.</param>
        /// <returns>The variance of the given data.</returns>
        public static double Variance(double[] values, double mean)
        {
            double sum1 = 0.0;
            double sum2 = 0.0;
            double N = values.Length;
            double x = 0.0;

            for (int i = 0; i < values.Length; i++)
            {
                x = values[i] - mean;
                sum1 += x;
                sum2 += x * x;
            }

            // Sample variance
            return (sum2 - ((sum1 * sum1) / N)) / (N - 1);
        }


        /// <summary>Computes the Mode of the given values.</summary>
        /// <param name="values">A number array containing the vector values.</param>
        /// <returns>The variance of the given data.</returns>
        public static double Mode(double[] values)
        {
            int[] itemCount = new int[values.Length];
            double[] itemArray = new double[values.Length];
            int count = 0;

            for (int i = 0; i < values.Length; i++)
            {
                int index = Array.IndexOf<double>(itemArray, values[i], 0, count);

                if (index >= 0)
                {
                    itemCount[index]++;
                }
                else
                {
                    itemArray[count] = values[i];
                    itemCount[count] = 1;
                    count++;
                }
            }

            int maxValue = 0;
            int maxIndex = 0;

            for (int i = 0; i < count; i++)
            {
                if (itemCount[i] > maxValue)
                {
                    maxValue = itemCount[i];
                    maxIndex = i;
                }
            }

            return itemArray[maxIndex];
        }

        /// <summary>Computes the Covariance between two values arrays.</summary>
        /// <param name="u">A number array containing the first vector members.</param>
        /// <param name="v">A number array containing the second vector members.</param>
        /// <returns>The variance of the given data.</returns>
        public static double Covariance(double[] u, double[] v)
        {
            if (u.Length != v.Length)
            {
                throw new ArgumentException("Vector sizes must be equal.", "u");
            }

            double uSum = 0.0;
            double vSum = 0.0;
            double N = u.Length;

            // Calculate Sums for each vector
            for (int i = 0; i < u.Length; i++)
            {
                uSum += u[i];
                vSum += v[i];
            }

            double uMean = uSum / N;
            double vMean = vSum / N;

            double covariance = 0.0;
            for (int i = 0; i < u.Length; i++)
            {
                covariance += (u[i] - uMean) * (v[i] - vMean);
            }

            return covariance / (N - 1); // sample variance
        }

        /// <summary>
        ///   Computes the Skewness for the given values.
        /// </summary>
        /// <remarks>
        ///   Skewness characterizes the degree of asymmetry of a distribution
        ///   around its mean. Positive skewness indicates a distribution with
        ///   an asymmetric tail extending towards more positive values. Negative
        ///   skewness indicates a distribution with an asymmetric tail extending
        ///   towards more negative values.
        /// </remarks>
        /// <param name="values">A number array containing the vector values.</param>
        /// <returns>The skewness of the given data.</returns>
        public static double Skewness(double[] values)
        {
            double mean = Mean(values);
            return Skewness(values, mean, StandardDeviation(values, mean));
        }

        /// <summary>
        ///   Computes the Skewness for the given values.
        /// </summary>
        /// <remarks>
        ///   Skewness characterizes the degree of asymmetry of a distribution
        ///   around its mean. Positive skewness indicates a distribution with
        ///   an asymmetric tail extending towards more positive values. Negative
        ///   skewness indicates a distribution with an asymmetric tail extending
        ///   towards more negative values.
        /// </remarks>
        /// <param name="values">A number array containing the vector values.</param>
        /// <param name="mean">The values' mean, if already known.</param>
        /// <param name="standardDeviation">The values' standard deviations, if already known.</param>
        /// <returns>The skewness of the given data.</returns>
        public static double Skewness(double[] values, double mean, double standardDeviation)
        {
            int n = values.Length;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                // Sum of third moment deviations
                sum += System.Math.Pow(values[i] - mean, 3);
            }

            return sum / ((n - 1) * System.Math.Pow(standardDeviation, 3));
        }

        public static double Kurtosis(double[] values)
        {
            double mean = Mean(values);
            return Kurtosis(values, mean, StandardDeviation(values, mean));
        }

        public static double Kurtosis(double[] values, double mean, double standardDeviation)
        {
            int n = values.Length;
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                // Sum of fourth moment deviations
                sum += System.Math.Pow(values[i] - mean, 4);
            }

            return sum / (n * System.Math.Pow(standardDeviation, 4)) - 3.0;
        }
        #endregion


        // ------------------------------------------------------------


        #region Matrix


        /// <summary>Calculates the matrix Mean vector.</summary>
        /// <param name="m">A matrix whose means will be calculated.</param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
        public static double[] Mean(double[,] value)
        {
            return Mean(value, 1);
        }

        /// <summary>Calculates the matrix Mean vector.</summary>
        /// <param name="m">A matrix whose means will be calculated.</param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
        public static double[] Mean(double[,] value, int dimension)
        {
            if (dimension == 1)
            {
                double[] mean = new double[value.GetLength(1)];
                double rows = value.GetLength(0);

                // for each column
                for (int j = 0; j < value.GetLength(1); j++)
                {
                    // for each row
                    for (int i = 0; i < value.GetLength(0); i++)
                        mean[j] += value[i, j];

                    mean[j] = mean[j] / rows;
                }

                return mean;
            }
            else
            {
                double[] mean = new double[value.GetLength(0)];
                double cols = value.GetLength(1);

                // for each row
                for (int j = 0; j < value.GetLength(0); j++)
                {
                    // for each column
                    for (int i = 0; i < value.GetLength(1); i++)
                        mean[j] += value[j, i];

                    mean[j] = mean[j] / cols;
                }

                return mean;
            }
        }

        /// <summary>Calculates the matrix Mean vector.</summary>
        /// <param name="m">A matrix whose means will be calculated.</param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
        public static double[] Mean(double[][] value)
        {
            double[] mean = new double[value.GetLength(1)];
            double rows = value.GetLength(0);

            // for each column
            for (int j = 0; j < value.GetLength(1); j++)
            {
                // for each row
                for (int i = 0; i < value.GetLength(0); i++)
                {
                    mean[j] += value[i][j];
                }

                mean[j] = mean[j] / rows;
            }

            return mean;
        }

        /// <summary>Calculates the matrix Mean vector.</summary>
        /// <param name="m">A matrix whose means will be calculated.</param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
        public static double[] Mean(double[,] value, double[] sumVector)
        {
            double[] mean = new double[value.GetLength(1)];
            double rows = value.GetLength(0);

            // for each column
            for (int j = 0; j < value.GetLength(1); j++)
            {
                mean[j] = sumVector[j] / rows;
            }

            return mean;
        }

        /// <summary>Calculates the matrix Standard Deviations vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// <returns>Returns a vector containing the standard deviations of the given matrix.</returns>
        public static double[] StandardDeviation(double[,] value)
        {
            return StandardDeviation(value, Mean(value));
        }

        /// <summary>Calculates the matrix Standard Deviations vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// <param name="meanVector">The mean vector containing already calculated means for each column of the matix.</param>
        /// <returns>Returns a vector containing the standard deviations of the given matrix.</returns>
        public static double[] StandardDeviation(this double[,] value, double[] meanVector)
        {
            return Matrix.Sqrt(Variance(value, meanVector));
        }

        /// <summary>Calculates the matrix Standard Deviations vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// <param name="meanVector">The mean vector containing already calculated means for each column of the matix.</param>
        /// <returns>Returns a vector containing the standard deviations of the given matrix.</returns>
        public static double[] StandardDeviation(this double[][] value, double[] meanVector)
        {
            return Matrix.Sqrt(Variance(value, meanVector));
        }

        /// <summary>Calculates the matrix Medians vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
        public static double[] Variance(this double[,] value)
        {
            return Variance(value, Mean(value));
        }

        /// <summary>Calculates the matrix Medians vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// /// <param name="meanVector">The mean vector containing already calculated means for each column of the matix.</param>
        /// <returns>Returns a vector containing the mean of the given matrix.</returns>
        public static double[] Variance(this double[,] value, double[] means)
        {
            double[] variance = new double[value.GetLength(1)];

            // for each column (for each variable)
            for (int j = 0; j < value.GetLength(1); j++)
            {
                double sum1 = 0.0;
                double sum2 = 0.0;
                double x = 0.0;
                double N = value.GetLength(0);

                // for each row (observation of the variable)
                for (int i = 0; i < value.GetLength(0); i++)
                {
                    x = value[i, j] - means[j];
                    sum1 += x;
                    sum2 += x * x;
                }

                // calculate the variance
                variance[j] = (sum2 - ((sum1 * sum1) / N)) / (N - 1);
            }

            return variance;
        }

        /// <summary>Calculates the matrix Medians vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// /// <param name="meanVector">The mean vector containing already calculated means for each column of the matix.</param>
        /// <returns>Returns a vector containing the mean of the given matrix.</returns>
        public static double[] Variance(this double[][] value, double[] means)
        {
            double[] variance = new double[value[0].Length];

            // for each column (for each variable)
            for (int j = 0; j < value.GetLength(1); j++)
            {
                double sum1 = 0.0;
                double sum2 = 0.0;
                double x = 0.0;
                double N = value.GetLength(0);

                // for each row (observation of the variable)
                for (int i = 0; i < value.GetLength(0); i++)
                {
                    x = value[i][j] - means[j];
                    sum1 += x;
                    sum2 += x * x;
                }

                // calculate the variance
                variance[j] = (sum2 - ((sum1 * sum1) / N)) / (N - 1);
            }

            return variance;
        }

        /// <summary>Calculates the matrix Medians vector.</summary>
        /// <param name="m">A matrix whose deviations will be calculated.</param>
        /// <returns>Returns a vector containing the medians of the given matrix.</returns>
        public static double[] Median(double[,] value)
        {
            int rows = value.GetLength(0);
            int cols = value.GetLength(1);
            double[] medians = new double[cols];

            for (int i = 0; i < cols; i++)
            {
                double[] data = new double[rows];

                // Creates a copy of the given values
                for (int j = 0; j < rows; j++)
                    data[j] = value[j, i];

                Array.Sort(data); // Sort it

                int N = data.Length;

                if ((N % 2) == 0)
                    medians[i] = (data[N / 2] + data[(N / 2) + 1]) * 0.5; // N is even 
                else medians[i] = data[(N + 1) / 2];                      // N is odd
            }

            return medians;
        }

        /// <summary>Calculates the matrix Modes vector.</summary>
        /// <param name="m">A matrix whose modes will be calculated.</param>
        /// <returns>Returns a vector containing the modes of the given matrix.</returns>
        public static double[] Mode(this double[,] matrix)
        {
            double[] mode = new double[matrix.GetLength(1)];

            for (int i = 0; i < mode.Length; i++)
            {
                int[] itemCount = new int[matrix.GetLength(0)];
                double[] itemArray = new double[matrix.GetLength(0)];
                int count = 0;

                // for each row
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    int index = Array.IndexOf<double>(itemArray, matrix[j, i], 0, count);

                    if (index >= 0)
                    {
                        itemCount[index]++;
                    }
                    else
                    {
                        itemArray[count] = matrix[j, i];
                        itemCount[count] = 1;
                        count++;
                    }
                }

                int maxValue = 0;
                int maxIndex = 0;

                for (int j = 0; j < count; j++)
                {
                    if (itemCount[j] > maxValue)
                    {
                        maxValue = itemCount[j];
                        maxIndex = j;
                    }
                }

                mode[i] = itemArray[maxIndex];
            }

            return mode;
        }

        /// <summary>
        ///   Computes the Skewness for the given values.
        /// </summary>
        /// <remarks>
        ///   Skewness characterizes the degree of asymmetry of a distribution
        ///   around its mean. Positive skewness indicates a distribution with
        ///   an asymmetric tail extending towards more positive values. Negative
        ///   skewness indicates a distribution with an asymmetric tail extending
        ///   towards more negative values.
        /// </remarks>
        /// <param name="values">A number array containing the vector values.</param>
        /// <returns>The skewness of the given data.</returns>
        public static double[] Skewness(double[,] matrix)
        {
            double[] means = Mean(matrix);
            return Skewness(matrix, means, StandardDeviation(matrix, means));
        }

        /// <summary>
        ///   Computes the Skewness for the given values.
        /// </summary>
        /// <remarks>
        ///   Skewness characterizes the degree of asymmetry of a distribution
        ///   around its mean. Positive skewness indicates a distribution with
        ///   an asymmetric tail extending towards more positive values. Negative
        ///   skewness indicates a distribution with an asymmetric tail extending
        ///   towards more negative values.
        /// </remarks>
        /// <param name="values">A number array containing the vector values.</param>
        /// <param name="mean">The values' mean, if already known.</param>
        /// <param name="standardDeviation">The values' standard deviations, if already known.</param>
        /// <returns>The skewness of the given data.</returns>
        public static double[] Skewness(double[,] matrix, double[] means, double[] standardDeviations)
        {
            int n = matrix.GetLength(0);
            double[] skewness = new double[matrix.GetLength(1)];
            for (int j = 0; j < skewness.Length; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    // Sum of third moment deviations
                    sum += System.Math.Pow(matrix[i, j] - means[j], 3);
                }

                skewness[j] = sum / ((n - 1) * System.Math.Pow(standardDeviations[j], 3));
            }

            return skewness;
        }

        public static double[] Kurtosis(double[,] matrix)
        {
            double[] means = Mean(matrix);
            return Kurtosis(matrix, means, StandardDeviation(matrix, means));
        }

        public static double[] Kurtosis(double[,] matrix, double[] means, double[] standardDeviations)
        {
            int n = matrix.GetLength(0);
            double[] kurtosis = new double[matrix.GetLength(1)];
            for (int j = 0; j < kurtosis.Length; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    // Sum of fourth moment deviations
                    sum += System.Math.Pow(matrix[i, j] - means[j], 4);
                }

                kurtosis[j] = sum / (n * System.Math.Pow(standardDeviations[j], 4)) - 3.0;
            }

            return kurtosis;
        }

        public static double[] StandardError(double[,] matrix)
        {
            return StandardError(matrix.GetLength(0), StandardDeviation(matrix));
        }

        public static double[] StandardError(int samples, double[] standardDeviations)
        {
            double[] standardErrors = new double[standardDeviations.Length];
            double sqrt = System.Math.Sqrt(samples);
            for (int i = 0; i < standardDeviations.Length; i++)
            {
                standardErrors[i] = standardDeviations[i] / sqrt;
            }
            return standardErrors;
        }

        /// <summary>Calculates the covariance matrix of a sample matrix, returning a new matrix object</summary>
        /// <remarks>
        ///   In statistics and probability theory, the covariance matrix is a matrix of
        ///   covariances between elements of a vector. It is the natural generalization
        ///   to higher dimensions of the concept of the variance of a scalar-valued
        ///   random variable.
        /// </remarks>
        /// <returns>The covariance matrix.</returns>
        public static double[,] Covariance(this double[,] matrix)
        {
            return Covariance(matrix, Mean(matrix));
        }

        public static double[,] Covariance(this double[,] matrix, double[] mean)
        {
            return Scatter(matrix, mean, matrix.GetLength(0) - 1, 1);
        }

        public static double[,] Scatter(double[,] matrix, double[] mean)
        {
            return Scatter(matrix, mean, 1.0, 1);
        }

        public static double[,] Scatter(double[,] matrix, double[] mean, double divide)
        {
            return Scatter(matrix, mean, divide, 1);
        }

        public static double[,] Scatter(double[,] matrix, double[] mean, int dimension)
        {
            return Scatter(matrix, mean, 1.0, dimension);
        }

        public static double[,] Scatter(double[,] matrix, double[] mean, double divide, int dimension)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] cov;

            if (dimension == 1)
            {
                cov = new double[cols, cols];
                for (int i = 0; i < cols; i++)
                {
                    for (int j = i; j < cols; j++)
                    {
                        double s = 0.0;
                        for (int k = 0; k < rows; k++)
                            s += (matrix[k, j] - mean[j]) * (matrix[k, i] - mean[i]);
                        s /= divide;
                        cov[i, j] = s;
                        cov[j, i] = s;
                    }
                }
            }
            else
            {
                cov = new double[rows, rows];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = i; j < rows; j++)
                    {
                        double s = 0.0;
                        for (int k = 0; k < cols; k++)
                            s += (matrix[j, k] - mean[j]) * (matrix[i, k] - mean[i]);
                        s /= divide;
                        cov[i, j] = s;
                        cov[j, i] = s;
                    }
                }
            }

            return cov;
        }

        /// <summary>Calculates the correlation matrix of this samples, returning a new matrix object</summary>
        /// <remarks>
        /// In statistics and probability theory, the correlation matrix is the same
        /// as the covariance matrix of the standardized random variables.
        /// </remarks>
        /// <returns>The correlation matrix</returns>
        public static double[,] Correlation(double[,] matrix)
        {
            double[] means = Mean(matrix);
            return Correlation(matrix, means, StandardDeviation(matrix, means));
        }

        /// <summary>
        ///   Calculates the correlation matrix of this samples, returning a new matrix object
        /// </summary>
        /// <remarks>
        ///   In statistics and probability theory, the correlation matrix is the same
        ///   as the covariance matrix of the standardized random variables.
        /// </remarks>
        /// <returns>The correlation matrix</returns>
        public static double[,] Correlation(double[,] matrix, double[] mean, double[] stdDev)
        {
            double[,] scores = ZScores(matrix, mean, stdDev);

            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double N = rows;
            double[,] cor = new double[cols, cols];
            for (int i = 0; i < cols; i++)
            {
                for (int j = i; j < cols; j++)
                {
                    double c = 0.0;
                    for (int k = 0; k < rows; k++)
                    {
                        c += scores[k, j] * scores[k, i];
                    }
                    c /= N - 1.0;
                    cor[i, j] = c;
                    cor[j, i] = c;
                }
            }

            return cor;
        }


        /// <summary>Generates the Standard Scores, also known as Z-Scores, the core from the given data.</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double[,] ZScores(double[,] value)
        {
            double[] mean = Mean(value);
            return ZScores(value, mean, StandardDeviation(value, mean));
        }


        public static double[,] ZScores(double[,] value, double[] means, double[] deviations)
        {
            double[,] m = (double[,])value.Clone();

            Center(m, means);
            Standardize(m, deviations);

            return m;
        }



        /// <summary>Centers column data, subtracting the empirical mean from each variable.</summary>
        /// <param name="m">A matrix where each column represent a variable and each row represent a observation.</param>
        public static void Center(double[,] value)
        {
            Center(value, Mean(value));
        }

        /// <summary>Centers column data, subtracting the empirical mean from each variable.</summary>
        /// <param name="m">A matrix where each column represent a variable and each row represent a observation.</param>
        public static void Center(double[,] value, double[] means)
        {
            for (int i = 0; i < value.GetLength(0); i++)
            {
                for (int j = 0; j < value.GetLength(1); j++)
                {
                    value[i, j] -= means[j];
                }
            }
        }


        /// <summary>Standardizes column data, removing the empirical standard deviation from each variable.</summary>
        /// <param name="m">A matrix where each column represent a variable and each row represent a observation.</param>
        /// <remarks>This method does not remove the empirical mean prior to execution.</remarks>
        public static void Standardize(double[,] value)
        {
            Standardize(value, StandardDeviation(value));
        }

        public static void Standardize(this double[,] value, double[] deviations)
        {
            for (int i = 0; i < value.GetLength(0); i++)
            {
                for (int j = 0; j < value.GetLength(1); j++)
                {
                    value[i, j] /= deviations[j];
                }
            }
        }
        #endregion


        // ------------------------------------------------------------


        #region Summarizing, grouping and extending operations
        /// <summary>
        ///   Calculate the prevalence of a class.
        /// </summary>
        /// <param name="positives">An array of counts detailing the occurence of the first class.</param>
        /// <param name="negatives">An array of counts detailing the occurence of the second class.</param>
        /// <returns>An array containing the proportion of the first class over the total of occurances.</returns>
        public static double[] Proportions(int[] positives, int[] negatives)
        {
            double[] r = new double[positives.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = (double)positives[i] / (positives[i] + negatives[i]);
            return r;
        }

        /// <summary>
        ///   Calculate the prevalence of a class.
        /// </summary>
        /// <param name="data">A matrix containing counted, grouped data.</param>
        /// <param name="positiveColumn">The index for the column which contains counts for occurence of the first class.</param>
        /// <param name="negativeColumn">The index for the column which contains counts for occurence of the second class.</param>
        /// <returns>An array containing the proportion of the first class over the total of occurances.</returns>
        public static double[] Proportions(int[][] data, int positiveColumn, int negativeColumn)
        {
            double[] r = new double[data.Length];
            for (int i = 0; i < r.Length; i++)
                r[i] = (double)data[i][positiveColumn] / (data[i][positiveColumn] + data[i][negativeColumn]);
            return r;
        }

        /// <summary>
        ///   Groups the occurances contained in data matrix of binary (dichotomous) data.
        /// </summary>
        /// <param name="data">A data matrix containing at least a column of binary data.</param>
        /// <param name="labelColumn">Index of the column which contains the group label name.</param>
        /// <param name="dataColumn">Index of the column which contains the binary [0,1] data.</param>
        /// <returns>
        ///    A matrix containing the group label in the first column, the number of occurances of the first class
        ///    in the second column and the number of occurances of the second class in the third column.
        /// </returns>
        public static int[][] Group(int[][] data, int labelColumn, int dataColumn)
        {
            var groups = new List<int>();
            var groupings = new List<int[]>();

            for (int i = 0; i < data.Length; i++)
            {
                int group = data[i][labelColumn];
                if (!groups.Contains(group))
                {
                    groups.Add(group);

                    int positives = 0, negatives = 0;
                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j][labelColumn] == group)
                        {
                            if (data[j][dataColumn] == 0)
                                negatives++;
                            else positives++;
                        }
                    }

                    groupings.Add(new int[] { group, positives, negatives });
                }
            }

            return groupings.ToArray();
        }

        /// <summary>
        ///   Extendes a grouped data into a full observation matrix.
        /// </summary>
        /// <param name="group">The group labels.</param>
        /// <param name="positives">
        ///   An array containing he occurence of the positive class
        ///   for each of the groups.</param>
        /// <param name="negatives">
        ///   An array containing he occurence of the negative class
        ///   for each of the groups.</param>
        /// <returns>A full sized observation matrix.</returns>
        public static int[][] Extend(int[] group, int[] positives, int[] negatives)
        {
            List<int[]> rows = new List<int[]>();

            for (int i = 0; i < group.Length; i++)
            {
                for (int j = 0; j < positives[i]; j++)
                    rows.Add(new int[] { group[i], 1 });

                for (int j = 0; j < negatives[i]; j++)
                    rows.Add(new int[] { group[i], 0 });
            }

            return rows.ToArray();
        }

        /// <summary>
        ///   Extendes a grouped data into a full observation matrix.
        /// </summary>
        /// <param name="data">The grouped data matrix.</param>
        /// <param name="labelColumn">Index of the column which contains the labels
        /// in the grouped data matrix. </param>
        /// <param name="positiveColumn">Index of the column which contains
        ///   the occurances for the first class.</param>
        /// <param name="positiveColumn">Index of the column which contains
        ///   the occurances for the second class.</param>
        /// <returns>A full sized observation matrix.</returns>
        public static int[][] Extend(int[][] data, int labelColumn, int positiveColumn, int negativeColumn)
        {
            List<int[]> rows = new List<int[]>();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i][positiveColumn]; j++)
                    rows.Add(new int[] { data[i][labelColumn], 1 });

                for (int j = 0; j < data[i][negativeColumn]; j++)
                    rows.Add(new int[] { data[i][labelColumn], 0 });
            }

            return rows.ToArray();
        }

        #endregion


        #region Performance measures
        /// <summary>
        ///   Gets the coefficient of determination, as known as the R-Squared (R²)
        /// </summary>
        /// <remarks>
        ///    The coefficient of determination is used in the context of statistical models
        ///    whose main purpose is the prediction of future outcomes on the basis of other
        ///    related information. It is the proportion of variability in a data set that
        ///    is accounted for by the statistical model. It provides a measure of how well
        ///    future outcomes are likely to be predicted by the model.
        ///    
        ///    The R^2 coefficient of determination is a statistical measure of how well the
        ///    regression approximates the real data points. An R^2 of 1.0 indicates that the
        ///    regression perfectly fits the data.
        /// </remarks>
        public static double Determination(double[] actual, double[] expected)
        {
            // R-squared = 100 * SS(regression) / SS(total)

            int N = actual.Length;
            double SSe = 0.0;
            double SSt = 0.0;
            double avg = 0.0;
            double d;

            // Calculate expected output mean
            for (int i = 0; i < N; i++)
                avg += expected[i];
            avg /= N;

            // Calculate SSe and SSt
            for (int i = 0; i < N; i++)
            {
                d = expected[i] - actual[i];
                SSe += d * d;

                d = expected[i] - avg;
                SSt += d * d;
            }

            // Calculate R-Squared
            return 1.0 - (SSe / SSt);
        }
        #endregion

        /// <summary>
        ///   Returns a random sample of size k from a population of size n.
        /// </summary>
        public static int[] Random(int n, int k)
        {
            var r = new Random();

            // If the sample is a sizeable fraction of the population, just
            // randomize the whole population (which involves a full sort
            // of n random values), and take the first k.
            if (4 * k > n)
            {
                int[] idx = Tools.Random(n);
                return idx.Submatrix(k);
            }
            else
            {
                int[] x = new int[n];

                int sumx = 0;
                while (sumx < k)
                {
                    x[r.Next(0, n)] = 1;
                    sumx = Accord.Math.Matrix.Sum(x);
                }

                int[] y = x.Find(z => z > 0);
                return y.Submatrix(Random(k));
            }
        }

        /// <summary>
        ///   Returns a random permutation of size n.
        /// </summary>
        public static int[] Random(int n)
        {
            var r = new Random();

            double[] x = new double[n];
            int[] idx = Matrix.Indices(0, n);

            for (int i = 0; i < n; i++)
                x[i] = r.NextDouble();

            Array.Sort(x, idx);

            return idx;
        }
    }
}

