using System.Windows;
using System.Windows.Media;
using SpaceInvaders.GameObjects.Bullets;

namespace SpaceInvaders.GameObjects
{
    public class Alien : Character
    {
        public Alien(double x, double y) : base(x, y)
        {
            CurrentSpeed = 1;
            Rotation = 3.15f;
            Color = Brushes.ForestGreen;
        }

        public void Update(Movement currentMovement, int speedMultiplier = 1)
        {
            var multiplier = (CurrentSpeed * speedMultiplier);

            switch (currentMovement)
            {
                case Movement.Left:
                    Position = new Point(Position.X - multiplier, Position.Y);
                    break;

                case Movement.Right:
                    Position = new Point(Position.X + multiplier, Position.Y);
                    break;

                case Movement.Down:
                    Position = new Point(Position.X, Position.Y + 12.5);
                    break;
            }

            base.Update();
        }

        public void Shoot()
        {
            Bullets.Add(new AlienBullet(Collider.Center.X-14, Collider.Center.Y - (Collider.Height / 2)));
        }

        public override void HandleCollision()
        {
            IsAlive = false;
            Bullets.Clear();
            base.HandleCollision();
        }
    }
}