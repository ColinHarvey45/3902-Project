using Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal abstract class Block(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Point sourceRect)
    {

        protected Texture2D texture = texture;
        protected Sprite blockSprite = new Sprite(texture, spriteBatch, startPosition, sourceRect);
        protected bool isVisible = true;

        public void SetVisibility(bool visible)
        {
            this.isVisible = visible;
        }

        public virtual void Update(GameTime gameTime){ }

        public virtual void Draw()
        {

            if (isVisible)
            {
                blockSprite?.Draw(texture);
            }

        }

    }
}
