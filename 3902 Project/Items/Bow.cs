using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Bow(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBowSprite())
    {
    }
}
