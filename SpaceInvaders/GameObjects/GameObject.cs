using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceInvaders.GameObjects
{
    public abstract class GameObject
    {
        public Point Position { get; set; }
        public float Rotation { get; set; }
        public double CurrentSpeed { get; set; }
        public Brush Color { get; set; }
        public Collider Collider { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public bool IsAlive { get; set; }

        protected GameObject(double x, double y)
        {
            Position = new Point(x, y);
            Rotation = 0.0f;
            CurrentSpeed = 0;
            Color = Brushes.White;
            Width = 0;
            Height = 0;
            IsAlive = true;
        }

        public virtual Polygon Render()
        {
            if (!IsAlive)
                return null;

            var triangle = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = Color,
                StrokeThickness = 2,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var originalPoints = new Point[] { new Point(Width/2, 0), new Point(0, Height), new Point(Width, Height) };
            var myPointCollection = new PointCollection();

            for (var i = 0; i < 3; ++i)
            {
                var rotatedPoint = new Point(
                    (originalPoints[i].X * Math.Cos(Rotation)) - (originalPoints[i].Y * Math.Sin(Rotation)),
                    (originalPoints[i].Y * Math.Cos(Rotation)) + (originalPoints[i].X * Math.Sin(Rotation))
                );
                var finalPoint = new Point(rotatedPoint.X + Position.X, rotatedPoint.Y + Position.Y);
                myPointCollection.Add(finalPoint);
            }

            triangle.Points = myPointCollection;

            return triangle;
        }

        public virtual void Update()
        {
            if (!IsAlive)
                return;

            Collider = new Collider(Position.X, Position.Y, Width, Height);
        }

        public virtual void HandleCollision()
        {
            
        }
    }
}