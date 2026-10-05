// BombedWallOpening.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class BombedWallOpening : Block
    {
        private static readonly Point bombedWallSourceRect = new(947, 11);
        private static readonly Point bombedWallSpriteSize = new(32, 31);

        public BombedWallOpening(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, bombedWallSourceRect)
        {
            this.blockSprite.SetSpriteSize(bombedWallSpriteSize);
        }
    }
}