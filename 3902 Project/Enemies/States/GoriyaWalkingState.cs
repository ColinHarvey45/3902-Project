using Interfaces;
using Microsoft.Xna.Framework;

namespace Enemies.States
{
    internal sealed class GoriyaWalkingState(Goriya goriya) : IEnemyState
    {

        public void Update(GameTime gameTime)
        {
            goriya.Walk(gameTime);
        }

    }
}
