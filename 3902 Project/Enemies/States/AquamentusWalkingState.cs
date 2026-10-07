using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies.States
{
    internal sealed class AquamentusWalkingState : IEnemyState
    {
        private const float TimeBetweenAttacks = 2.5f;

        private readonly Aquamentus aquamentus;
        private float timer;

        public AquamentusWalkingState(Aquamentus aquamentus)
        {
            this.aquamentus = aquamentus;
            aquamentus.Sprite = EnemySpriteFactory.Instance.CreateAquamentusSprite();
        }

        public void Update(GameTime gameTime)
        {
            aquamentus.Pace(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= TimeBetweenAttacks)
                aquamentus.ChangeState(new AquamentusAttackingState(aquamentus));
        }
    }
}
