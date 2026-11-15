using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Compass(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateCompassSprite())
    {
        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.CollectCompass();
        }
    }
}
