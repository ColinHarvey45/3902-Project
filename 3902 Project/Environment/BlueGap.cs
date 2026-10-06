using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class BlueGap(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateBlueGapSprite())
    {
    }
}
