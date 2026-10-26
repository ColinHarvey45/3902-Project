using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Movement;
using Sprites;

namespace Enemies
{
    // Takes off, flutters around in all eight directions while speeding up and slowing
    // down, then lands for a moment. Whether it is flying or resting is decided by its state
    internal sealed class Keese : Enemy
    {
        // Keese change direction much more often than walking enemies
        private const float TurnTime = 0.4f;

        private IEnemyState state;

        public Keese(Vector2 startPosition)
            : base(startPosition, RandomMovement.EightDirections, TurnTime)
        {
            Sprite = EnemySpriteFactory.Instance.CreateKeeseSprite();
            state = new KeeseFlyingState(this);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        protected override void UpdateBehavior(GameTime gameTime)
        {
            state.Update(gameTime);
            UpdateCollider();
        }

        // Keese fly over blocks, and their wandering already keeps them off the walls
        public override void OnBlocked(Vector2 push) { }

        // Called by the flying state every frame: flutters at the given speed, flapping its wings
        public void Fly(GameTime gameTime, float speed)
        {
            Movement.Speed = speed;
            Wander(gameTime);
        }
    }
}
