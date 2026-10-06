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

    }
}
