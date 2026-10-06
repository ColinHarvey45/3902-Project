using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class SquareBlock(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateSquareBlockSprite())
    {
    }
}
