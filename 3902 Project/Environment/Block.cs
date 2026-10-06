using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Environment
{
    // A piece of the room that stays where it was placed; its sprite may still animate (e.g. fire)
    internal abstract class Block(Vector2 position, ISprite sprite) : IBlock
    {

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

    }
}
