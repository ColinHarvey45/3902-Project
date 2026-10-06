using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Draws one or more frames from a sprite sheet, moving to the next frame every frameTime seconds
    internal class Sprite : ISprite
    {
        // Every sprite sheet pixel is drawn as a 4x4 block on screen
        public const float Scale = 4f;

        private readonly Texture2D texture;
        private readonly SpriteFrame[] frames;
        private readonly float frameTime;
        private readonly bool loops;
        private int currentFrame = 0;
        private float frameTimer = 0f;

        public bool IsFinished { get; private set; } = false;

        // A sprite that never changes
        public Sprite(Texture2D texture, SpriteFrame frame)
            : this(texture, [frame], 0f, loops: true)
        {
        }

        public Sprite(Texture2D texture, SpriteFrame[] frames, float frameTime, bool loops)
        {
            this.texture = texture;
            this.frames = frames;
            this.frameTime = frameTime;
            this.loops = loops;
        }

        public void Update(GameTime gameTime)
        {
            if (frames.Length == 1 || IsFinished) return;

            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer < frameTime) return;

            frameTimer -= frameTime;

            if (currentFrame < frames.Length - 1)
                currentFrame++;
            else if (loops)
                currentFrame = 0;
            else
                IsFinished = true;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint)
        {
            SpriteFrame frame = frames[currentFrame];
            spriteBatch.Draw(texture, position + frame.Offset * Scale, frame.Source, tint, 0f, Vector2.Zero, Scale, frame.Effects, 0f);
        }
    }
}
