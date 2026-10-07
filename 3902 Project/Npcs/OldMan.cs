using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Npcs
{
    // Stands in place next to the fires; he doesn't move or animate
    internal sealed class OldMan(Vector2 position) : INpc
    {

        private readonly ISprite sprite = NpcSpriteFactory.Instance.CreateOldManSprite();

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

    }
}
