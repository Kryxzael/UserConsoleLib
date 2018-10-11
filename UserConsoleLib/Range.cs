using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserConsoleLib
{
    /// <summary>
    /// Represents a range of numbers
    /// </summary>
    public struct Range : IComparable<Range>
    {
        /// <summary>
        /// The smallest value of this range
        /// </summary>
        public double Minimum { get; }

        /// <summary>
        /// The biggest value of this range
        /// </summary>
        public double Maximum { get; }

        /// <summary>
        /// Is the length of this range infinite?
        /// </summary>
        public bool IsInfinite => !HasStart || !HasEnd;

        /// <summary>
        /// Is the minimum value the same as the maximum?
        /// </summary>
        public bool IsPoint => Minimum == Maximum;

        /// <summary>
        /// Does this range have a non-infinite minimum?
        /// </summary>
        public bool HasStart => !double.IsInfinity(Minimum);

        /// <summary>
        /// Does this range have a non-infinite maximum?
        /// </summary>
        public bool HasEnd => !double.IsInfinity(Maximum);

        /// <summary>
        /// The difference between the minimum and maximum value
        /// </summary>
        public double Length => Maximum - Minimum;

        /// <summary>
        /// An empty range. This field is read-only
        /// </summary>
        public static readonly Range ZERO = new Range(0.0, 0.0);

        /// <summary>
        /// A point range of one. This field is read-only
        /// </summary>
        public static readonly Range ONE = new Range(1.0, 1.0);

        /// <summary>
        /// A range containing every number. This field is read-only
        /// </summary>
        public static readonly Range INFINITY = From(float.NegativeInfinity);

        private Range(double min, double max)
        {
            Minimum = min;
            Maximum = max;
        }

        /// <summary>
        /// Begins a new range starting at a specific point and going to infinity
        /// </summary>
        /// <param name="start">The start point</param>
        /// <returns></returns>
        public static Range From(double start)
        {
            return new Range(start, double.PositiveInfinity);
        }

        /// <summary>
        /// Specifies a range that goes from infinity to a specific point
        /// </summary>
        /// <param name="end">The end point</param>
        /// <returns></returns>
        public static Range UpTo(double end)
        {
            return new Range(double.NegativeInfinity, end);
        }

        /// <summary>
        /// Finishes a range by specifing its end point
        /// </summary>
        /// <param name="end">The end point</param>
        /// <returns></returns>
        public Range To(double end)
        {
            return new Range(Minimum, end);
        }

        /// <summary>
        /// Does this range contain a specific number?
        /// </summary>
        /// <param name="value">Value to check if in range</param>
        /// <returns></returns>
        public bool IsInRange(double value)
        {
            return value >= Minimum && value <= Maximum;
        }

        /// <summary>
        /// Clamps the value so that it is within the range
        /// </summary>
        /// <param name="value">Value to clamp</param>
        /// <returns></returns>
        public double Clamp(double value)
        {
            return Math.Min(Math.Max(value, Minimum), Maximum);
        }

        /// <summary>
        /// Checks if two ranges houses the same exact values
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns></returns>
        public static bool operator ==(Range left, Range right)
        {
            return left.Minimum == right.Minimum && left.Maximum == right.Maximum;
        }

        /// <summary>
        /// Checks if two ranges differ from each other in any way
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns></returns>
        public static bool operator !=(Range left, Range right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Checks if this Range is considered equal to another object
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Range r)
            {
                return this == r;
            }

            return false;
        }

        /// <summary>
        /// Gets the hashing code for this Range
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Minimum.GetHashCode() ^ Maximum.GetHashCode();
        }

        /// <summary>
        /// Compares this Range to another for sorting purposes
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Range other)
        {
            int comp = other.Minimum.CompareTo(Minimum);

            if (comp == 0)
            {
                return other.Maximum.CompareTo(Maximum);
            }

            return comp;
        }

        /// <summary>
        /// Returns a string representation of this Range
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsPoint)
            {
                return Minimum.ToString();
            }

            if (this == INFINITY)
            {
                return "..";
            }

            if (!HasEnd)
            {
                return Minimum + "..";
            }
            else if (!HasStart)
            {
                return ".." + Maximum;
            }

            return Minimum + ".." + Maximum;
        }
    }
}
