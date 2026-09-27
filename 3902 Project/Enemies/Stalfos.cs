using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Stalfos : Enemy
    {
        private static readonly Point StalfosSourceRect = new(1, 58);
        private static readonly Point StalfosSpriteSize = new(16, 16);
        private float flipTimer = 0f;
        private SpriteEffects currentEffect = SpriteEffects.None;

        public Stalfos(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, StalfosSourceRect)
        {
            this.enemySprite.SetSpriteSize(StalfosSpriteSize);
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

                this.enemySprite.SetEffects(currentEffect);
            }
        }
    }
}

