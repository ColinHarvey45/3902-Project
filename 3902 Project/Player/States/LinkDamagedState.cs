using Interfaces;
using Microsoft.Xna.Framework;

namespace Player.States
{
    // Link flashes red for a moment after being hit and can't be hurt again until it stops
    internal sealed class LinkDamagedState(Link link) : ILinkHealthState
    {
        private const float DamageDuration = 1f;
        private const float FlashInterval = 0.1f;

        private float timeLeft = DamageDuration;

        public Color Tint => (int)(timeLeft / FlashInterval) % 2 == 1 ? Color.Red : Color.White;

        public void TakeDamage() { }

        public void Update(GameTime gameTime)
        {
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeLeft <= 0)
                link.HealthState = new LinkHealthyState(link);
        }

    }
}
