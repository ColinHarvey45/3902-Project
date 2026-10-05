// WhiteBrick.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class WhiteBrick : Block
    {
        private static readonly Point whiteBrickSourceRect = new(984, 45);
        private static readonly Point whiteBrickSpriteSize = new(16, 16);

        public WhiteBrick(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, whiteBrickSourceRect)
        {
            this.blockSprite.SetSpriteSize(whiteBrickSpriteSize);
        }
    }
}