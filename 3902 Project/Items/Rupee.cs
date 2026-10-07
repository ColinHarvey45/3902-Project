using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Rupee(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateRupeeSprite())
    {
    }
}
