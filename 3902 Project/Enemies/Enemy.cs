using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Movement;
using Rooms;
using System;
using Colliders;
using Sprites;

namespace Enemies
{
    // Shared behaviour for enemies that wander: walk in a straight line, pick a new random
    // direction every couple of seconds, and turn around instead of leaving the room's floor
    internal abstract class Enemy : IEnemy
    {
        private const float DefaultDirectionChangeTime = 2f;
        private const float WalkSpeed = 1f; // pixels per frame

        protected static readonly Random RandomGenerator = new();

        public Vector2 Position { get; protected set; }
        protected RandomMovement Movement { get; }
        protected Vector2 CurrentDirection => Movement.Direction;
        protected ISprite Sprite { get; set; }
        public ICollider Collider { get; set; }


        protected Enemy(Vector2 startPosition)
            : this(startPosition, RandomMovement.FourDirections, DefaultDirectionChangeTime)
        {
        }

        protected Enemy(Vector2 startPosition, Vector2[] directions, float directionChangeTime)
            : this(startPosition, directions, directionChangeTime,
                (int)(12 * Sprites.Sprite.Scale), (int)(12 * Sprites.Sprite.Scale),
                new Point((int)(2 * Sprites.Sprite.Scale), (int)(2 * Sprites.Sprite.Scale)))
        {
        }

        protected Enemy(Vector2 startPosition, Vector2[] directions, float directionChangeTime,
            int colliderWidth, int colliderHeight, Point colliderOffset = default)
        {
            Position = startPosition;
            Movement = new RandomMovement(directions, WalkSpeed, directionChangeTime, RoomLayout.FloorBounds);
            this.Collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, colliderWidth, colliderHeight),
                colliderOffset);
        }

        public virtual void Update(GameTime gameTime)
        {
            Wander(gameTime);
            this.Collider.Update(Position);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        protected void Wander(GameTime gameTime)
        {
            Vector2 oldDirection = CurrentDirection;
            Position = Movement.Wander(gameTime, Position);

            if (CurrentDirection != oldDirection)
                OnDirectionChanged();

            Sprite.Update(gameTime);
        }

        public void UpdateCollider()
        {
            Collider.Update(Position);
        }

        // Lets an enemy react when it turns (on its timer or at the edge of the floor),
        // e.g. by switching to a sprite facing the new way
        protected virtual void OnDirectionChanged() { }
    }
}
