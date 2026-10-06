using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class Key(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateKeySprite())
    {
    }
}
