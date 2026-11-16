using Audio;
using Interfaces;
using Projectiles;
using Rooms;

namespace Collisions
{
    // Link picking things up, and opening doors: unlocking them with his keys, and blowing holes in cracked walls
    // with his bombs
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

        // A bomb going off against a cracked wall blows a hole in it, with the secret sound, as in the NES
        public static void BlastsOpenWalls(Room room, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (!projectile.IsBlast || projectile.Collider == null)
                    continue;

                foreach (IBlock block in room.Blocks)
                {
                    if (block is IBombableWall { IsOpen: false } wall && projectile.Collider.Intersects(wall.Collider))
                    {
                        wall.BlowOpen();
                        AudioManager.Instance.Play(Sound.Secret);
                    }
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
