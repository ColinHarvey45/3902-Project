using Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Enemies.States
{
    // Sits still for a random moment, then hops again
    internal sealed class HopperRestingState : IEnemyState
    {
        private static readonly Random RandomGenerator = new();

        private readonly HoppingEnemy enemy;
        private readonly float restTime;
        private float timer;

        public HopperRestingState(HoppingEnemy enemy)
        {
            this.enemy = enemy;
            restTime = enemy.MaxRestTime * (0.5f + 0.5f * (float)RandomGenerator.NextDouble());
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= restTime)
                enemy.ChangeState(new HopperHoppingState(enemy));
        }
    }
}
