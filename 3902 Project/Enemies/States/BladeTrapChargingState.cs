using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    internal sealed class BladeTrapChargingState(BladeTrap trap, Vector2 direction) : IEnemyState
    {
        private const float ChargeSpeed = 4f; // pixels per frame
        private const float ChargeDistance = 160f; // how far it shoots out before turning back

        public void Update(GameTime gameTime)
        {
            trap.Position += direction * ChargeSpeed;

            if (Vector2.Distance(trap.Position, trap.Home) >= ChargeDistance)
                trap.ChangeState(new BladeTrapReturningState(trap));
        }
    }
}
