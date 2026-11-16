using Interfaces;
using Player;
using Rooms;
using Sprites;

namespace Environment
{
    // The shutter door, which sits in the gap in one of the room's walls (side). No key opens it: it stays shut
    // until the room opens it, and then looks like an open door and Link can walk through
    internal sealed class DiamondLockedDoor(Direction side)
        : Block(RoomLayout.DoorPosition(side), BlockSpriteFactory.Instance.CreateDiamondLockedDoorSprite(side), RoomLayout.DoorSize),
        IShutterDoor
    {
        public bool IsOpen { get; private set; }

        public override bool IsSolid => !IsOpen;

        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            Sprite = BlockSpriteFactory.Instance.CreateOpenDoorSprite(side);
        }
    }
}
