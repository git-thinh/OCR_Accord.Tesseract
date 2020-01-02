// AccordSystem.Math.Library
// Accord.NET framework
// http://www.crsouza.com
//
// Copyright © César Souza, 2009-2010
// cesarsouza at gmail.com
//

namespace Accord.Math
{
    using System;
    using System.Collections.Generic;
    using AForge;

    /// <summary>
    ///   Set of mathematical tools.
    /// </summary>
    public static class Tools
    {

        /// <summary>
        ///   Returns the next power of 2 after the input value x.
        /// </summary>
        /// <param name="x">Input value x.</param>
        /// <returns>Returns the next power of 2 after the input value x.</returns>
        public static int NextPowerOf2(int x)
        {
            --x;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            return ++x;
        }

        /// <summary>
        ///   Returns the previous power of 2 after the input value x.
        /// </summary>
        /// <param name="x">Input value x.</param>
        /// <returns>Returns the previous power of 2 after the input value x.</returns>
        public static int PreviousPowerOf2(int x)
        {
            return NextPowerOf2(x + 1) / 2;
        }


        /// <summary>
        ///   Hypotenuse calculus without overflow/underflow
        /// </summary>
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// <returns>The hypotenuse Sqrt(a^2 + b^2)</returns>
        public static double Hypotenuse(double a, double b)
        {
            double r = 0.0;
            double absA = System.Math.Abs(a);
            double absB = System.Math.Abs(b);

            if (absA > absB)
            {
                r = b / a;
                r = absA * System.Math.Sqrt(1 + r * r);
            }
            else if (b != 0)
            {
                r = a / b;
                r = absB * System.Math.Sqrt(1 + r * r);
            }

            return r;
        }

        /// <summary>
        ///   Gets the proper modulus operation for
        ///   a integer x and modulo m.
        /// </summary>
        public static int Mod(int x, int m)
        {
            if (m < 0) m = -m;
            int r = x % m;
            return r < 0 ? r + m : r;
        }




        /// <summary>
        ///   Converts the value x (which is measured in the scale
        ///   'from') to another value measured in the scale 'to'.
        /// </summary>
        public static int Scale(this IntRange from, IntRange to, int x)
        {
            if (from.Length == 0) return 0;
            return (to.Length) * (x - from.Min) / from.Length + to.Min;
        }

        /// <summary>
        ///   Converts the value x (which is measured in the scale
        ///   'from') to another value measured in the scale 'to'.
        /// </summary>
        public static double Scale(this DoubleRange from, DoubleRange to, double x)
        {
            if (from.Length == 0) return 0;
            return (to.Length) * (x - from.Min) / from.Length + to.Min;
        }

        /// <summary>
        ///   Converts the value x (which is measured in the scale
        ///   'from') to another value measured in the scale 'to'.
        /// </summary>
        public static double Scale(double fromMin, double fromMax, double toMin, double toMax, double x)
        {
            if (fromMax-fromMin == 0) return 0;
            return (toMax-toMin) * (x - fromMin) / (fromMax-fromMin) + toMin;
        }


        /// <summary>
        ///   Returns the hyperbolic arc cosine of the specified value.
        /// </summary>
        public static double Acosh(double x)
        {
            if (x < 1.0)
                throw new ArgumentOutOfRangeException("x");
            return System.Math.Log(x + System.Math.Sqrt(x * x - 1));
        }


        /// <summary>
        /// Returns the hyperbolic arc sine of the specified value.
        /// </summary>
        public static double Asinh(double d)
        {
            double x;
            int sign;

            if (d == 0.0)
                return d;

            if (d < 0.0)
            {
                sign = -1;
                x = -d;
            }
            else
            {
                sign = 1;
                x = d;
            }
            return sign * System.Math.Log(x + System.Math.Sqrt(x * x + 1));
        }


        /// <summary>
        /// Returns the hyperbolic arc tangent of the specified value.
        /// </summary>
        public static double Atanh(double d)
        {
            if (d > 1.0 || d < -1.0)
                throw new ArgumentOutOfRangeException("d");
            return 0.5 * System.Math.Log((1.0 + d) / (1.0 - d));
        }


        /// <summary>
        ///   Returns the factorial of the specified value.
        /// </summary>
        public static double Factorial(double value)
        {
            double d = System.Math.Abs(value);

            // Check if the number is an integer
            if (System.Math.Floor(d) == d)
            {
                // Calculate factorial iteractively
                return (double)Factorial((int)value);
            }
            else
            {
                // Return Gamma approximation
                return Special.Gamma(value + 1.0);
            }
        }

        /// <summary>
        ///   Returns the factorial of the specified value.
        /// </summary>
        public static int Factorial(int value)
        {
            int i = System.Math.Abs(value);
            int fac = 1;

            while (i > 1)
            {
                fac *= i--;
            }

            // Return factorial with original value sign
            return (value < 0) ? -fac : fac;
        }

        /// <summary>
        ///   Returns the falling power of the specified value.
        /// </summary>
        public static int Falling(int value, int power)
        {
            int t = 1;
            for (int i = 0; i < power; i++)
                t *= power--;
            return t;
        }

        /// <summary>
        ///   Truncated power function.
        /// </summary>
        public static double TruncPower(double value, double degree)
        {
            double x = System.Math.Pow(value, degree);
            return (x > 0) ? x : 0.0;
        }

    }

    /// <summary>
    ///   Directions for the General Comparer.
    /// </summary>
    public enum ComparerDirection { Ascending, Descending };

    /// <summary>
    ///   General comparer which supports multiple directions
    ///   and comparison of absolute values.
    /// </summary>
    public class GeneralComparer : IComparer<double>
    {
        private bool absolute;
        private int direction = 1;

        /// <summary>
        ///   Constructs a new General Comparer.
        /// </summary>
        /// <param name="direction">The direction to compare.</param>
        /// <param name="useAbsoluteValues">True to compare absolute values, false otherwise.</param>
        public GeneralComparer(ComparerDirection direction, bool useAbsoluteValues)
        {
            this.direction = (direction == ComparerDirection.Ascending) ? 1 : -1;
            this.absolute = useAbsoluteValues;
        }

        /// <summary>
        ///   Compares two objects and returns a value indicating whether one is less than,
        ///    equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        public int Compare(double x, double y)
        {
            if (absolute)
            {
                return direction * (System.Math.Abs(x).CompareTo(System.Math.Abs(y)));
            }
            else
            {
                return direction * (x.CompareTo(y));
            }
        }
    }
}