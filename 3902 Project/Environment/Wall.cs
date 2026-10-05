// Wall.cs  ("Walls / room border")
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class Wall : Block
    {
        private static readonly Point wallSourceRect = new(815, 11);
        private static readonly Point wallSpriteSize = new(32, 31);

        public Wall(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, wallSourceRect)
        {
            this.blockSprite.SetSpriteSize(wallSpriteSize);
        }
    }
}