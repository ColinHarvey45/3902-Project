using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Sprites;

namespace Projectiles
{
    // Link's sword while he swings it: one of his projectiles, lasting as long as the swing, so the collisions treat it
    // like his other weapons. It hits every enemy it touches (each only once, as a hit enemy can't be hurt again for
    // a moment) and knocks them back away from Link. It isn't drawn; Link's attacking sprite shows the sword
    internal sealed class SwordSwing : IProjectile
    {
        private const int SwordDamage = 1;

        // The hitbox is 12 x 12 sheet pixels, one tile (16 sheet pixels) in front of Link
        private const float Reach = 16 * Sprite.Scale;
        private const int HitboxSize = (int)(12 * Sprite.Scale);

        private readonly Vector2 linkMiddle;
        private readonly float duration;
        private float timer;

        // linkPosition is Link's top-left corner and linkMiddle his middle; duration is how long the swing lasts
        public SwordSwing(Vector2 linkPosition, Vector2 linkMiddle, Direction facing, float duration)
        {
            this.linkMiddle = linkMiddle;
            this.duration = duration;

            Point inFront = (facing.ToVector() * Reach).ToPoint();
            Collider = new BoxCollider(new Rectangle(0, 0, HitboxSize, HitboxSize), inFront);
            Collider.Update(linkPosition);
        }

        public ICollider Collider { get; set; }
        public bool IsFinished => timer >= duration;
        public ProjectileOwner Owner => ProjectileOwner.Link;
        public bool IsBlast => false;

        public void HitEnemy(IDamageable enemy)
        {
            enemy.TakeDamage(SwordDamage, linkMiddle);
        }

        // Hitting something doesn't stop the swing
        public void Hit() { }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}
