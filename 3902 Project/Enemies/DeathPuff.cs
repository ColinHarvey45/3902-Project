using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;

namespace Enemies
{
    // The burst of light left for a moment where an enemy died. drop is the item it left behind, if any, which
    // appears once the burst has gone (see Room)
    internal sealed class DeathPuff(Vector2 middle, IItem drop)
    {
        private static readonly Vector2 HalfTile = new(RoomLayout.TileSize / 2f);

        private readonly ISprite sprite = EnemySpriteFactory.Instance.CreateDeathPuffSprite();

        public bool IsFinished => sprite.IsFinished;
        public IItem Drop => drop;

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, middle - HalfTile);
        }
    }
}
