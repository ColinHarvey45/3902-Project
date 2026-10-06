using Microsoft.Xna.Framework;
using Movement;
using Sprites;

namespace Items
{
    // Flutters around in short hops instead of staying where it was placed
    internal class Fairy : Item
    {
        private const float DirectionChangeTime = 0.5f;
        private const float FlySpeed = 1.5f; // pixels per frame

        // Diagonals are scaled down so the fairy is no faster when flying diagonally
        private const float Diagonal = 0.7071f;
        private static readonly Vector2[] FlyingDirections =
        [
            new(1, 0), new(-1, 0), new(0, -1), new(0, 1),
            new(Diagonal, Diagonal), new(Diagonal, -Diagonal), new(-Diagonal, -Diagonal), new(-Diagonal, Diagonal)
        ];

        private readonly RandomMovement movement = new(FlyingDirections, DirectionChangeTime, FlySpeed);

        public Fairy(Vector2 position)
            : base(position, ItemSpriteFactory.Instance.CreateFairySprite())
        {
        }

        public override void Update(GameTime gameTime)
        {
            movement.Update(gameTime);
            Position += movement.Velocity;

            base.Update(gameTime);
        }
    }
}
