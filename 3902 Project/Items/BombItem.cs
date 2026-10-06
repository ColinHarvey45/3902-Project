using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class BombItem(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBombSprite())
    {
    }
}
