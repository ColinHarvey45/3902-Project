using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    // Goriya stands still until its boomerang is gone, then goes back to walking
    internal sealed class GoriyaThrowingState(Goriya goriya) : IEnemyState
    {

        public void Update(GameTime gameTime)
        {
            if (!goriya.IsBoomerangOut)
                goriya.ChangeState(new GoriyaWalkingState(goriya));
        }

    }
}
