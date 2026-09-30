using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Animation
{
    internal class SwordSwing : Sprite
    {
        private readonly Rectangle[] frameRects;
        private readonly Vector2[] frameOffsets;
        private int currentFrame;
        private double frameTimer;
        private readonly double frameInterval;

        public bool IsPlaying { get; private set; }

        public SwordSwing(Texture2D texture, SpriteBatch spriteBatch, Vector2 position, Rectangle[] frameRects, Vector2[] frameOffsets, float frameSpeed)
            : base(texture, spriteBatch, position, new Point(frameRects[0].X, frameRects[0].Y))
        {
            this.frameRects = frameRects;
            this.frameOffsets = frameOffsets;
            frameInterval = frameSpeed;
            sourceRect = frameRects[0];
        }

        public void Play()
        {
            currentFrame = 0;
            frameTimer = 0;
            IsPlaying = true;
            sourceRect = frameRects[0];
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;

            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer >= frameInterval)
            {
                frameTimer -= frameInterval;
                currentFrame++;

                if (currentFrame >= frameRects.Length)
                {
                    IsPlaying = false;
                    currentFrame = frameRects.Length - 1;
                }

                sourceRect = frameRects[currentFrame];
            }
        }

        public void Draw(Texture2D texture)
        {
            base.Draw(texture, frameOffsets[currentFrame]);
        }
    }
}