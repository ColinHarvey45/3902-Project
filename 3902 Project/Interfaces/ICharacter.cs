using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Anyone besides Link who lives in a room: enemies and NPCs like the Old Man
    internal interface ICharacter
    {

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
