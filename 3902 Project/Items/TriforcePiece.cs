using Microsoft.Xna.Framework;
using Sprites;

namespace Items
{
    internal sealed class TriforcePiece(Vector2 position)
        : Item(position, ItemSpriteFactory.Instance.CreateTriforcePieceSprite())
    {
    }
}
