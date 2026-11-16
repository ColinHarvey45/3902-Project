using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // One of the things the game can be doing, like playing, being paused or showing GAME OVER. Each state has
    // its own keys, so only the ones that make sense right now do anything
    internal interface IGameState
    {

        // Called each time the game switches to this state
        public void Enter();

        public void Update(GameTime gameTime);

        // Into a batch that has already been begun
        public void Draw(SpriteBatch spriteBatch);

    }
}
