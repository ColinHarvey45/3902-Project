using Microsoft.Xna.Framework;

namespace Player
{
    internal static class DirectionExtensions
    {

        // One pixel in the given direction (screen y grows downwards)
        public static Vector2 ToVector(this Direction direction) => direction switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            _ => new Vector2(1, 0)
        };

        // The direction a vector mostly points in
        public static Direction ToDirection(this Vector2 vector)
        {
            if (System.Math.Abs(vector.X) >= System.Math.Abs(vector.Y))
                return vector.X < 0 ? Direction.Left : Direction.Right;

            return vector.Y < 0 ? Direction.Up : Direction.Down;
        }

        // The way back, e.g. the wall Link comes in by after leaving the last room through the one facing it
        public static Direction Opposite(this Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            _ => Direction.Left
        };

    }
}
