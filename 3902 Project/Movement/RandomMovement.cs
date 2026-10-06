using Microsoft.Xna.Framework;
using System;

namespace Movement
{
    // Moves in a straight line and picks a new random direction every few seconds.
    // Shared by enemies that wander and by the fairy.
    internal class RandomMovement
    {
        private static readonly Random RandomGenerator = new();

        private readonly Vector2[] directions;
        private readonly float changeTime;
        private readonly float speed;
        private float timer = 0f;

        public Vector2 Direction { get; private set; }

        // True only on the frame a new direction was picked
        public bool JustTurned { get; private set; } = false;

        // How far to move this frame, in pixels
        public Vector2 Velocity => Direction * speed;

        public RandomMovement(Vector2[] directions, float changeTime, float speed)
        {
            this.directions = directions;
            this.changeTime = changeTime;
            this.speed = speed;
            Direction = RandomDirection();
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            JustTurned = timer >= changeTime;

            if (JustTurned)
            {
                timer = 0f;
                Direction = RandomDirection();
            }
        }

        private Vector2 RandomDirection()
        {
            return directions[RandomGenerator.Next(directions.Length)];
        }
    }
}
