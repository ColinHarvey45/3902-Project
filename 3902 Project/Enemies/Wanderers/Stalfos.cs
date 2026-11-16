using Microsoft.Xna.Framework;
using Sprites;
using Enemies;

namespace Enemies.Wanderers
{
    internal sealed class Stalfos : WanderingEnemy
    {
        // Two sword hits, as in the NES
        private const int StalfosHealth = 2;

        public Stalfos(Vector2 startPosition)
            : base(startPosition)
        {
            Health = new EnemyHealth(StalfosHealth);
            Sprite = EnemySpriteFactory.Instance.CreateStalfosSprite();
        }
    }
}
