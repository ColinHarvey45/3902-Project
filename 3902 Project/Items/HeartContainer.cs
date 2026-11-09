using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class HeartContainer(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateHeartContainerSprite())
    {
        public override void OnPickedUp(IPlayer player)
        {
            player.IncreaseMaxHealth();
        }
    }
}
