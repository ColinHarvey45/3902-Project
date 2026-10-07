using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class Wall(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateWallSprite())
    {
    }
}
