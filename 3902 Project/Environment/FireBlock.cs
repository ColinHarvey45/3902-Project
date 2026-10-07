using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class FireBlock(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateFireSprite())
    {
    }
}
