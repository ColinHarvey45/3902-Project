using Audio;
using Interfaces;
using Rooms;

namespace Collisions
{
    // Link picking things up, and unlocking doors with his keys
    internal static class PickupCollisions
    {
        // Walking into a locked door with a key opens it and uses the key up; without one it is just a closed door
        public static void LinkUnlocksDoors(IPlayer link, Room room)
        {
            foreach (IBlock block in room.Blocks)
            {
                if (block is ILockedDoor { IsLocked: true } door && link.Collider.Intersects(door.Collider)
                    && link.Inventory.TryUseKey())
                {
                    door.Unlock();
                    AudioManager.Instance.Play(Sound.DoorUnlock);
                }
            }
        }

        // Link picks up any item he touches: it does what it does for him, and disappears from the room
        public static void LinkPicksUpItems(IPlayer link, Room room)
        {
            for (int i = room.Items.Count - 1; i >= 0; i--)
            {
                IItem item = room.Items[i];
                if (link.Collider.Intersects(item.Collider))
                {
                    item.OnPickedUp(link);
                    AudioManager.Instance.Play(item.PickUpSound);
                    room.RemoveItem(item);
                }
            }
        }
    }
}
