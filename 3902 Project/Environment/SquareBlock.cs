using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class SquareBlock : Block
    {

        private static readonly Point squareBlockSourceRect = new(1001, 11);
        private static readonly Point squareBlockSpriteSize = new(16, 16);

        public SquareBlock(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, squareBlockSourceRect)
        {
            this.blockSprite.SetSpriteSize(squareBlockSpriteSize);
        }

    }
}
