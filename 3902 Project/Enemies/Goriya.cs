using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;
using Sprites;

namespace Enemies
{
    // Wanders like other enemies, but sometimes stops to throw a boomerang and waits for it to come back
    internal sealed class Goriya : Enemy, IBoomerangThrower
    {
        private const int ThrowChancePercent = 40; // chance of throwing each time it turns
        private const float BoomerangSpawnDistance = 24f;

        private IEnemyState state;
        private Boomerang boomerang;

        public Goriya(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds)
        {
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);
            state = new GoriyaWalkingState(this);
        }

        public override void Update(GameTime gameTime)
        {
            state.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            boomerang?.Draw(spriteBatch);
        }

        // Called by the walking state
        public void Walk(GameTime gameTime)
        {
            Wander(gameTime);
        }

        // Called by the throwing state while Goriya stands still waiting for its boomerang
        public void UpdateBoomerang(GameTime gameTime)
        {
            boomerang?.Update(gameTime);
        }

        public void OnBoomerangReturned()
        {
            boomerang = null;
            state = new GoriyaWalkingState(this);
        }

        protected override void OnDirectionChanged()
        {
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);

            if (RandomGenerator.Next(100) < ThrowChancePercent)
                ThrowBoomerang();
        }

        private void ThrowBoomerang()
        {
            Vector2 spawnPosition = Position + CurrentDirection * BoomerangSpawnDistance;
            ISprite boomerangSprite = ProjectileSpriteFactory.Instance.CreateGoriyaBoomerangSprite();

            boomerang = new Boomerang(boomerangSprite, spawnPosition, CurrentDirection, this);
            state = new GoriyaThrowingState(this);
        }
    }
}
