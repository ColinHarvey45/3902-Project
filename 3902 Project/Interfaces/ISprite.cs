using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Interfaces
{
    // Only knows how to animate and draw itself; the object using it decides where it is
    internal interface ISprite
    {

        // True once a sprite that plays only once has shown its last frame
        public bool IsFinished { get; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch, Vector2 position);
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color tint);

    }
}
