using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class Ladder(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateLadderSprite())
    {
    }
}
