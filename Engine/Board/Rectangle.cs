using System;
using System.IO;

namespace Noxico.Engine.Board
{
    /// <summary>
    /// Stores a set of four integers that represent the location and size of a rectangle.
    /// Basically a version of <see cref="System.Drawing.Rectangle"/> that replaces the extra stuff with a single feature.
    /// </summary>
    public struct Rectangle : IEquatable<Rectangle>
    {
        /// <summary>
        /// Gets the x-coordinate of the left edge of this <see cref="Noxico.Rectangle"/> structure.
        /// </summary>
        public int Left { get; set; }

        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        /// <summary>
        /// Creates and returns a <see cref="Noxico.Point"/> that is the centerpoint of this <c>Noxico.Rectangle</c>.
        /// </summary>
        /// <returns></returns>
        public Point GetCenter()
        {
            return new Point(Left + ((Right - Left) / 2), Top + ((Bottom - Top) / 2));
        }

        public void SaveToFile(BinaryWriter stream)
        {
            stream.Write(Left);
            stream.Write(Top);
            stream.Write(Right);
            stream.Write(Bottom);
        }

        public static Rectangle LoadFromFile(BinaryReader stream)
        {
            var l = stream.ReadInt32();
            var t = stream.ReadInt32();
            var r = stream.ReadInt32();
            var b = stream.ReadInt32();
            return new Rectangle {Left = l, Top = t, Right = r, Bottom = b};
        }

        public bool Equals(Rectangle other)
        {
            return other.Top == Top && other.Left == Left && other.Bottom == Bottom && other.Right == Right;
        }
    }
}