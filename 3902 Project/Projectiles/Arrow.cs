using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;
using Colliders;

namespace Projectiles
{
    // Flies straight until it leaves the room
    internal sealed class Arrow : IProjectile
    {
        private const float Speed = 200f; // pixels per second
        // More than half an arrow's length, so the whole arrow has left the room before it is removed
        private const int OffscreenMargin = 40;
        private const int ColliderWidth = (int)(8 * Sprite.Scale);
        private const int ColliderHeight = (int)(8 * Sprite.Scale);
        private static readonly Point ColliderOffset = Point.Zero;

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position; // the middle of the arrow

        public ICollider collider { get; set; }

        public bool IsFinished { get; private set; }
        public int Damage { get; } = 1;
        public ProjectileOwner Owner => ProjectileOwner.Link;

        // startPosition is where the middle of the arrow starts
        public Arrow(Vector2 startPosition, Direction direction)
        {
            position = startPosition;
            velocity = direction.ToVector() * Speed;
            sprite = ProjectileSpriteFactory.Instance.CreateArrowSprite(direction);

            flightArea = RoomLayout.Bounds;
            flightArea.Inflate(OffscreenMargin, OffscreenMargin);

            collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, ColliderWidth, ColliderHeight),
                ColliderOffset);
        }

        public void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            collider.Update(position);

            if (!flightArea.Contains(position))
                IsFinished = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        public void Hit()
        {
            IsFinished = true;
        }
    }
}