using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Shoots out fast in direction until it has gone distance (or hits a wall or a block, see BladeTrap.OnBlocked)
    internal sealed class BladeTrapChargingState(BladeTrap trap, Vector2 direction, float distance) : IEnemyState
    {
        private const float ChargeSpeed = 4f; // pixels per frame

        public void Update(GameTime gameTime)
        {
            trap.Position += direction * ChargeSpeed;

            if (Vector2.Distance(trap.Position, trap.Home) >= distance)
                trap.ChangeState(new BladeTrapReturningState(trap));
        }
    }
}
