using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Colliders;

namespace Enemies
{
    // Sits still, then shoots out in a straight line and slowly slides back to where it started.
    // What it is doing right now is decided by its state (waiting, charging or returning)
    internal sealed class BladeTrap : IEnemy
    {
        private readonly ISprite sprite = EnemySpriteFactory.Instance.CreateBladeTrapSprite();
        private IEnemyState state;

        public Vector2 Home { get; }
        public Vector2 Position { get; set; }
        public ICollider Collider { get; set; }

        private const int ColliderWidth = (int)(16 * Sprites.Sprite.Scale);
        private const int ColliderHeight = (int)(16 * Sprites.Sprite.Scale);
        private static readonly Point ColliderOffset = Point.Zero;

        public BladeTrap(Vector2 position)
        {
            Home = position;
            Position = position;
            state = new BladeTrapWaitingState(this);
            this.Collider = new BoxCollider(
                new Rectangle((int)position.X, (int)position.Y, ColliderWidth, ColliderHeight),
                ColliderOffset);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        public void Update(GameTime gameTime)
        {
            state.Update(gameTime);
            UpdateCollider();
        }

        public void UpdateCollider()
        {
            Collider.Update(Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }


    }
}
