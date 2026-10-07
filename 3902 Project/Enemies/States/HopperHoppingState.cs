using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Hops in a random direction for a moment, then rests
    internal class HopperHoppingState : IEnemyState
    {
        private readonly HoppingEnemy enemy;
        private float timer = 0f;

        public HopperHoppingState(HoppingEnemy enemy)
        {
            this.enemy = enemy;
            enemy.StartHop();
        }

        public void Update(GameTime gameTime)
        {
            enemy.HopForward();
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= enemy.HopTime)
                enemy.ChangeState(new HopperRestingState(enemy));
        }
    }
}
