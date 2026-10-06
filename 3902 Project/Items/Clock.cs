using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class Clock(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateClockSprite())
    {
    }
}
