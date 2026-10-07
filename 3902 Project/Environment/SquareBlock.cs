using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class SquareBlock(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateSquareBlockSprite())
    {
    }
}
