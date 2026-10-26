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

        // The hitbox is the arrow itself (16 x 5 on the sheet), lying the way it flies
        private const int ColliderLength = (int)(16 * Sprite.Scale);
        private const int ColliderThickness = (int)(5 * Sprite.Scale);

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

            bool flyingUpOrDown = direction is Direction.Up or Direction.Down;
            Point size = flyingUpOrDown ? new Point(ColliderThickness, ColliderLength) : new Point(ColliderLength, ColliderThickness);

            // position is the arrow's middle, so the box starts half its size up and left of it
            collider = new BoxCollider(new Rectangle(Point.Zero, size), new Point(-size.X / 2, -size.Y / 2));
            collider.Update(position);
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