using System.Collections.Generic;
using System.Linq;
using System.Windows.Shapes;
using SpaceInvaders.GameObjects.Bullets;

namespace SpaceInvaders.GameObjects
{
    public class Character : GameObject
    {
        public readonly List<Bullet> Bullets;

        public Character(double x, double y) : base(x, y)
        {
            Bullets = new List<Bullet>();
            Width = 12;
            Height = 20;
        }

        public override Polygon Render()
        {
            return IsAlive ? base.Render() : null;
        }

        public List<Polygon> BulletRender()
        {
            if (!Bullets.Any())
                return null;

            var bullets = new List<Polygon>();

            foreach (var bullet in Bullets)
            {
                if (bullet.IsAlive)
                    bullets.Add(bullet.Render());
            }

            return bullets;
        }
    }
}
