using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Key(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateKeySprite())
    {
        private const int KeysGiven = 1;

        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.AddKeys(KeysGiven);
        }
    }
}
