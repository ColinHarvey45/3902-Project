using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class SandTile(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateSandTileSprite())
    {
    }
}
