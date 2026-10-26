using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side)
    internal sealed class KeyholeLockedDoor(Direction side)
        : Block(RoomLayout.DoorPosition(side), BlockSpriteFactory.Instance.CreateKeyholeLockedDoorSprite(side), RoomLayout.DoorSize)
    {
    }
}
