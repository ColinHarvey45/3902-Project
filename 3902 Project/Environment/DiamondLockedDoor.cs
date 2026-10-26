using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side)
    internal sealed class DiamondLockedDoor(Direction side)
        : Block(RoomLayout.DoorPosition(side), BlockSpriteFactory.Instance.CreateDiamondLockedDoorSprite(side), RoomLayout.DoorSize)
    {
    }
}
