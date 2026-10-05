// KeyholeLockedDoor.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class KeyholeLockedDoor : Block
    {
        private static readonly Point keyholeDoorSourceRect = new(881, 11);
        private static readonly Point keyholeDoorSpriteSize = new(32, 31);

        public KeyholeLockedDoor(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, keyholeDoorSourceRect)
        {
            this.blockSprite.SetSpriteSize(keyholeDoorSpriteSize);
        }
    }
}