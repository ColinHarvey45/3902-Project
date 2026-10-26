using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Projectiles
{

    // Every arrow, boomerang, bomb and fireball in the room Link is in, whoever fired it. Keeping
    // them all in one place, instead of with whoever fired them, lets the game check any of them for hits
    // (see Collisions/CollisionManager.cs)
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

        public void Update(GameTime gameTime)
        {
            foreach (IProjectile projectile in projectiles)
                projectile.Update(gameTime);
        }

        // Called once a frame after collisions, which can finish projectiles (e.g. an arrow hitting a wall)
        public void RemoveFinished()
        {
            projectiles.RemoveAll(projectile => projectile.IsFinished);
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
