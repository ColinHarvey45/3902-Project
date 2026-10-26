using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;

namespace Interfaces
{
    internal interface IProjectile
    {

        public bool IsFinished { get; }
        public ProjectileOwner Owner { get; }
        // How much health it takes off an enemy it hits
        public int Damage { get; }
        public ICollider Collider { get; set; }

        // Called when it hits whoever it is allowed to hurt
        public void Hit();

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
