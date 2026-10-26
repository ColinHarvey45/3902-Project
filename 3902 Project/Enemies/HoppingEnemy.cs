using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Movement;

namespace Enemies
{
    // Enemies like Gel and Zol that move in quick hops with short rests in between,
    // instead of walking steadily. Whether it is hopping or resting is decided by its state
    internal abstract class HoppingEnemy : Enemy
    {
        private IEnemyState state;

        // How long one hop lasts, in seconds
        public float HopTime { get; }

        // The longest it rests between hops, in seconds; each rest is somewhere between half this and this
        public float MaxRestTime { get; }

        protected HoppingEnemy(Vector2 startPosition, float hopSpeed, float hopTime, float maxRestTime,
            int colliderWidth = 12 * (int)Sprites.Sprite.Scale,
            int colliderHeight = 12 * (int)Sprites.Sprite.Scale,
            Point colliderOffset = default)
            : base(startPosition, RandomMovement.FourDirections, 2f,
                colliderWidth, colliderHeight, colliderOffset)
        {
            Movement.Speed = hopSpeed;
            HopTime = hopTime;
            MaxRestTime = maxRestTime;
            state = new HopperHoppingState(this);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        protected override void UpdateBehavior(GameTime gameTime)
        {
            state.Update(gameTime);

            // Hopper states change Position directly, so synchronize after the state update.
            UpdateCollider();

            // Gel and Zol keep wobbling even while they rest
            Sprite.Update(gameTime);
        }

        // Called by the hopping state when a hop begins
        public void StartHop()
        {
            Movement.TurnRandomly();
        }

        // Called by the hopping state every frame of a hop
        public void HopForward()
        {
            Position = Movement.Step(Position);
        }
    }
}
