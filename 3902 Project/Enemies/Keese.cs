using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Movement;
using Sprites;

namespace Enemies
{
    // Takes off, flutters around in all eight directions while speeding up and slowing
    // down, then lands for a moment. Whether it is flying or resting is decided by its state
    internal class Keese : Enemy
    {
        // Keese change direction much more often than walking enemies
        private const float TurnTime = 0.4f;

        private IEnemyState state;

        public Keese(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds, RandomMovement.EightDirections, TurnTime)
        {
            Sprite = EnemySpriteFactory.Instance.CreateKeeseSprite();
            state = new KeeseFlyingState(this);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        public override void Update(GameTime gameTime)
        {
            state.Update(gameTime);
        }

        // Called by the flying state every frame: flutters at the given speed, flapping its wings
        public void Fly(GameTime gameTime, float speed)
        {
            Movement.Speed = speed;
            Wander(gameTime);
        }
    }
}
