// Accord Statistics Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Accord.Math;
using Accord.Math.Decompositions;

namespace Accord.Statistics.Analysis
{
    /// <summary>
    ///   Linear Discriminant Analysis (LDA) is a method of finding such a linear
    ///   combination of variables which best separates two or more classes.
    /// </summary>
    /// <remarks>
    ///   In itself LDA is not a classification algorithm, although it makes use of class
    ///   labels. However, the LDA result is mostly used as part of a linear classifier.
    ///   The other alternative use is making a dimension reduction before using nonlinear
    ///   classification algorithms.
    ///   
    ///   It should be noted that several similar techniques (differing in requirements to the sample)
    ///   go together under the general name of Linear Discriminant Analysis. Described below is one of
    ///   these techniques with only two requirements:
    ///   
    ///      (1) The sample size shall exceed the number of variables, and 
    ///      (2) Classes may overlap, but their centers shall be distant from each other.
    ///      
    ///   Moreover, LDA requires the following assumptions to be true:
    ///      
    ///       Independent subjects.
    ///       Normality: the variance-covariance matrix of the predictors is the same in all groups.
    ///   
    ///   If the latter assumption is violated, it is common to use quadratic discriminant analysis in
    ///   the same manner as linear discriminant analysis instead.
    ///      
    ///    References:
    ///     - http://cmp.felk.cvut.cz/cmp/software/stprtool/manual/linear/extraction/list/lda.html
    ///     - http://research.cs.tamu.edu/prism/lectures/pr/pr_l10.pdf
    /// </remarks>
    public class LinearDiscriminantAnalysis
    {
        private int dimension;
        private int samples;
        private int classes;

        private double[] totalMeans;
        private double[] totalStdDevs;

        private int[] classCount;
        private double[][] classMeans;
        private double[][] classStdDevs;
        private double[][,] classScatter;
        private double[][] classMeansTransformed;

        private double[,] eigenVectors;
        private double[] eigenValues;
        private double[] bias;

        private double[,] result;
        private double[,] source;
        private int[] outputs;

        double[,] Sw, Sb, St; // Scatter matrices

        private double[] discriminantProportions;
        private double[] discriminantCumulative;

        DiscriminantCollection discriminantCollection;
        DiscriminantAnalysisClassCollection classCollection;


        //---------------------------------------------


        #region Constructor
        /// <summary>
        ///   Constructs a new Linear Discriminant Analysis object.
        /// </summary>
        /// <param name="data">The source data to perform analysis. The matrix should contain
        /// variables as columns and observations of each variable as rows.</param>
        /// <param name="output">The labels for each observation row in the input matrix.</param>
        public LinearDiscriminantAnalysis(double[,] inputs, int[] output)
        {
            // Gets the number of classes
            int startingClass = output.Min();
            this.classes = output.Max() - startingClass + 1;

            // Store the original data
            this.source = inputs;
            this.outputs = output;
            this.samples = inputs.GetLength(0);
            this.dimension = inputs.GetLength(1);

            // Creates simple structures to hold information later
            this.classCount = new int[classes];
            this.classMeans = new double[classes][];
            this.classStdDevs = new double[classes][];
            this.classScatter = new double[classes][,];


            // Creates the object-oriented structure to hold information about the classes
            DiscriminantAnalysisClass[] collection = new DiscriminantAnalysisClass[classes];
            for (int i = 0; i < classes; i++)
                collection[i] = new DiscriminantAnalysisClass(this, i, startingClass + i);
            this.classCollection = new DiscriminantAnalysisClassCollection(collection);
        }
        #endregion


        //---------------------------------------------


        #region Properties
        /// <summary>Returns the original supplied data to be analyzed.</summary>
        public double[,] Source
        {
            get { return this.source; }
        }

        /// <summary>
        ///   Gets the resulting projection of the source data given on
        ///   the creation of the analysis into discriminant space.
        /// </summary>
        public double[,] Result
        {
            get { return this.result; }
        }

        /// <summary>
        ///   Gets the original classifications (labels) of the source data
        ///   given on the moment of creation of this analysis object.
        /// </summary>
        public int[] Classifications
        {
            get { return this.outputs; }
        }

        /// <summary>Gets the mean of the original data given at method construction.</summary>
        public double[] Means
        {
            get { return totalMeans; }
            protected set { totalMeans = value; }
        }

        /// <summary>Gets the standard mean of the original data given at method construction.</summary>
        public double[] StandardDeviations
        {
            get { return totalStdDevs; }
            protected set { totalStdDevs = value; }
        }

        /// <summary>Gets the Within-Class Scatter Matrix for the data.</summary>
        public double[,] ScatterWithinClass
        {
            get { return Sw; }
            protected set { Sw = value; }
        }

        /// <summary>Gets the Between-Class Scatter Matrix for the data.</summary>
        public double[,] ScatterBetweenClass
        {
            get { return Sb; }
            protected set { Sb = value; }
        }

        /// <summary>Gets the Total Scatter Matrix for the data.</summary>
        public double[,] ScatterMatrix
        {
            get { return St; }
            protected set { St = value; }
        }

        /// <summary>
        ///   Gets the Eigen Vectors obtained by the analysis,
        ///   consisting in a basis for the discriminant space.
        /// </summary>
        public double[,] DiscriminantMatrix
        {
            get { return eigenVectors; }
            protected set { eigenVectors = value; }
        }

        /// <summary>
        ///   Gets the Eigen Values found by the analysis associated
        ///   with each vector of the DiscriminantMatrix matrix.
        /// </summary>
        public double[] Eigenvalues
        {
            get { return eigenValues; }
            protected set { eigenValues = value; }
        }

        /// <summary>Gets the level of importance each discriminant has in
        /// discriminant space. Also known as ammount of variance explained.</summary>
        public double[] Proportions
        {
            get { return discriminantProportions; }
        }

        /// <summary>The cumulative distribution of the discriminants proportion role.
        /// Also known as the cumulative energy of the first dimensions of the discriminant
        /// space or as the ammount of variance explained by those dimensions.</summary>
        public double[] CumulativeProportions
        {
            get { return discriminantCumulative; }
        }

        /// <summary>Gets the discriminants in a object-oriented fashion.</summary>
        public DiscriminantCollection Discriminants
        {
            get { return discriminantCollection; }
        }

        /// <summary>Gets the different Classes contained in the
        /// analysed data set in a object-oriented fashion.</summary>
        public DiscriminantAnalysisClassCollection Classes
        {
            get { return classCollection; }
        }

        /// <summary>
        ///   Gets the Scatter matrix for each class.
        /// </summary>
        public double[][,] ClassScatter
        {
            get { return classScatter; }
        }

        /// <summary>
        ///   Gets the Mean vector for each class.
        /// </summary>
        public double[][] ClassMeans
        {
            get { return classMeans; }
        }

        /// <summary>
        ///   Gets the Standard Deviation vector for each class.
        /// </summary>
        public double[][] ClassStandardDeviations
        {
            get { return classStdDevs; }
        }

        /// <summary>
        ///   Gets the observation count for each class.
        /// </summary>
        public int[] ClassCount
        {
            get { return classCount; }
        }
        #endregion


        //---------------------------------------------


        #region Public Methods
        /// <summary>
        ///   Computes the Multi-Class Linear Discriminant Analysis algorithm.
        /// </summary>
        public virtual void Compute()
        {
            // Compute entire data set measures
            Means = Tools.Mean(source);
            StandardDeviations = Tools.StandardDeviation(source, totalMeans);
            double total = dimension;

            // Initialize the scatter matrices
            this.Sw = new double[dimension, dimension];
            this.Sb = new double[dimension, dimension];


            // For each class
            for (int c = 0; c < Classes.Count; c++)
            {
                // Get the class subset
                double[,] subset = Classes[c].Subset;
                int count = subset.GetLength(0);

                // Get the class mean
                double[] mean = Tools.Mean(subset);


                // Continue constructing the Within-Class Scatter Matrix
                double[,] Swi = Tools.Scatter(subset, mean, (double)count);
                
                // Sw = Sw + Swi
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sw[i, j] += Swi[i, j];


                // Continue constructing the Between-Class Scatter Matrix
                double[] d = mean.Subtract(totalMeans);
                double[,] Sbi = d.Multiply(d.Transpose()).Multiply(total);
                
                // Sb = Sb + Sbi
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sb[i, j] += Sbi[i, j];


                // Store some additional information
                this.classScatter[c] = Swi;
                this.classCount[c] = count;
                this.classMeans[c] = mean;
                this.classStdDevs[c] = Tools.StandardDeviation(subset, mean);
            }


            // Compute eigen value decomposition
            EigenvalueDecomposition evd = new EigenvalueDecomposition(Matrix.Inverse(Sw).Multiply(Sb));

            // Gets the eigenvalues and corresponding eigenvectors
            double[] evals = evd.RealEigenvalues;
            double[,] eigs = evd.Eigenvectors;


            // Sort eigen values and vectors in ascending order
            eigs = Matrix.Sort(evals, eigs, new GeneralComparer(ComparerDirection.Descending, true));


            // Store information
            this.Eigenvalues = evals;
            this.DiscriminantMatrix = eigs;


            // Create discriminant functions bias
            bias = new double[classes];
            for (int i = 0; i < classes; i++)
            {
                bias[i] = (-0.5).Multiply(classMeans[i]).Multiply(
                    eigs.Multiply(classMeans[i])) +
                    System.Math.Log(classCount[i] / total);
            }

            // Create projections
            this.result = new double[dimension, dimension];
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    for (int k = 0; k < dimension; k++)
                        result[i, j] += source[i, k] * eigenVectors[k, j];


            // Computes additional information about the analysis and creates the
            //  object-oriented structure to hold the discriminants found.
            createDiscriminants();
        }

        /// <summary>Projects a given matrix into discriminant space.</summary>
        /// <param name="data">The matrix to be projected.</param>
        public double[,] Transform(double[,] data)
        {
            return Transform(data, discriminantCollection.Count);
        }

        /// <summary>Projects a given matrix into discriminant space.</summary>
        /// <param name="data">The matrix to be projected.</param>
        /// <param name="dimensions">The number of discriminants to use in the projection.</param>
        public virtual double[,] Transform(double[,] data, int discriminants)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[,] r = new double[rows, discriminants];

            // multiply the data matrix by the selected eigenvectors
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < discriminants; j++)
                    for (int k = 0; k < cols; k++)
                        r[i, j] += data[i, k] * eigenVectors[k, j];

            return r;
        }

        /// <summary>Projects a given point into discriminant space.</summary>
        /// <param name="data">The point to be projected.</param>
        public double[] Transform(double[] data)
        {
            return Transform(data.ToMatrix()).GetRow(0);
        }

        /// <summary>Projects a given point into discriminant space.</summary>
        /// <param name="data">The point to be projected.</param>
        public double[] Transform(double[] data, int discriminants)
        {
            return Transform(data.ToMatrix(),discriminants).GetRow(0);
        }

        /// <summary>
        ///   Returns the minimal number of dimensions required
        ///   to represent a given percentile of the data.
        /// </summary>
        /// <param name="threshold">The percentile of the data requiring representation.</param>
        /// <returns>The minimal number of dimensions required.</returns>
        public int GetNumberOfDimensions(float threshold)
        {
            if (threshold < 0 || threshold > 1.0)
                throw new ArgumentException("Threshold should be a value between 0 and 1", "threshold");

            for (int i = 0; i < discriminantCumulative.Length; i++)
            {
                if (discriminantCumulative[i] >= threshold)
                    return i + 1;
            }

            return discriminantCumulative.Length;
        }

        /// <summary>
        ///   Classifies a new instance into one of the available classes.
        /// </summary>
        public int Classify(double[] input)
        {
            double[] projection = Transform(input);

            // Select class which higher discriminant function fy
            int imax = 0;
            double max = discriminantFunction(0, projection);
            for (int i = 1; i < classCollection.Count; i++)
            {
                double fy = discriminantFunction(i, projection);
                if (fy > max)
                {
                    max = fy;
                    imax = i;
                }
            }

            return classCollection[imax].Number;
        }

        /// <summary>
        ///   Classifies a new instance into one of the available classes.
        /// </summary>
        public int Classify(double[] input, out double[] responses)
        {
            double[] projection = Transform(input);
            responses = new double[classCollection.Count];

            // Select class which higher discriminant function fy
            int imax = 0;
            double max = discriminantFunction(0, projection);
            responses[0] = max;
            for (int i = 1; i < classCollection.Count; i++)
            {
                double fy = discriminantFunction(i, projection);
                responses[i] = fy;
                if (fy > max)
                {
                    max = fy;
                    imax = i;
                }
            }

            return classCollection[imax].Number;
        }

        /// <summary>
        ///   Classifies new instances into one of the available classes.
        /// </summary>
        public int[] Classify(double[][] inputs)
        {
            int[] output = new int[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
                output[i] = Classify(inputs[i]);
            return output;
        }

        /// <summary>
        ///   Gets the discriminant function output for class c.
        /// </summary>
        /// <param name="c">The class index.</param>
        /// <param name="projection">The projected input.</param>
        internal virtual double discriminantFunction(int c, double[] projection)
        {
            return classMeans[c].Multiply(projection) + bias[c];
        }
        #endregion


        //---------------------------------------------


        #region Protected Methods
        /// <summary>
        ///   Creates additional information about principal components.
        /// </summary>
        protected void createDiscriminants()
        {
            int numDiscriminants = eigenValues.Length;
            discriminantProportions = new double[numDiscriminants];
            discriminantCumulative = new double[numDiscriminants];


            // Calculate total scatter matrix
            int size = Sw.GetLength(0);
            St = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    St[i, j] = Sw[i, j] + Sb[i, j];


            // Calculate proportions
            double sum = 0.0;
            for (int i = 0; i < numDiscriminants; i++)
                sum += System.Math.Abs(eigenValues[i]);
            sum = (sum == 0) ? 0 : (1.0 / sum);

            for (int i = 0; i < numDiscriminants; i++)
                discriminantProportions[i] = System.Math.Abs(eigenValues[i]) * sum;


            // Calculate cumulative proportions
            this.discriminantCumulative[0] = this.discriminantProportions[0];
            for (int i = 1; i < this.discriminantCumulative.Length; i++)
                this.discriminantCumulative[i] = this.discriminantCumulative[i - 1] + this.discriminantProportions[i];


            // Creates the object-oriented structure to hold the linear discriminants
            Discriminant[] discriminants = new Discriminant[numDiscriminants];
            for (int i = 0; i < numDiscriminants; i++)
                discriminants[i] = new Discriminant(this, i);
            this.discriminantCollection = new DiscriminantCollection(discriminants);
        }
        #endregion

    }


    #region Support Classes
    /// <summary>
    ///   Class representing a Class found during Discriminant Analysis,
    ///   allowing it to be bound to controls like the DataGridView.
    ///   This class cannot be instantiated outside LinearDiscriminantAnalysis.
    /// </summary>
    public class DiscriminantAnalysisClass
    {
        private LinearDiscriminantAnalysis analysis;
        private int classNumber;
        private int index;

        /// <summary>
        ///   Creates a new Class representation
        /// </summary>
        internal DiscriminantAnalysisClass(LinearDiscriminantAnalysis analysis, int index, int classNumber)
        {
            this.analysis = analysis;
            this.index = index;
            this.classNumber = classNumber;
        }

        /// <summary>
        ///   Gets the Index of this class on the original analysis collection.
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        ///   Gets the number labelling this class.
        /// </summary>
        public int Number
        {
            get { return classNumber; }
        }

        /// <summary>
        ///   Gets the prevalence of the class on the original data set.
        /// </summary>
        public double Prevalence
        {
            get { return (double)Count / analysis.Source.GetLength(0); }
        }

        /// <summary>
        ///   Gets the class' mean vector.
        /// </summary>
        public double[] Mean
        {
            get { return analysis.ClassMeans[index]; }
        }

        /// <summary>
        ///   Gets the class' standard deviation vector.
        /// </summary>
        public double[] StandardDeviation
        {
            get { return analysis.ClassStandardDeviations[index]; }
        }

        /// <summary>
        ///   Gets the Scatter matrix for this class.
        /// </summary>
        public double[,] Scatter
        {
            get { return analysis.ClassScatter[index]; }
        }

        /// <summary>
        ///   Gets the indexes of the rows in the original data which belong to this class.
        /// </summary>
        public int[] Indexes
        {
            get { return Matrix.Find(analysis.Classifications, y => y == classNumber); }
        }

        /// <summary>
        ///   Gets the subset of the original data spawned by this class.
        /// </summary>
        public double[,] Subset
        {
            get
            {
                return analysis.Source.Submatrix(Indexes);
            }
        }

        /// <summary>
        ///   Gets the number of observations inside this class.
        /// </summary>
        public int Count
        {
            get { return analysis.ClassCount[index]; }
        }

        /// <summary>
        ///   Discriminant function for the class.
        /// </summary>
        public double DiscriminantFunction(double[] projection)
        {
            //return Mean.Multiply(projection) + Bias[index];
            return analysis.discriminantFunction(index, projection);
        }
    }

    /// <summary>
    ///   Class representing a Discriminant found during Discriminant Analysis,
    ///   allowing it to be bound to controls like the DataGridView.
    ///   This class cannot be instantiated outside LinearDiscriminantAnalysis.
    /// </summary>
    public class Discriminant
    {
        private LinearDiscriminantAnalysis analysis;
        private int index;

        /// <summary>
        ///   Creates a new Discriminant representation.
        /// </summary>
        internal Discriminant(LinearDiscriminantAnalysis analysis, int index)
        {
            this.analysis = analysis;
            this.index = index;
        }

        /// <summary>
        ///   Gets the Index of this discriminant on the original analysis collection.
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        ///   Gets the Eiven vector of this discriminant.
        /// </summary>
        public double[] Eigenvector
        {
            get { return analysis.DiscriminantMatrix.GetColumn(index); }
        }

        /// <summary>
        ///   Gets the Eigen value for this discriminant.
        /// </summary>
        public double Eigenvalue
        {
            get { return analysis.Eigenvalues[index]; }
        }

        /// <summary>
        ///   Gets the proportion, or ammount of information explained by this discriminant.
        /// </summary>
        public double Proportion
        {
            get { return analysis.Proportions[index]; }
        }

        /// <summary>
        ///   Gets the cumulative proportion of all discriminants until this.
        /// </summary>
        public double CumulativeProportion
        {
            get { return analysis.CumulativeProportions[index]; }
        }

    }

    /// <summary>
    ///   Represents a Collection of Discriminants found in the Discriminant Analysis.
    ///   This class cannot be instantiated.
    /// </summary>
    public class DiscriminantCollection : ReadOnlyCollection<Discriminant>
    {
        internal DiscriminantCollection(Discriminant[] components)
            : base(components)
        {
        }
    }

    /// <summary>
    ///   Represents a Collection of Discriminants found in the Discriminant Analysis.
    ///   This class cannot be instantiated.
    /// </summary>
    public class DiscriminantAnalysisClassCollection : ReadOnlyCollection<DiscriminantAnalysisClass>
    {
        internal DiscriminantAnalysisClassCollection(DiscriminantAnalysisClass[] components)
            : base(components)
        {
        }
    }
    #endregion

}


