using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class Bow(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBowSprite())
    {
    }
}
