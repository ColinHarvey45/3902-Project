// DiamondLockedDoor.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class DiamondLockedDoor : Block
    {
        private static readonly Point diamondDoorSourceRect = new(914, 11);
        private static readonly Point diamondDoorSpriteSize = new(32, 31);

        public DiamondLockedDoor(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, diamondDoorSourceRect)
        {
            this.blockSprite.SetSpriteSize(diamondDoorSpriteSize);
        }
    }
}