using Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Enemies.States
{
    // In the real game the trap charges when Link lines up with it; until collisions
    // arrive in Sprint 3 it charges on a timer in a random direction instead
    internal sealed class BladeTrapWaitingState(BladeTrap trap) : IEnemyState
    {
        private const float WaitTime = 1.5f;

        private static readonly Random RandomGenerator = new();
        private static readonly Vector2[] ChargeDirections = [new(1, 0), new(-1, 0), new(0, -1), new(0, 1)];

        private float timer;

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= WaitTime)
            {
                Vector2 direction = ChargeDirections[RandomGenerator.Next(ChargeDirections.Length)];
                trap.ChangeState(new BladeTrapChargingState(trap, direction));
            }
        }
    }
}
