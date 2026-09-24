using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animation
{
    internal class AnimatedSprite : Sprite
    {
        private int currentFrame;
        private int totalFrames;
        private double frameTimer;
        private double frameInterval;

        public AnimatedSprite(Texture2D texture, SpriteBatch passedSpriteBatch, Vector2 position, Point rectLocation, int numFrames, float animSpeed) : base(texture, passedSpriteBatch, position, rectLocation)
        {

            // The larger the interval the slower the speed of the animation
            frameInterval = animSpeed;
            totalFrames = numFrames;
            currentFrame = 0;
            frameTimer = 0;

        }

        public void UpdateAnimation(GameTime gameTime, Vector2 movement)
        {

            spritePosition += movement;

            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (movement == Vector2.Zero)
            {
                sourceRect = new Rectangle(location.X, location.Y, rectSize.X, rectSize.Y);
                currentFrame = 0;
            }
            else if (frameTimer >= frameInterval)
            {
                currentFrame++;
                if (currentFrame >= totalFrames)
                {
                    currentFrame = 0;
                }

                frameTimer -= frameInterval;

                int column = currentFrame % totalFrames;
                int row = currentFrame / totalFrames;

                int newX = location.X + (column * rectSize.X);
                int newY = location.Y + (row * rectSize.Y);

                sourceRect = new Rectangle(newX, newY, rectSize.X, rectSize.Y);

            }

            //spriteColor = Color.Red;

        }

    }
}
