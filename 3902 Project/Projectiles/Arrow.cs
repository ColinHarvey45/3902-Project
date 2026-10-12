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
        private const int OffscreenMargin = 20; // lets the arrow fully leave the room before it is removed

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position;

        public bool IsFinished { get; private set; }

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
