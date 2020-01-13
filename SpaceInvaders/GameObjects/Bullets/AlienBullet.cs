using System.Windows;
using System.Windows.Media;

namespace SpaceInvaders.GameObjects.Bullets
{
    public class AlienBullet : Bullet
    {
        public AlienBullet(double x, double y) : base(x, y)
        {
            Color = Brushes.ForestGreen;
        }

        public override void Update()
        {
            Position = new Point(Position.X, Position.Y + 12.5);
            base.Update();
        }
    }
}
