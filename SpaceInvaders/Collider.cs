using System.Windows;

namespace SpaceInvaders
{
    public class Collider
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public double Left => X;
        public double Right => X + Width;
        public double Top => Y;
        public double Bottom => Y + Height;

        public Point Center => new Point(X + Width / 2, Y + Height / 2);

        public Collider(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets whether or not the other Collider intersects with this Collider.
        /// </summary>
        /// <param name="other">The other Collider for testing.</param>
        /// <returns>True/False if other Collider intersects with this.</returns>
        public bool Intersects(Collider other)
        {
            if (other.Left < Right && Left < other.Right && other.Top < Bottom)
                return Top < other.Bottom;
            return false;
        }
    }
}