using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class HeartContainer(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateHeartContainerSprite())
    {
    }
}
