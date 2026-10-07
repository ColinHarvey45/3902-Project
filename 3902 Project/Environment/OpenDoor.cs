using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class OpenDoor(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateOpenDoorSprite())
    {
    }
}
