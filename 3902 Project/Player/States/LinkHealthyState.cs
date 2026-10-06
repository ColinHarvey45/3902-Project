using Interfaces;
using Microsoft.Xna.Framework;

namespace Player.States
{
    internal class LinkHealthyState(Link link) : ILinkHealthState
    {

        public Color Tint => Color.White;

        public void TakeDamage()
        {
            link.HealthState = new LinkDamagedState(link);
        }

        public void Update(GameTime gameTime) { }

    }
}
