using System.Windows;

namespace SpaceInvaders.GameObjects.Bullets
{
    public class PlayerBullet : Bullet
    {
        public PlayerBullet(double x, double y) : base(x, y)
        {
        }

        public override void Update()
        {
            Position = new Point(Position.X, Position.Y - 12.5);
            base.Update();
        }
    }
}
