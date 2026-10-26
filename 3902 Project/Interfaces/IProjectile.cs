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

        // False for most projectiles, which hurt the first enemy they touch and are used up (or turn back);
        // true for a bomb's explosion, which hurts every enemy in the blast
        public bool HitsEveryEnemyInRange { get; }

        // Called when it hits whoever it is allowed to hurt, or runs into a wall or a solid block
        public void Hit();

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
