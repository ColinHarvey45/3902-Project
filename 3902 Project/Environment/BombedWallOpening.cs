using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class BombedWallOpening(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateBombedWallOpeningSprite())
    {
    }
}
