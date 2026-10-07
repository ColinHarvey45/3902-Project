using Microsoft.Xna.Framework;
using System;

namespace Movement
{
    // Moves in a straight line in one of a set of directions, turning to a random new one every
    // so often and whenever the next step would leave the screen. Shared by enemies and the fairy.
    internal sealed class RandomMovement
    {
        // Nothing that uses this is bigger than one 16px tile at 4x scale
        private const int MaxObjectSize = 64;

        // Scales diagonals down so moving diagonally is no faster than moving straight
        private const float Diagonal = 0.7071f;

        public static readonly Vector2[] FourDirections = [new(1, 0), new(-1, 0), new(0, -1), new(0, 1)];
        public static readonly Vector2[] EightDirections =
        [
            new(1, 0), new(-1, 0), new(0, -1), new(0, 1),
            new(Diagonal, Diagonal), new(Diagonal, -Diagonal), new(-Diagonal, -Diagonal), new(-Diagonal, Diagonal)
        ];

        private static readonly Random RandomGenerator = new();

        private readonly Vector2[] directions;
        private readonly float changeTime;
        private readonly Rectangle area; // where the object's top-left corner is allowed to go
        private float timer;

        public Vector2 Direction { get; private set; }

        // Pixels per frame; enemies that speed up and slow down change this as they go
        public float Speed { get; set; }

        public RandomMovement(Vector2[] directions, float speed, float changeTime, Rectangle screenBounds)
        {
            this.directions = directions;
            Speed = speed;
            this.changeTime = changeTime;
            area = new Rectangle(screenBounds.X, screenBounds.Y, screenBounds.Width - MaxObjectSize, screenBounds.Height - MaxObjectSize);

            TurnRandomly();
        }

        public void TurnRandomly()
        {
            Direction = directions[RandomGenerator.Next(directions.Length)];
        }

        // Takes one step from position, also turning to a new random direction every changeTime seconds
        public Vector2 Wander(GameTime gameTime, Vector2 position)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= changeTime)
            {
                timer = 0f;
                TurnRandomly();
            }

            return Step(position);
        }

        // Takes one step from position; if that step would leave the screen, it first turns
        // to a random direction that stays on screen (or turns back if none does)
        public Vector2 Step(Vector2 position)
        {
            Vector2 next = position + Direction * Speed;
            if (area.Contains(next)) return next;

            Vector2[] staysInside = Array.FindAll(directions, direction => area.Contains(position + direction * Speed));
            Direction = staysInside.Length > 0 ? staysInside[RandomGenerator.Next(staysInside.Length)] : -Direction;

            return position + Direction * Speed;
        }
    }
}
