using Interfaces;
using Player;
using Sprites;

namespace Environment
{
    // Sits in the gap in one of the room's walls (side). Link opens it by walking into it with a key, which uses
    // the key up; once open it looks like an open door and he can walk through
    internal sealed class KeyholeLockedDoor(Direction side)
        : Door(side, BlockSpriteFactory.Instance.CreateKeyholeLockedDoorSprites(side)), ILockedDoor
    {
        public bool IsLocked { get; private set; } = true;

        public override bool IsSolid => IsLocked;

        public void Unlock()
        {
            if (!IsLocked)
                return;

            IsLocked = false;
            ChangeLook(BlockSpriteFactory.Instance.CreateOpenDoorSprites(Side));
        }
    }
}
