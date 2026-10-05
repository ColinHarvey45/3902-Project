
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class Ladder : Block
    {
        private static readonly Point ladderSourceRect = new(1001, 45);
        private static readonly Point ladderSpriteSize = new(16, 16);

        public Ladder(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, ladderSourceRect)
        {
            this.blockSprite.SetSpriteSize(ladderSpriteSize);
        }
    }
}