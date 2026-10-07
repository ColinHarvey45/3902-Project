using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Key(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateKeySprite())
    {
    }
}
