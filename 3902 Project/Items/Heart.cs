using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Heart(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateHeartSprite())
    {
    }
}
