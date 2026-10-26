using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Projectiles
{

    // Every arrow, boomerang, bomb and fireball in the room Link is in, whoever fired it. Keeping
    // them all in one place, instead of with whoever fired them, lets the game check any of them for hits
    internal sealed class ProjectileManager
    {
        private readonly List<IProjectile> projectiles = new();
        public IReadOnlyList<IProjectile> ActiveProjectiles => projectiles;

        public void Add(IProjectile projectile)
        {
            projectiles.Add(projectile);
        }

        // False once the projectile has finished (e.g. a boomerang was caught) or been cleared away
        public bool Contains(IProjectile projectile)
        {
            return projectiles.Contains(projectile);
        }

        // Moves every projectile, then checks it for hits. What a projectile can hurt depends on who
        // fired it: Link's only hurt enemies, and an enemy's only hurt Link
        public void Update(GameTime gameTime, IEnumerable<ICharacter> characters, IPlayer link)
        {
            foreach (IProjectile projectile in projectiles)
            {
                projectile.Update(gameTime);

                // Nothing to hit with (a bomb has no collider yet) or already used up this frame
                if (projectile.IsFinished || projectile.Collider == null)
                    continue;

                bool hitSomething = projectile.Owner switch
                {
                    ProjectileOwner.Link => HitsEnemy(projectile, characters),
                    ProjectileOwner.Enemy => HitsLink(projectile, link),
                    _ => false
                };

                if (hitSomething)
                    projectile.Hit();
            }

            projectiles.RemoveAll(projectile => projectile.IsFinished);
        }

        // Damages the first enemy the projectile is touching; true if it hit one. Enemies that are
        // already hurt or dead are skipped, and so are NPCs and blade traps, who can't be hurt
        private static bool HitsEnemy(IProjectile projectile, IEnumerable<ICharacter> characters)
        {
            foreach (ICharacter character in characters)
            {
                if (character is not IDamageable enemy || enemy.IsDead || enemy.IsInvincible || enemy.Collider == null)
                    continue;

                if (projectile.Collider.Intersects(enemy.Collider))
                {
                    enemy.TakeDamage(projectile.Damage);
                    return true;
                }
            }

            return false;
        }

        // Damages Link (and knocks him back) if the projectile is touching him; true if it hit
        private static bool HitsLink(IProjectile projectile, IPlayer link)
        {
            if (link?.Collider == null || !link.Collider.Intersects(projectile.Collider))
                return false;

            link.TakeDamage();
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IProjectile projectile in projectiles)
                projectile.Draw(spriteBatch);
        }

        public void Clear()
        {
            projectiles.Clear();
        }
    }
}