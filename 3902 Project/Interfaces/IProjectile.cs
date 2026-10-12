using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;

namespace Interfaces
{
    internal interface IProjectile
    {

        public bool IsFinished { get; }
        public ProjectileOwner Owner { get; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
