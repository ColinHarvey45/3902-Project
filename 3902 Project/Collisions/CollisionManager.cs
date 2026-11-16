using Interfaces;
using Projectiles;
using Rooms;

namespace Collisions
{
    // Finds everything that is touching in the room Link is in, once a frame after everything has moved,
    // and tells the objects involved. Each object decides how it reacts: e.g. Link's health state decides
    // whether a hit hurts him, and each enemy decides what to do when it walks into a wall.
    // The collisions themselves are grouped by what they are about, one method for each kind: fighting
    // (CombatCollisions), moving about (MovementCollisions), and picking things up and opening doors
    // (PickupCollisions). This runs them in the order that matters
    internal static class CollisionManager
    {
        public static void HandleCollisions(IPlayer link, Room room, ProjectileManager projectiles)
        {
            CombatCollisions.LinksWeaponsHitEnemies(room, projectiles);
            CombatCollisions.LinksWeaponsProvokeGuards(room, projectiles);

            // Anything killed this frame is gone before it can touch Link
            room.RemoveDeadEnemies();

            CombatCollisions.EnemiesHurtLink(link, room);
            CombatCollisions.EnemyProjectilesHitLink(link, projectiles);
            CombatCollisions.BlastsHurtLink(link, projectiles);
            MovementCollisions.ProjectilesHitWallsAndBlocks(room, projectiles);

            // A door Link unlocks (or blows open) this frame no longer stops him
            PickupCollisions.LinkUnlocksDoors(link, room);
            PickupCollisions.BlastsOpenWalls(room, projectiles);
            MovementCollisions.WallsBlocksAndNpcsStopLink(link, room);
            MovementCollisions.WallsAndBlocksStopEnemies(room);
            PickupCollisions.LinkPicksUpItems(link, room);
        }
    }
}
