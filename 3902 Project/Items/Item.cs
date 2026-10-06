using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Items
{
    // An item lying in the room; most stay put, but their sprite may still animate (e.g. a flashing rupee)
    internal abstract class Item(Vector2 position, ISprite sprite) : IItem
    {

        protected Vector2 Position { get; set; } = position;

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }

    }
}
