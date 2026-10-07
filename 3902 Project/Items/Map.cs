using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Map(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateMapSprite())
    {
    }
}
