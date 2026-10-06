using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class FishStatue(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateFishStatueSprite())
    {
    }
}
