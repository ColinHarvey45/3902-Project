using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class Stairs : Block
    {

        private static readonly Point stairsSourceRect = new(1035, 28);
        private static readonly Point stairsSpriteSize = new(16, 16);

        public Stairs(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, stairsSourceRect)
        {
            this.blockSprite.SetSpriteSize(stairsSpriteSize);
        }

    }
}
