using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;

namespace Environment
{
    // A piece of the room that stays where it was placed; its sprite may still animate (e.g. fire)
    internal abstract class Block(Vector2 position, ISprite sprite) : IBlock
    {
        // Blocks never move so hitbox is set once. One tile big where the block is drawn.
        public ICollider Collider { get; } = new BoxCollider(
            new Rectangle((int)position.X, (int)position.Y, RoomLayout.TileSize, RoomLayout.TileSize));

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
