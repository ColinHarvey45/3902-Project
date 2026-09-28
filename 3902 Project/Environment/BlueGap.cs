using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class BlueGap : Block
    {

        private static readonly Point blueGapSourceRect = new(1018, 28);
        private static readonly Point blueGapSpriteSize = new(16, 16);

        public BlueGap(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, blueGapSourceRect)
        {
            this.blockSprite.SetSpriteSize(blueGapSpriteSize);
        }

    }
}
