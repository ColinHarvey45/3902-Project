using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Something Link can pick up (hearts, rupees, keys, ...)
    internal interface IItem
    {

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
