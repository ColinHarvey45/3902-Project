using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class SandTile(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateSandTileSprite())
    {
    }
}
