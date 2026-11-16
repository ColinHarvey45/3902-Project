using Audio;
using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Key(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateKeySprite())
    {
        private const int KeysGiven = 1;

        // A key makes the same sound as a heart, as in the NES
        public override Sound PickUpSound => Sound.GetHeart;

        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.AddKeys(KeysGiven);
        }
    }
}
