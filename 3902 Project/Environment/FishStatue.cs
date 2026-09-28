using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environment
{
    internal class FishStatue : Block
    {

        private static readonly Point fishStatueSourceRect = new(1018, 11);
        private static readonly Point fishStatueSpriteSize = new(16, 16);

        public FishStatue(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, fishStatueSourceRect)
        {
            this.blockSprite.SetSpriteSize(fishStatueSpriteSize);
        }

    }
}
