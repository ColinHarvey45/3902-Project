using System;
using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;
using Colliders;

namespace Enemies
{
    // Sits still until Link lines up with it across or down the room, then shoots out at him and slowly slides
    // back to where it started, as in the NES. What it is doing right now is decided by its state (waiting,
    // charging or returning)
    internal sealed class BladeTrap : IEnemy, ILinkWatcher
    {
        private readonly ISprite sprite = EnemySpriteFactory.Instance.CreateBladeTrapSprite();
        private IEnemyState state;

        // Where Link's hitbox was this frame, or null before the room has shown it to the trap
        private Rectangle? link;

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

        public void Watch(Rectangle linkHitbox)
        {
            link = linkHitbox;
        }

        // The way to charge to reach Link if he is lined up with it, across the room or up and down it; null if
        // he isn't
        public Vector2? DirectionToLink()
        {
            if (link is not Rectangle target)
                return null;

            Rectangle trap = Collider.GetCollider();
            bool sameRow = target.Top < trap.Bottom && target.Bottom > trap.Top;
            bool sameColumn = target.Left < trap.Right && target.Right > trap.Left;

            if (sameRow && target.Center.X != trap.Center.X)
                return new Vector2(Math.Sign(target.Center.X - trap.Center.X), 0);

            if (sameColumn && target.Center.Y != trap.Center.Y)
                return new Vector2(0, Math.Sign(target.Center.Y - trap.Center.Y));

            return null;
        }

        // How far it shoots out in direction: to the middle of the room, where a trap from the other side meets it.
        // A trap that starts in the middle goes right across, until it hits a wall or a block
        public float ChargeDistance(Vector2 direction)
        {
            Vector2 toMiddle = RoomLayout.FloorBounds.Center.ToVector2() - (Home + new Vector2(ColliderWidth / 2f, ColliderHeight / 2f));
            float alongCharge = Vector2.Dot(toMiddle, direction) - (direction.X != 0 ? ColliderWidth : ColliderHeight) / 2f;

            return alongCharge > 0 ? alongCharge : RoomLayout.FloorBounds.Width;
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

        // Charged into a wall or a block: stop there and start sliding back home
        public void OnBlocked(Vector2 push)
        {
            Position += push;
            UpdateCollider();
            ChangeState(new BladeTrapReturningState(this));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, Position);
        }
    }
}
