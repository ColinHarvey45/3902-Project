using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal class TriforcePiece(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateTriforcePieceSprite())
    {
    }
}
