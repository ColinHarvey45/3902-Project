using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Sits still without flapping, then takes off again
    internal sealed class KeeseRestingState(Keese keese) : IEnemyState
    {
        private const float RestTime = 1f;

        private float timer;

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= RestTime)
                keese.ChangeState(new KeeseFlyingState(keese));
        }
    }
}
