using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class BoomerangItem(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBoomerangSprite())
    {
    }
}
