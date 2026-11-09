using Interfaces;
using Microsoft.Xna.Framework;

namespace Player.States
{
    internal sealed class LinkHealthyState(Link link) : ILinkHealthState
    {
        private const int HitDamage = 1; // hearts lost per hit

        public Color Tint => Color.White;

        // A hit takes a heart and knocks him back, whatever hit him (an enemy, a fireball, a boomerang);
        // while he flashes afterwards (LinkDamagedState) further hits do nothing
        public void TakeDamage()
        {
            link.LoseHealth(HitDamage);
            link.HealthState = new LinkDamagedState(link);
            link.KnockBack();
        }

        public void Update(GameTime gameTime) { }

    }
}
