using System.Collections.Generic;
using System.Linq;
using SpaceInvaders.GameObjects;

namespace SpaceInvaders
{
    public static class CollisionDetection
    {
        public static bool CharacterBulletCollided(Character character, IEnumerable<GameObject> gameObjects, bool destroyOnCollide = true)
        {
            if (!gameObjects.ToList().Any())
                return false;

            var anyCollision = false;

            foreach (var bullet in character.Bullets.ToList())
            {
                foreach (var gameObject in gameObjects)
                {
                    if (bullet.Collider == null || !bullet.Collider.Intersects(gameObject.Collider))
                        continue;

                    if(destroyOnCollide)
                        gameObject.HandleCollision();

                    character.Bullets.Remove(bullet);
                    bullet.Destroy();
                    anyCollision = true;
                    break;
                }
            }

            return anyCollision;
        }

        public static void PlayerBulletBunkerCollision(Player player, IEnumerable<GameObject> bunkers)
        {
            CharacterBulletCollided(player, bunkers, false);
        }

        public static void AlienBulletBunkerCollision(Alien alien, IEnumerable<GameObject> bunkers)
        {
            CharacterBulletCollided(alien, bunkers);
        }
    }
}