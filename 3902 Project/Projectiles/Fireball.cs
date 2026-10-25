using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;

namespace Projectiles
{
    // Breathed by Aquamentus; flickers through its colours and flies straight until it leaves the room
    internal sealed class Fireball : IProjectile
    {
        private const int OffscreenMargin = 20; // lets the fireball fully leave the room before it is removed
        private const int ColliderWidth = (int)(8 * Sprites.Sprite.Scale);
        private const int ColliderHeight = (int)(10 * Sprites.Sprite.Scale);
        private static readonly Point ColliderOffset = Point.Zero;

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position;
        public ICollider collider { get; set; }

        public bool IsFinished { get; private set; }
        public int Damage { get; } = 1;
        public ProjectileOwner Owner => ProjectileOwner.Enemy;

        // velocity is in pixels per second
        public Fireball(Vector2 startPosition, Vector2 velocity)
        {
            position = startPosition;
            this.velocity = velocity;
            sprite = ProjectileSpriteFactory.Instance.CreateFireballSprite();

            flightArea = RoomLayout.Bounds;
            flightArea.Inflate(OffscreenMargin, OffscreenMargin);

            this.collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, ColliderWidth, ColliderHeight),
                ColliderOffset);
        }

        public void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            sprite.Update(gameTime);

            if (!flightArea.Contains(position))
                IsFinished = true;
            UpdateCollider();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Hit()
        {
            IsFinished = true;
        }

        private void UpdateCollider()
        {
            collider.Update(position);
        }
    }
}
