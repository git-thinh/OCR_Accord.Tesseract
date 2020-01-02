﻿// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Kernels;
using System.Collections.Generic;

namespace Accord.Statistics.Analysis
{
    /// <summary>
    ///   Kernel (Fisher) Discriminant Analysis
    /// </summary>
    /// <remarks>
    ///   Kernel (Fisher) discriminant analysis (kernel FDA) is a non-linear generalization
    ///   of linear discriminant analysis (LDA) using techniques of kernel methods. Using a
    ///   kernel, the originally linear operations of LDA are done in a reproducing kernel
    ///   Hilbert space with a non-linear mapping.
    ///   
    ///   The algorithm used is a multi-class generalization of the original algorithm by
    ///   Mika et al. in Fisher discriminant analysis with kernels (1999).
    ///   
    ///   References:
    ///    - Mika et al, Fisher discriminant analysis with kernels (1999)
    ///      http://citeseerx.ist.psu.edu/viewdoc/summary?doi=10.1.1.35.9904
    /// </remarks>
    public class KernelDiscriminantAnalysis : LinearDiscriminantAnalysis
    {
        private IKernel kernel;
        private double regularization = 0.0001;
        private double threshold = 0.001;

        private double[][] kernelClassMeans;


        //---------------------------------------------


        #region Constructor
        /// <summary>
        ///   Constructs a new Kernel Discriminant Analysis object.
        /// </summary>
        /// <param name="inputs">The source data to perform analysis. The matrix should contain
        /// variables as columns and observations of each variable as rows.</param>
        /// <param name="output">The labels for each observation row in the input matrix.</param>
        /// <param name="kernel">The kernel to be used in the analysis.</param>
        public KernelDiscriminantAnalysis(double[,] inputs, int[] output, IKernel kernel)
            : base(inputs, output)
        {
            this.kernel = kernel;
            this.kernelClassMeans = new double[Classes.Count][];
        }
        #endregion


        //---------------------------------------------


        #region Public Properties
        /// <summary>
        ///   Gets the Kernel used in the analysis.
        /// </summary>
        public IKernel Kernel
        {
            get { return kernel; }
        }

        /// <summary>
        ///   Gets or sets the regularization parameter to
        ///   avoid non-singularities at the solution.
        /// </summary>
        public double Regularization
        {
            get { return regularization; }
            set { regularization = value; }
        }

        /// <summary>
        ///   Gets or sets whether unused discriminants should
        ///   be automatically discarded after the analysis if
        ///   their proportion are below a given threshold.
        /// </summary>
        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }
        #endregion


        //---------------------------------------------


        #region Public Methods
        /// <summary>
        ///   Computes the Multi-Class Kernel Discriminant Analysis algorithm.
        /// </summary>
        public override void Compute()
        {
            // Get some initial information
            int dimension = Source.GetLength(0);
            double[,] source = Source;
            double total = dimension;


            // Create the Gram (Kernel) Matrix
            double[,] K = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = i; j < dimension; j++)
                {
                    double s = kernel.Function(source.GetRow(i), source.GetRow(j));
                    K[i, j] = s;
                    K[j, i] = s;
                }
            }


            // Compute entire data set measures
            base.Means = Tools.Mean(K);
            base.StandardDeviations = Tools.StandardDeviation(K, Means);


            // Initialize the kernel analogue scatter matrices
            double[,] Sb = new double[dimension, dimension];
            double[,] Sw = new double[dimension, dimension];


            // For each class
            for (int c = 0; c < Classes.Count; c++)
            {
                // Get the Kernel matrix class subset
                double[,] Kc = K.Submatrix(Classes[c].Indexes);
                int count = Kc.GetLength(0);

                // Get the Kernel matrix class mean
                double[] mean = Tools.Mean(Kc);


                // Construct the Kernel equivalent of the Within-Class Scatter matrix
                double[,] Swi = Tools.Scatter(Kc, mean, (double)count);

                // Sw = Sw + Swi
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sw[i, j] += Swi[i, j];


                // Construct the Kernel equivalent of the Between-Class Scatter matrix
                double[] d = mean.Subtract(base.Means);
                double[,] Sbi = d.Multiply(d.Transpose()).Multiply(total);

                // Sb = Sb + Sbi
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sb[i, j] += Sbi[i, j];


                // Store additional information
                base.ClassScatter[c] = Swi;
                base.ClassCount[c] = count;
                base.ClassMeans[c] = mean;
                base.ClassStandardDeviations[c] = Tools.StandardDeviation(Kc, mean);
            }


            // Add regularization
            for (int i = 0; i < dimension; i++)
                Sw[i, i] += regularization;


            // Compute eigen value decomposition
            double[,] C = Matrix.Inverse(Sw).Multiply(Sb);
            EigenvalueDecomposition evd = new EigenvalueDecomposition(C);


            // Gets the eigenvalues and corresponding eigenvectors
            double[] evals = evd.RealEigenvalues;
            double[,] eigs = evd.Eigenvectors;

            // Sort eigen values and vectors in ascending order
            eigs = Matrix.Sort(evals, eigs, new GeneralComparer(ComparerDirection.Descending, true));


            if (threshold > 0)
            {
                // Calculate proportions earlier
                double sum = 0.0;
                for (int i = 0; i < dimension; i++)
                    sum += System.Math.Abs(evals[i]);

                if (sum > 0)
                {
                    sum = 1.0 / sum;

                    // Discard less important eigenvectors to conserve memory
                    int keep = 0; while (keep < dimension &&
                        System.Math.Abs(evals[keep]) * sum > threshold) keep++;
                    eigs = eigs.Submatrix(0, dimension - 1, 0, keep - 1);
                    evals = evals.Submatrix(0, keep - 1);
                }
            }


            // Store information
            base.Eigenvalues = evals;
            base.DiscriminantMatrix = eigs;
            base.ScatterBetweenClass = Sb;
            base.ScatterWithinClass = Sw;


            // Compute feature space means for later classification
            for (int c = 0; c < Classes.Count; c++)
            {
                double[] mean = new double[eigs.GetLength(1)];
                for (int i = 0; i < eigs.GetLength(0); i++)
                    for (int j = 0; j < eigs.GetLength(1); j++)
                        mean[j] += ClassMeans[c][i] * eigs[i, j];
                kernelClassMeans[c] = mean;
            }


            // Computes additional information about the analysis and creates the
            //  object-oriented structure to hold the discriminants found.
            createDiscriminants();
        }

        /// <summary>Projects a given matrix into discriminant space.</summary>
        /// <param name="data">The matrix to be projected.</param>
        /// <param name="discriminants">The number of discriminants to use in the projection.</param>
        public override double[,] Transform(double[,] data, int discriminants)
        {
            // Get some information
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);
            int N = Source.GetLength(0);


            // Create the Kernel matrix
            double[,] K = new double[rows, N];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < N; j++)
                    K[i, j] = kernel.Function(Source.GetRow(j), data.GetRow(i));


            // Project into the kernel discriminant space
            double[,] result = new double[rows, discriminants];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < discriminants; j++)
                    for (int k = 0; k < N; k++)
                        result[i, j] += K[i, k] * DiscriminantMatrix[k, j];


            return result;
        }

        /// <summary>
        ///   Gets the discriminant function output for class c.
        /// </summary>
        /// <param name="c">The class index.</param>
        /// <param name="projection">The projected input.</param>
        internal override double discriminantFunction(int i, double[] projection)
        {
            return -Distance.SquareEuclidean(projection, kernelClassMeans[i]);
        }
        #endregion

    }
}
