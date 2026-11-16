using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Rooms;

namespace Collisions
{
    // Fighting: Link's weapons hitting enemies (and setting off the Old Man's fires), and enemies, their projectiles
    // and bomb blasts hurting Link
    internal static class CombatCollisions
    {
        // Link's weapons: an arrow or boomerang hits the first enemy it touches and is used up (or turns back); the
        // sword and a bomb's explosion hit every enemy they touch. Each decides what its hit does (see IProjectile)
        public static void LinksWeaponsHitEnemies(Room room, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.Owner != ProjectileOwner.Link || projectile.IsFinished || projectile.Collider == null)
                    continue;

                foreach (IDamageable enemy in HurtableEnemiesTouching(projectile.Collider, room))
                {
                    projectile.HitEnemy(enemy);

                    if (!projectile.IsBlast)
                    {
                        projectile.Hit();
                        break;
                    }
                }
            }
        }

        // Attacking the Old Man (with any weapon) sets off the fires beside him, as in the NES
        public static void LinksWeaponsProvokeGuards(Room room, ProjectileManager projectiles)
        {
            bool attacked = projectiles.ActiveProjectiles.Any(weapon => weapon.Owner == ProjectileOwner.Link
                && !weapon.IsFinished && weapon.Collider != null && room.Npcs.Any(npc => weapon.Collider.Intersects(npc.Collider)));
            if (!attacked)
                return;

            foreach (IBlock block in room.Blocks)
            {
                if (block is INpcGuard guard)
                    guard.Provoke();
            }
        }

        // Touching an enemy hurts Link (his health state ignores it while he is flashing from a hit), except a
        // Wallmaster, which catches him instead
        public static void EnemiesHurtLink(IPlayer link, Room room)
        {
            foreach (IEnemy enemy in room.Enemies)
            {
                if (enemy.Collider == null || !link.Collider.Intersects(enemy.Collider))
                    continue;

                if (enemy is ILinkGrabber grabber)
                    grabber.Grab(link);
                else
                    link.TakeDamage();
            }
        }

        // An enemy's fireball or boomerang hurts Link, and is used up (or turns back)
        public static void EnemyProjectilesHitLink(IPlayer link, ProjectileManager projectiles)
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

        // Link's own bomb hurts him too if he is caught in the blast, so he has to get clear of it
        public static void BlastsHurtLink(IPlayer link, ProjectileManager projectiles)
        {
            foreach (IProjectile projectile in projectiles.ActiveProjectiles)
            {
                if (projectile.IsBlast && projectile.Collider != null && link.Collider.Intersects(projectile.Collider))
                    link.TakeDamage();
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
    }
}
