using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Items
{
    // An item lying in the room; most stay put, but their sprite may still animate (e.g. a flashing rupee)
    internal abstract class Item(Vector2 position, ISprite sprite) : IItem
    {

        // Most items are 8 x 16 on the sheet, so that is the size of every item's hitbox
        private const int HitboxWidth = (int)(8 * Sprite.Scale);
        private const int HitboxHeight = (int)(16 * Sprite.Scale);

        protected Vector2 Position { get; set; } = position;

        public ICollider Collider { get; } =
            new BoxCollider(new Rectangle((int)position.X, (int)position.Y, HitboxWidth, HitboxHeight));

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            Collider.Update(Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }

    }
}
