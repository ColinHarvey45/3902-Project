using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Something Link can pick up (hearts, rupees, keys, ...) by touching it
    internal interface IItem
    {

        public ICollider Collider { get; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
