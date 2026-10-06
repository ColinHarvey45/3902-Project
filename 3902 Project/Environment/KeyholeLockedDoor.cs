using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class KeyholeLockedDoor(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateKeyholeLockedDoorSprite())
    {
    }
}
