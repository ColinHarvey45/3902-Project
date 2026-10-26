namespace Interfaces
{
    // An enemy that has health and can be killed (every enemy except the blade trap)
    internal interface IDamageable : IEnemy
    {

        public bool IsDamaged { get; }
        public bool IsDead { get; }

        public void TakeDamage(int damage);

    }
}
