using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Sits still without flapping, then takes off again
    internal class KeeseRestingState(Keese keese) : IEnemyState
    {
        private const float RestTime = 1f;

        private float timer = 0f;

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= RestTime)
                keese.ChangeState(new KeeseFlyingState(keese));
        }
    }
}
