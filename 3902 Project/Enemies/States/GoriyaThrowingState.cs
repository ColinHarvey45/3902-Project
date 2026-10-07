using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Goriya stands still until its boomerang comes back, which puts it back in the walking state
    internal sealed class GoriyaThrowingState(Goriya goriya) : IEnemyState
    {

        public void Update(GameTime gameTime)
        {
            goriya.UpdateBoomerang(gameTime);
        }

    }
}
