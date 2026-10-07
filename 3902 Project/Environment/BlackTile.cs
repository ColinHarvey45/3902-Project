using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal sealed class BlackTile(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateBlackTileSprite())
    {
    }
}
