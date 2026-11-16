using Microsoft.Xna.Framework;

namespace Interfaces
{
    // An enemy that has health and can be killed (every enemy except the blade trap)
    internal interface IDamageable : IEnemy
    {

        public bool IsInvincible { get; }
        public bool IsDead { get; }

        // Takes damage off its health. hitFrom is where the blow came from (the middle of Link's sword, an arrow or
        // a bomb); an enemy that survives is knocked back away from there
        public void TakeDamage(int damage, Vector2 hitFrom);

        // What the boomerang does: stops most enemies in their tracks for a moment, and kills the smallest
        public void Stun();

    }
}
