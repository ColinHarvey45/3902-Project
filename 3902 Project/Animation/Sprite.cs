using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Animation
{
    internal class Sprite : ISprite
    {

        // constants
        protected Texture2D spriteTexture;
        protected Color spriteColor;
        protected Point rectSize;
        public float spriteRotation { get; set; }
        private float layerDepth;
        private float spriteScale;
        public SpriteEffects spriteEffects { get; set; }
        private Vector2 spriteOrigin;
        private float flipTimer = 0f;
        private float maxTime = 0.15f;


        private SpriteBatch spriteBatch;
        protected Vector2 spritePosition;
        protected Rectangle? sourceRect;
        protected Point location;

        public Sprite(Texture2D texture, SpriteBatch passedSpriteBatch, Vector2 position, Point rectLocation)
        {

            spriteOrigin = Vector2.Zero;
            spriteColor = Color.White;
            spriteEffects = SpriteEffects.None;
            rectSize = new Point(16, 16);
            layerDepth = 0f;
            spriteScale = 4f;
            spriteRotation = 0f;

            spriteTexture = texture;
            spriteBatch = passedSpriteBatch;

            spritePosition = position;
            location = rectLocation;

            sourceRect = new Rectangle(location, rectSize);

        }

        public void Flip(GameTime gameTime)
        {
            flipTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flipTimer >= maxTime)
            {
                flipTimer = 0f;

                if (spriteEffects == SpriteEffects.None)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }

                //this.SetEffects(spriteEffects);
            }
        }

        public Vector2 Position
        {
            get => spritePosition;
            set => spritePosition = value;
        }

        public void SetEffects(SpriteEffects effects)
        {
            spriteEffects = effects;
        }

        public void SetSpriteSize(Point spriteSize)
        {
            rectSize = spriteSize;
        }

        // Using this constructor for spriteBatch.Draw so that we can scale up our sprites
        public void Draw(Texture2D spriteTexture)
        {
            spriteBatch.Draw(spriteTexture, spritePosition, sourceRect, spriteColor, spriteRotation, spriteOrigin, spriteScale, spriteEffects, layerDepth);
        }
        
        public void Draw(Texture2D spriteTexture, Vector2 drawOffset)
        {
         spriteBatch.Draw(spriteTexture, spritePosition + drawOffset * spriteScale, sourceRect, spriteColor, spriteRotation, spriteOrigin, spriteScale, spriteEffects, layerDepth);
        }


    }
}
