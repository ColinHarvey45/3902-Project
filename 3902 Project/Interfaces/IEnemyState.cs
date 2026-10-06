using Microsoft.Xna.Framework;

namespace Interfaces
{
    // What an enemy is doing right now (walking, attacking, ...); each state decides how the enemy behaves
    internal interface IEnemyState
    {

        public void Update(GameTime gameTime);

    }
}
