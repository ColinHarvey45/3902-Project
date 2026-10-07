using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class Stairs(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateStairsSprite())
    {
    }
}
