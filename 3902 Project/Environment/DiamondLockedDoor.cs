using Interfaces;
using Player;
using Sprites;

namespace Environment
{
    // The shutter door, which sits in the gap in one of the room's walls (side). No key opens it: it stays shut
    // until the room opens it, and then looks like an open door and Link can walk through
    internal sealed class DiamondLockedDoor(Direction side)
        : Door(side, BlockSpriteFactory.Instance.CreateDiamondLockedDoorSprites(side)), IShutterDoor
    {
        public bool IsOpen { get; private set; }

        public override bool IsSolid => !IsOpen;

        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            ChangeLook(BlockSpriteFactory.Instance.CreateOpenDoorSprites(Side));
        }
    }
}
