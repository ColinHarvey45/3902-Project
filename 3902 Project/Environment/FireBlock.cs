using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Rooms;
using Sprites;

namespace Environment
{
    // A fire beside the Old Man. It just burns, until Link attacks the Old Man: from then on it shoots a fireball
    // at Link every so often, as in the NES. Its fireballs go into projectiles
    internal sealed class FireBlock(Vector2 position, ProjectileManager projectiles)
        : Block(position, BlockSpriteFactory.Instance.CreateFireSprite()), ILinkWatcher, INpcGuard
    {
        private const float ShotInterval = 1.5f; // seconds
        private const float FireballSpeed = 150f; // pixels per second

        // A fireball is 8 x 10 on the sheet. It starts this far from the middle of the fire, towards Link, so that it
        // is clear of the fire (which is solid, and would stop it) whichever way it goes
        private static readonly Vector2 HalfFireball = new(4 * Sprites.Sprite.Scale, 5 * Sprites.Sprite.Scale);
        private const float LaunchDistance = 1.25f * RoomLayout.TileSize;

        private readonly Vector2 middle = position + new Vector2(RoomLayout.TileSize / 2f);
        private Rectangle? link;
        private bool provoked;
        private float timeSinceShot;

        public void Watch(Rectangle linkHitbox)
        {
            link = linkHitbox;
        }

        // Shoots at once, then every ShotInterval
        public void Provoke()
        {
            if (provoked)
                return;

            provoked = true;
            timeSinceShot = ShotInterval;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!provoked || link is not Rectangle target)
                return;

            timeSinceShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceShot < ShotInterval)
                return;

            timeSinceShot = 0f;
            Vector2 towardsLink = target.Center.ToVector2() - middle;
            if (towardsLink == Vector2.Zero)
                return;

            Vector2 direction = Vector2.Normalize(towardsLink);
            projectiles.Add(ProjectileFactory.CreateFireball(middle + direction * LaunchDistance - HalfFireball, direction * FireballSpeed));
        }
    }
}
