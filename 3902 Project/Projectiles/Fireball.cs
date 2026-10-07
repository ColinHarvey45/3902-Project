using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Projectiles
{
    // Breathed by Aquamentus; flickers through its colours and flies straight until it leaves the screen
    internal class Fireball : IProjectile
    {
        private const int OffscreenMargin = 20; // lets the fireball fully leave the screen before it is removed

        private readonly ISprite sprite;
        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;
        private Vector2 position;

        public bool IsFinished { get; private set; } = false;

        // velocity is in pixels per second
        public Fireball(Vector2 startPosition, Vector2 velocity, Rectangle screenBounds)
        {
            position = startPosition;
            this.velocity = velocity;
            sprite = ProjectileSpriteFactory.Instance.CreateFireballSprite();

            flightArea = screenBounds;
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
