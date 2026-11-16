using Interfaces;
using Microsoft.Xna.Framework;
using Player;
using Sprites;

namespace Projectiles
{
    // Makes every projectile, so whoever fires one (Link, Goriya, Aquamentus, the Old Man's fires) only deals with
    // IProjectile and never with a particular kind of projectile
    internal static class ProjectileFactory
    {
        // middle is where the middle of the arrow starts
        public static IProjectile CreateArrow(Vector2 middle, Direction direction)
        {
            return new Arrow(middle, direction);
        }

        // Link's boomerang stuns enemies; middle is where its middle starts, and it comes back to thrower
        public static IProjectile CreateLinkBoomerang(Vector2 middle, Direction direction, IBoomerangThrower thrower)
        {
            return new Boomerang(ProjectileSpriteFactory.Instance.CreateLinkBoomerangSprite(), middle, direction.ToVector(),
                thrower, ProjectileOwner.Link);
        }

        // A Goriya's boomerang hurts Link; it flies in direction, which may be any way, and comes back to thrower
        public static IProjectile CreateGoriyaBoomerang(Vector2 middle, Vector2 direction, IBoomerangThrower thrower)
        {
            return new Boomerang(ProjectileSpriteFactory.Instance.CreateGoriyaBoomerangSprite(), middle, direction,
                thrower, ProjectileOwner.Enemy);
        }

        // position is the bomb's top-left corner
        public static IProjectile CreateBomb(Vector2 position)
        {
            return new Bomb(position);
        }

        // velocity is in pixels per second
        public static IProjectile CreateFireball(Vector2 position, Vector2 velocity)
        {
            return new Fireball(position, velocity);
        }

        public static IProjectile CreateSwordSwing(Vector2 linkPosition, Vector2 linkMiddle, Direction facing, float duration)
        {
            return new SwordSwing(linkPosition, linkMiddle, facing, duration);
        }
    }
}
