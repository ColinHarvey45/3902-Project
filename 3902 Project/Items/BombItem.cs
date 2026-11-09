using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class BombItem(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateBombSprite())
    {
        private const int BombsGiven = 4;

        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.AddBombs(BombsGiven);
        }
    }
}
