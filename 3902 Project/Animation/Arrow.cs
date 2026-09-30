using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    internal class Arrow : Sprite
    {
        private static readonly Rectangle VerticalFrame = new Rectangle(3, 175, 5, 16);   // points up
        private static readonly Rectangle HorizontalFrame = new Rectangle(10, 180, 16, 5); // points right

        private const float Speed = 200f; // pixels per second - tune to taste

        private readonly Vector2 velocity;

        public bool Active { get; private set; } = true;

        public Arrow(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Vector2 direction)
            : base(texture, spriteBatch, startPosition, new Point(3, 175))
        {
            velocity = direction * Speed;

            bool vertical = direction.Y != 0;
            sourceRect = vertical ? VerticalFrame : HorizontalFrame;

            if (vertical)
                SetEffects(direction.Y < 0 ? SpriteEffects.None : SpriteEffects.FlipVertically); // up = as-drawn, down = flip
            else
                SetEffects(direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally); // right = as-drawn, left = flip
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            if (!Active) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += velocity * dt;

            if (Position.X < -20 || Position.Y < -20 || Position.X > screenWidth + 20 || Position.Y > screenHeight + 20)
                Active = false;
        }

        public void Draw(Texture2D texture)
        {
            if (Active)
                base.Draw(texture);
        }
    }
}