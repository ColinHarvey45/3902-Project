using Interfaces;
using Player;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side), looking like a plain wall, until a bomb blows a hole in it
    // (see PickupCollisions); from then on it is a bombed opening, as in the NES
    internal sealed class BombableWall(Direction side)
        : Door(side, BlockSpriteFactory.Instance.CreateBombableWallSprites(side)), IBombableWall
    {
        public bool IsOpen { get; private set; }

        public override bool IsSolid => !IsOpen;

        public void BlowOpen()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            ChangeLook(BlockSpriteFactory.Instance.CreateBombedWallOpeningSprites(Side));
        }
    }
}
