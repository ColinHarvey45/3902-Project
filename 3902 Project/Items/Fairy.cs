using Microsoft.Xna.Framework;
using Movement;
using Rooms;
using Sprites;

namespace Items
{
    // Flutters around in all eight directions instead of staying where it was placed,
    // turning back before it leaves the room's floor
    internal sealed class Fairy : Item
    {
        private const float DirectionChangeTime = 0.5f;
        private const float FlySpeed = 1.5f; // pixels per frame

        private readonly RandomMovement movement;

        public Fairy(Vector2 position)
            : base(position, ItemSpriteFactory.Instance.CreateFairySprite())
        {
            movement = new RandomMovement(RandomMovement.EightDirections, FlySpeed, DirectionChangeTime, RoomLayout.FloorBounds);
        }

        public override void Update(GameTime gameTime)
        {
            Position = movement.Wander(gameTime, Position);

            base.Update(gameTime);
        }
    }
}
