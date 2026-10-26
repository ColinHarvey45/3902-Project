using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;

namespace Environment
{
    // A piece of the room that stays where it was placed; its sprite may still animate (e.g. fire).
    // size is how wide and tall it is in pixels: one tile, or two for the door pieces that fill a gap in a wall
    internal abstract class Block(Vector2 position, ISprite sprite, int size = RoomLayout.TileSize) : IBlock
    {
        // Blocks never move, so the hitbox is set once, covering the block where it is drawn
        public ICollider Collider { get; } = new BoxCollider(new Rectangle((int)position.X, (int)position.Y, size, size));

        // Blocks are solid unless a kind of block says otherwise
        public virtual bool IsSolid => true;

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
