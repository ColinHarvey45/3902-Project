using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Clock(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateClockSprite())
    {
    }
}
