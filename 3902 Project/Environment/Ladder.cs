using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class Ladder(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateLadderSprite())
    {
    }
}
