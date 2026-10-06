using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class BoomerangItem(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBoomerangSprite())
    {
    }
}
