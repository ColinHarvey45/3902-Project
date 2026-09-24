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
        private float spriteRotation;
        private float layerDepth;
        private float spriteScale;
        protected SpriteEffects spriteEffects;
        private Vector2 spriteOrigin;

        
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


            //Draw(spriteTexture, sourceRect, spritePosition);

        }



        // Using this constructor for spriteBatch.Draw so that we can scale up our sprites
        public void Draw(Texture2D spriteTexture) //Rectangle? sourceRectangle, Vector2 pos
        {
            spriteBatch.Draw(spriteTexture, spritePosition, sourceRect, spriteColor, spriteRotation, spriteOrigin, spriteScale, spriteEffects, layerDepth);
        }


    }
}
