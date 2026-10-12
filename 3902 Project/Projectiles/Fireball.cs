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

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position;

        public bool IsFinished { get; private set; }
        public ProjectileOwner Owner => ProjectileOwner.Enemy;

        // velocity is in pixels per second
        public Fireball(Vector2 startPosition, Vector2 velocity)
        {
            position = startPosition;
            this.velocity = velocity;
            sprite = ProjectileSpriteFactory.Instance.CreateFireballSprite();

            flightArea = RoomLayout.Bounds;
            flightArea.Inflate(OffscreenMargin, OffscreenMargin);
        }

        public void Update(GameTime gameTime)
        {
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            sprite.Update(gameTime);

            if (!flightArea.Contains(position))
                IsFinished = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
