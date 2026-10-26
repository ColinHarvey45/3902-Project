using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // Fills the gap in one of the room's walls (side) when there is no door there
    internal sealed class Wall(Direction side)
        : Block(RoomLayout.DoorPosition(side), BlockSpriteFactory.Instance.CreateWallSprite(side), RoomLayout.DoorSize)
    {
    }
}
