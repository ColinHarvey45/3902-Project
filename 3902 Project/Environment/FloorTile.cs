using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class FloorTile(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateFloorTileSprite())
    {
    }
}
