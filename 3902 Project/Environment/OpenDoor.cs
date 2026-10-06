using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class OpenDoor(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateOpenDoorSprite())
    {
    }
}
