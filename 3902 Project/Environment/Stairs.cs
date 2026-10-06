using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class Stairs(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateStairsSprite())
    {
    }
}
