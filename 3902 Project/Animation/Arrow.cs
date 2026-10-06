using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    internal class Arrow : Sprite, IProjectile
    {
        private static readonly Rectangle VerticalFrame = new Rectangle(3, 175, 5, 16);   // points up
        private static readonly Rectangle HorizontalFrame = new Rectangle(10, 180, 16, 5); // points right

        private const float Speed = 200f; // pixels per second - tune to taste
        private const int OffscreenMargin = 20; // lets the arrow fully leave the screen before it is removed

        private readonly Vector2 velocity;
        private readonly Rectangle flightArea;

        public bool IsFinished { get; private set; } = false;

        public Arrow(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Vector2 direction, Rectangle screenBounds)
            : base(texture, spriteBatch, startPosition, new Point(3, 175))
        {
            velocity = direction * Speed;

            flightArea = screenBounds;
            flightArea.Inflate(OffscreenMargin, OffscreenMargin);

            bool vertical = direction.Y != 0;
            sourceRect = vertical ? VerticalFrame : HorizontalFrame;

            if (vertical)
                SetEffects(direction.Y < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically); // up = as-drawn, down = flip
            else
                SetEffects(direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally); // right = as-drawn, left = flip
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += velocity * dt;

            if (!flightArea.Contains(Position))
                IsFinished = true;
        }

        public new void Draw(Texture2D texture)
        {
            if (!IsFinished)
                base.Draw(texture);
        }
    }
}
