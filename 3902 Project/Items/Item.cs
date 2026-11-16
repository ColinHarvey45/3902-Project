using Audio;
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

        public void CenterOn(Vector2 middle)
        {
            Position = middle - new Vector2(HitboxWidth / 2f, HitboxHeight / 2f);
            Collider.Update(Position);
        }

        // Most items do nothing for Link yet; the ones that do override this
        public virtual void OnPickedUp(IPlayer player) { }

        // The NES's item sound, unless the item has its own
        public virtual Sound PickUpSound => Sound.GetItem;

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
