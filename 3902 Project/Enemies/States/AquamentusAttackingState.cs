using Interfaces;
using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies.States
{
    // Opens its mouth, breathes three fireballs, and keeps it open for a moment before closing it again
    internal sealed class AquamentusAttackingState : IEnemyState
    {
        private const float MouthOpenTime = 1f;

        private readonly Aquamentus aquamentus;
        private float timer;

        public AquamentusAttackingState(Aquamentus aquamentus)
        {
            this.aquamentus = aquamentus;
            aquamentus.Sprite = EnemySpriteFactory.Instance.CreateAquamentusMouthOpenSprite();
            aquamentus.BreatheFireballs();
        }

        public void Update(GameTime gameTime)
        {
            aquamentus.Pace(gameTime);
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= MouthOpenTime)
                aquamentus.ChangeState(new AquamentusWalkingState(aquamentus));
        }
    }
}
