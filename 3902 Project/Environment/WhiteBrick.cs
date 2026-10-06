using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class WhiteBrick(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateWhiteBrickSprite())
    {
    }
}
