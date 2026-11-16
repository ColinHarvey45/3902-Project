using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    // Picking it up wins the game
    internal sealed class TriforcePiece(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateTriforcePieceSprite())
    {
        public override void OnPickedUp(IPlayer player)
        {
            player.Inventory.CollectTriforce();
        }
    }
}
