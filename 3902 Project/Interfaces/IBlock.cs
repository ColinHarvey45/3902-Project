using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Blocks never move, but some (like fire) still animate
    internal interface IBlock
    {

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
