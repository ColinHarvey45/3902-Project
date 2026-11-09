using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Rupee(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateRupeeSprite())
    {
        private const int RupeesGiven = 1;

        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.AddRupees(RupeesGiven);
        }
    }
}
