using System;
using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    // What a dead enemy leaves behind, as in the NES: often nothing, otherwise a rupee, a heart or some bombs
    internal static class ItemDrops
    {
        // Out of every 100 enemies killed, roughly how many leave each; the rest leave nothing
        private const int RupeeChance = 25;
        private const int HeartChance = 15;
        private const int BombChance = 10;

        // Items are 8 x 16 on the sheet, so one is centred on a point by moving it half that up and left
        private static readonly Vector2 HalfItem = new(4 * Sprite.Scale, 8 * Sprite.Scale);

        private static readonly Random RandomGenerator = new();

        // The item left where an enemy died, centred on middle, or null if it leaves nothing
        public static IItem Roll(Vector2 middle)
        {
            Vector2 position = middle - HalfItem;
            int roll = RandomGenerator.Next(100);

            if (roll < RupeeChance)
                return new Rupee(position);

            if (roll < RupeeChance + HeartChance)
                return new Heart(position);

            if (roll < RupeeChance + HeartChance + BombChance)
                return new BombItem(position);

            return null;
        }
    }
}
