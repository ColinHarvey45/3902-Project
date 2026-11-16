using Player;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side)
    internal sealed class OpenDoor(Direction side) : Door(side, BlockSpriteFactory.Instance.CreateOpenDoorSprites(side))
    {
        public override bool IsSolid => false;
    }
}
