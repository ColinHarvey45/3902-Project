using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Keese : Enemy
    {
        // Keese fly, so unlike other enemies they can also move diagonally
        private static readonly Vector2[] FlyingDirections =
        [
            new(1, 0), new(-1, 0), new(0, -1), new(0, 1),
            new(1, 1), new(1, -1), new(-1, -1), new(-1, 1)
        ];

        public Keese(Vector2 startPosition)
            : base(startPosition, FlyingDirections)
        {
            Sprite = EnemySpriteFactory.Instance.CreateKeeseSprite();
        }
    }
}
