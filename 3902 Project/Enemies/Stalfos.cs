using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal sealed class Stalfos : Enemy
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
