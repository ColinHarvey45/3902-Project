using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class DragonStatue : Block
    {

        private static readonly Point dragonStatueSourceRect = new(1035, 11);
        private static readonly Point dragonStatueSpriteSize = new(16, 16);

        public DragonStatue(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, dragonStatueSourceRect)
        {
            this.blockSprite.SetSpriteSize(dragonStatueSpriteSize);
        }

    }
}
