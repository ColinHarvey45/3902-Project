using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    internal interface IEnemy
    {

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
