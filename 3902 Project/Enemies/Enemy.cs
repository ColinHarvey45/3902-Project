using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Movement;
using Rooms;
using System;
using Colliders;

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
        public ICollider collider { get; set; }
        private const int colliderWidth = 16;
        private const int colliderHeight = 16;


        protected Enemy(Vector2 startPosition)
            : this(startPosition, RandomMovement.FourDirections, DefaultDirectionChangeTime)
        {
        }

        protected Enemy(Vector2 startPosition, Vector2[] directions, float directionChangeTime)
        {
            Position = startPosition;
            Movement = new RandomMovement(directions, WalkSpeed, directionChangeTime, RoomLayout.FloorBounds);
            this.collider = new BoxCollider(new Rectangle((int)startPosition.X, (int)startPosition.Y, colliderWidth, colliderHeight));
        }

        public virtual void Update(GameTime gameTime)
        {
            Wander(gameTime);
            this.collider.Update(Position);
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

        // Lets an enemy react when it turns (on its timer or at the edge of the floor),
        // e.g. by switching to a sprite facing the new way
        protected virtual void OnDirectionChanged() { }
    }
}
