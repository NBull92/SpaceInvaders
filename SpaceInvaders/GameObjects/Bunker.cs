using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceInvaders.GameObjects
{
    public class Bunker : GameObject
    {
        public int Lives = 3;

        public Bunker(double x, double y) : base(x, y)
        {
            Width = 50;
            Height = 20;
        }

        public override void HandleCollision()
        {
            if (Lives > 0)
            {
                Lives--;
            }
            else
            {
                IsAlive = false;
            }
            base.HandleCollision();
        }

        public override Polygon Render()
        {
            if (!IsAlive)
                return null;

            var rectangle = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = Color,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var originalPoints = new Point[] { new Point(0, 0), new Point(Width, 0), new Point(Width, Height), new Point(0, Height) };
            var myPointCollection = new PointCollection();

            for (var i = 0; i < 4; ++i)
            {
                var rotatedPoint = new Point(
                    (originalPoints[i].X * Math.Cos(Rotation)) - (originalPoints[i].Y * Math.Sin(Rotation)),
                    (originalPoints[i].Y * Math.Cos(Rotation)) + (originalPoints[i].X * Math.Sin(Rotation))
                );
                var finalPoint = new Point(rotatedPoint.X + Position.X, rotatedPoint.Y + Position.Y);
                myPointCollection.Add(finalPoint);
            }

            rectangle.Points = myPointCollection;

            return rectangle;
        }
    }
}
