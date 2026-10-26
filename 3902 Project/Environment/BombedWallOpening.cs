using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side)
    internal sealed class BombedWallOpening(Direction side)
        : Block(RoomLayout.DoorPosition(side), BlockSpriteFactory.Instance.CreateBombedWallOpeningSprite(side))
    {
        public override bool IsSolid => false;
    }
}
