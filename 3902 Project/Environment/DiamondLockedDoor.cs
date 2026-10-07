using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class DiamondLockedDoor(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateDiamondLockedDoorSprite())
    {
    }
}
