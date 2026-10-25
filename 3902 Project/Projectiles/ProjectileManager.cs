using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectiles
{
    internal sealed class ProjectileManager
    {
        private readonly List<IProjectile> projectiles = [];

        public IReadOnlyList<IProjectile> ActiveProjectiles => projectiles;

        public void Add(IProjectile projectile)
        {
            projectiles.Add(projectile);
        }

        public bool Contains(IProjectile projectile)
        {
            return projectiles.Contains(projectile);
        }

        public void Update(GameTime gameTime)
        {
            foreach (IProjectile projectile in projectiles)
                projectile.Update(gameTime);

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
