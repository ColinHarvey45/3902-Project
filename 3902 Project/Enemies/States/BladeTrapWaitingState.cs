using Interfaces;
using Microsoft.Xna.Framework;
using Enemies.Traps;

namespace Enemies.States
{
    // At home, watching for Link to line up with it
    internal sealed class BladeTrapWaitingState(BladeTrap trap) : IEnemyState
    {
        public void Update(GameTime gameTime)
        {
            if (trap.DirectionToLink() is Vector2 direction)
                trap.ChangeState(new BladeTrapChargingState(trap, direction, trap.ChargeDistance(direction)));
        }
    }
}
