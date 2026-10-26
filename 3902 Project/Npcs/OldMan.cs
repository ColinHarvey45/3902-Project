using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;

namespace Npcs
{
    // Stands in place next to the fires; he doesn't move or animate
    internal sealed class OldMan(Vector2 position) : INpc
    {

        private readonly ISprite sprite = NpcSpriteFactory.Instance.CreateOldManSprite();

        // He is one tile big, and Link can't walk through him
        public ICollider Collider { get; } =
            new BoxCollider(new Rectangle((int)position.X, (int)position.Y, RoomLayout.TileSize, RoomLayout.TileSize));

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

    }
}
