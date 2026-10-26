using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Rooms;

namespace Collisions
{
    // Finds everything that is touching in the room Link is in, once a frame after everything has moved,
    // and tells the objects involved. Each object decides how it reacts: e.g. Link's health state decides
    // whether a hit hurts him, and each enemy decides what to do when it walks into a wall.
    // There is one method below for each kind of collision
    internal static class CollisionManager
    {
        public static void HandleCollisions(IPlayer link, Room room, ProjectileManager projectiles)
        {
            SwordHitsEnemies(link, room);
            LinksProjectilesHitEnemies(room, projectiles);

            // Anything killed this frame is gone before it can touch Link
            room.RemoveDeadEnemies();

            EnemiesHurtLink(link, room);
            EnemyProjectilesHitLink(link, projectiles);
            ProjectilesHitWallsAndBlocks(room, projectiles);
            WallsBlocksAndNpcsStopLink(link, room);
            WallsAndBlocksStopEnemies(room);
            LinkPicksUpItems(link, room);
        }

        // Link's sword hurts every enemy it touches
        private static void SwordHitsEnemies(IPlayer link, Room room)
        {
            if (link.SwordCollider is not ICollider sword)
                return;

            foreach (IDamageable enemy in HurtableEnemiesTouching(sword, room))
                enemy.TakeDamage(link.SwordDamage);
        }

        // An arrow or boomerang hurts the first enemy it touches and is used up (or turns back);
        // a bomb's explosion hurts every enemy in the blast
        private static void LinksProjectilesHitEnemies(Room room, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.Owner != ProjectileOwner.Link || projectile.IsFinished || projectile.Collider == null)
                    continue;

                foreach (IDamageable enemy in HurtableEnemiesTouching(projectile.Collider, room))
                {
                    enemy.TakeDamage(projectile.Damage);

                    if (!projectile.HitsEveryEnemyInRange)
                    {
                        projectile.Hit();
                        break;
                    }
                }
            }
        }

        // Touching an enemy hurts Link (his health state ignores it while he is flashing from a hit)
        private static void EnemiesHurtLink(IPlayer link, Room room)
        {
            foreach (IEnemy enemy in room.Enemies)
            {
                if (enemy.Collider != null && link.Collider.Intersects(enemy.Collider))
                    link.TakeDamage();
            }
        }

        // An enemy's fireball or boomerang hurts Link, and is used up (or turns back)
        private static void EnemyProjectilesHitLink(IPlayer link, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.Owner == ProjectileOwner.Enemy && !projectile.IsFinished && projectile.Collider != null
                    && link.Collider.Intersects(projectile.Collider))
                {
                    link.TakeDamage();
                    projectile.Hit();
                }
            }
        }

        // Arrows and fireballs end at walls and solid blocks, and boomerangs turn back
        private static void ProjectilesHitWallsAndBlocks(Room room, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.IsFinished || projectile.Collider == null)
                    continue;

                Rectangle box = projectile.Collider.GetCollider();
                if (Obstacles(room).Any(obstacle => obstacle.Intersects(box)))
                    projectile.Hit();
            }
        }

        // Link can't walk into a wall, a solid block (closed doors included) or an NPC: he is pushed straight back out
        private static void WallsBlocksAndNpcsStopLink(IPlayer link, Room room)
        {
            IEnumerable<Rectangle> npcs = room.Npcs.Select(npc => npc.Collider.GetCollider());

            foreach (Rectangle obstacle in Obstacles(room).Concat(npcs))
            {
                Vector2 push = PushOut(link.Collider.GetCollider(), obstacle);
                if (push != Vector2.Zero)
                    link.Push(push);
            }
        }

        // Enemies can't walk into walls or solid blocks either; each one decides what to do instead
        private static void WallsAndBlocksStopEnemies(Room room)
        {
            foreach (IEnemy enemy in room.Enemies)
            {
                if (enemy.Collider == null)
                    continue;

                foreach (Rectangle obstacle in Obstacles(room))
                {
                    Vector2 push = PushOut(enemy.Collider.GetCollider(), obstacle);
                    if (push != Vector2.Zero)
                        enemy.OnBlocked(push);
                }
            }
        }

        // Link picks up any item he touches, and it disappears from the room
        private static void LinkPicksUpItems(IPlayer link, Room room)
        {
            for (int i = room.Items.Count - 1; i >= 0; i--)
            {
                IItem item = room.Items[i];
                if (link.Collider.Intersects(item.Collider))
                    room.RemoveItem(item);
            }
        }

        // Enemies that can be hurt right now (not dead, and not still flashing from a hit) and are touching hitbox.
        // Blade Traps can't be hurt at all
        private static IEnumerable<IDamageable> HurtableEnemiesTouching(ICollider hitbox, Room room)
        {
            foreach (IEnemy enemy in room.Enemies)
            {
                if (enemy is IDamageable damageable && !damageable.IsDead && !damageable.IsInvincible
                    && enemy.Collider != null && hitbox.Intersects(enemy.Collider))
                    yield return damageable;
            }
        }

        // The room's walls (and the strips just outside it), and every solid block, closed doors included
        private static IEnumerable<Rectangle> Obstacles(Room room)
        {
            foreach (Rectangle wall in room.Walls)
                yield return wall;

            foreach (IBlock block in room.Blocks)
            {
                if (block.IsSolid)
                    yield return block.Collider.GetCollider();
            }
        }

        // How far to move a box so it no longer overlaps an obstacle: straight out across whichever side
        // overlaps the least, as in Caden's original block collision. Zero if they don't overlap
        private static Vector2 PushOut(Rectangle box, Rectangle obstacle)
        {
            Rectangle overlap = Rectangle.Intersect(box, obstacle);
            if (overlap.IsEmpty)
                return Vector2.Zero;

            if (overlap.Width < overlap.Height)
                return new Vector2(box.Center.X < obstacle.Center.X ? -overlap.Width : overlap.Width, 0);

            return new Vector2(0, box.Center.Y < obstacle.Center.Y ? -overlap.Height : overlap.Height);
        }
    }
}
