using Interfaces;
using Microsoft.Xna.Framework;

namespace Player.States
{
    internal sealed class LinkHealthyState(Link link) : ILinkHealthState
    {

        public Color Tint => Color.White;

        // A hit hurts Link and knocks him back, whatever hit him (an enemy, a fireball, a boomerang);
        // while he flashes afterwards (LinkDamagedState) further hits do nothing
        public void TakeDamage()
        {
            link.HealthState = new LinkDamagedState(link);
            link.KnockBack();
        }

        public void Update(GameTime gameTime) { }

    }
}
