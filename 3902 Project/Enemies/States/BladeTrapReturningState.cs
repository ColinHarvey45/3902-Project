using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Slides back home much slower than it charged out
    internal class BladeTrapReturningState(BladeTrap trap) : IEnemyState
    {
        private const float ReturnSpeed = 1f; // pixels per frame

        public void Update(GameTime gameTime)
        {
            Vector2 toHome = trap.Home - trap.Position;

            if (toHome.Length() <= ReturnSpeed)
            {
                trap.Position = trap.Home;
                trap.ChangeState(new BladeTrapWaitingState(trap));
                return;
            }

            toHome.Normalize();
            trap.Position += toHome * ReturnSpeed;
        }
    }
}
