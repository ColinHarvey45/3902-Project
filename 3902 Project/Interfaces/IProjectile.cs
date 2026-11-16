using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;

namespace Interfaces
{
    internal interface IProjectile
    {

        public bool IsFinished { get; }
        public ProjectileOwner Owner { get; }
        public ICollider Collider { get; set; }

        // True for a bomb's explosion, which hurts every enemy in the blast (and Link, if he is in it); false for
        // the rest, which hit the first enemy they touch and are used up (or turn back)
        public bool IsBlast { get; }

        // What it does to an enemy it hits: each weapon does its own damage, and the boomerang stuns instead.
        // Only called for Link's projectiles; enemies' projectiles only hurt Link
        public void HitEnemy(IDamageable enemy);

        // Called when it hits whoever it is allowed to hurt, or runs into a wall or a solid block
        public void Hit();

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
