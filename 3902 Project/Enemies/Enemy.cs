using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enemies
{
    // Shared behaviour for enemies that wander: walk in a straight line and
    // pick a new random direction every couple of seconds
    internal abstract class Enemy : IEnemy
    {
        private const float DirectionChangeTime = 2f;
        private const float MoveSpeed = 1f; // pixels per frame

        private static readonly Vector2[] FourDirections = [new(1, 0), new(-1, 0), new(0, -1), new(0, 1)];

        protected static readonly Random RandomGenerator = new();

        private readonly Vector2[] directions;
        private float directionTimer = 0f;

        public Vector2 Position { get; private set; }
        protected Vector2 CurrentDirection { get; private set; }
        protected ISprite Sprite { get; set; }

        protected Enemy(Vector2 startPosition)
            : this(startPosition, FourDirections)
        {
        }

        protected Enemy(Vector2 startPosition, Vector2[] directions)
        {
            Position = startPosition;
            this.directions = directions;
            CurrentDirection = RandomDirection();
        }

        public virtual void Update(GameTime gameTime)
        {
            Wander(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        protected void Wander(GameTime gameTime)
        {
            directionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (directionTimer >= DirectionChangeTime)
            {
                directionTimer = 0f;
                CurrentDirection = RandomDirection();
                OnDirectionChanged();
            }

            Position += CurrentDirection * MoveSpeed;
            Sprite.Update(gameTime);
        }

        // Lets an enemy react when it turns, e.g. by switching to a sprite facing the new way
        protected virtual void OnDirectionChanged() { }

        private Vector2 RandomDirection()
        {
            return directions[RandomGenerator.Next(directions.Length)];
        }
    }
}
