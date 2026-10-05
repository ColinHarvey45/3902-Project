// OpenDoor.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    internal class OpenDoor : Block
    {
        private static readonly Point openDoorSourceRect = new(848, 11);
        private static readonly Point openDoorSpriteSize = new(32, 31);

        public OpenDoor(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, openDoorSourceRect)
        {
            this.blockSprite.SetSpriteSize(openDoorSpriteSize);
        }
    }
}