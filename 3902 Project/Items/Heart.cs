using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class Heart(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateHeartSprite())
    {
        private const int HeartsRestored = 1;

        public override void OnPickedUp(IPlayer player)
        {
            player.Heal(HeartsRestored);
        }
    }
}
