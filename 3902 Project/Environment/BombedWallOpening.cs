using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class BombedWallOpening(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateBombedWallOpeningSprite())
    {
    }
}
