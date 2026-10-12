using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Rooms;
using Sprites;

namespace Projectiles
{
    // Flies straight until it leaves the room
    internal sealed class Arrow : IProjectile
    {
        private const float Speed = 200f; // pixels per second

        // More than half an arrow's length, so the whole arrow has left the room before it is removed
        private const int OffscreenMargin = 40;

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position; // the middle of the arrow

        public bool IsFinished { get; private set; }
        public ProjectileOwner Owner => ProjectileOwner.Link;

        // startPosition is where the middle of the arrow starts
        public Arrow(Vector2 startPosition, Direction direction)
        {
            position = startPosition;
            velocity = direction.ToVector() * Speed;
            sprite = ProjectileSpriteFactory.Instance.CreateArrowSprite(direction);

            flightArea = RoomLayout.Bounds;
            flightArea.Inflate(OffscreenMargin, OffscreenMargin);
        }

        public void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!flightArea.Contains(position))
                IsFinished = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
