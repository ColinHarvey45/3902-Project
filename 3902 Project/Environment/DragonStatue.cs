using Microsoft.Xna.Framework;
using Sprites;

namespace Environment
{
    internal class DragonStatue(Vector2 position)
        : Block(position, BlockSpriteFactory.Instance.CreateDragonStatueSprite())
    {
    }
}
