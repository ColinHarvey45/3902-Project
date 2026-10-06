using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class FireBlock(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateFireSprite())
    {
    }
}
