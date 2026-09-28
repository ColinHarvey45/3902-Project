using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class FireBlock : Block
    {

        private static readonly Point fireSourceRect = new(52, 11);
        private static readonly Point fireSpriteSize = new(18, 18);
        private float flipTimer = 0f;
        private SpriteEffects currentEffect = SpriteEffects.None;

        public FireBlock(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, fireSourceRect)
        {
            this.blockSprite.SetSpriteSize(fireSpriteSize);
        }


        public override void Update(GameTime gameTime)
        {

            if (isVisible)
            {

                if (blockSprite != null)
                {
                    Flip(gameTime);
                }

            }
        }

        public void Flip(GameTime gameTime)
        {
            flipTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flipTimer >= 0.15f)
            {
                flipTimer = 0f;

                if (currentEffect == SpriteEffects.None)
                {
                    currentEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    currentEffect = SpriteEffects.None;
                }

                this.blockSprite.SetEffects(currentEffect);
            }
        }

    }
}
