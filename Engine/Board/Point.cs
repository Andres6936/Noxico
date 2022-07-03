using System;

namespace Noxico.Engine.Board
{
    /// <summary>
    /// Represents an ordered pair of integer x- and y-coordinates that defines a point in a two-dimensional plane.
    /// Basically a version of <see cref="System.Drawing.Point"/> without the extra stuff.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Noxico.Point"/> class with the specified coordinates.
        /// </summary>
        /// <param name="x">The horizontal position of the point.</param>
        /// <param name="y">The vertical position of the point.</param>
        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Compares two <see cref="Noxico.Point"/> objects.
        /// The result specifies whether the values of the <see cref="Noxico.Point.X"/> and <see cref="Noxico.Point.Y"/> properties of the two <see cref="Noxico.Point"/> objects are equal.
        /// </summary>
        /// <param name="l">A <see cref="Noxico.Point"/> to compare.</param>
        /// <param name="r">A <see cref="Noxico.Point"/> to compare.</param>
        /// <returns>true if the <see cref="Noxico.Point.X"/> and <see cref="Noxico.Point.Y"/> values of left and right are equal; otherwise, false.</returns>
        public static bool operator ==(Point l, Point r)
        {
            return l.X == r.X && l.Y == r.Y;
        }

        /// <summary>
        /// Compares two <see cref="Noxico.Point"/> objects.
        /// The result specifies whether the values of the <see cref="Noxico.Point.X"/> and <see cref="Noxico.Point.Y"/> properties of the two <see cref="Noxico.Point"/> objects are unequal.
        /// </summary>
        /// <param name="l">A <see cref="Noxico.Point"/> to compare.</param>
        /// <param name="r">A <see cref="Noxico.Point"/> to compare.</param>
        /// <returns>true if the <see cref="Noxico.Point.X"/> and <see cref="Noxico.Point.Y"/> values of left and right differ; otherwise, false.</returns>
        public static bool operator !=(Point l, Point r)
        {
            return !(l == r);
        }

        /// <summary>
        /// Specifies whether this <see cref="Noxico.Point"/> contains the same coordinates as the specified <c>System.Object</c>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to test.</param>
        /// <returns>true if <paramref name="obj"/> is a <see cref="Noxico.Point"/> and has the same coordinates as this <see cref="Noxico.Point"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
                return false;
            var opt = (Point) obj;
            return opt.X == X && opt.Y == Y;
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Noxico.Point"/>.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this <see cref="Noxico.Point"/>.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Noxico.Point"/> to a human-readable string.
        /// </summary>
        /// <returns>A string that represents this <see cref="Noxico.Point"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}x{1}", X, Y);
        }

        public bool Equals(Point other)
        {
            return other.X == X && other.Y == Y;
        }
    }
}