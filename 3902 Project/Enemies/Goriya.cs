using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Rooms;
using Sprites;

namespace Enemies
{
    // Wanders like other enemies, but sometimes stops to throw a boomerang and waits for it to come back
    internal sealed class Goriya : Enemy, IBoomerangThrower
    {
        private const int ThrowChancePercent = 40; // chance of throwing each time it turns
        private const float BoomerangSpawnDistance = 24f;

        private readonly ProjectileManager projectiles;
        private IEnemyState state;
        private Boomerang boomerang;

        // Its boomerang goes into projectiles, along with everything else flying around the room
        public Goriya(Vector2 startPosition, ProjectileManager projectiles)
            : base(startPosition)
        {
            this.projectiles = projectiles;
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);
            state = new GoriyaWalkingState(this);
        }

        // The middle of Goriya, who is one tile big; its boomerang is thrown from just in front of here
        public Vector2 Center => Position + new Vector2(RoomLayout.TileSize / 2f);

        // True until its boomerang is caught (or cleared away when Link leaves the room)
        public bool IsBoomerangOut => projectiles.Contains(boomerang);

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        public override void Update(GameTime gameTime)
        {
            state.Update(gameTime);
        }

        // Called by the walking state
        public void Walk(GameTime gameTime)
        {
            Wander(gameTime);
        }

        protected override void OnDirectionChanged()
        {
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);

            if (RandomGenerator.Next(100) < ThrowChancePercent)
                ThrowBoomerang();
        }

        private void ThrowBoomerang()
        {
            Vector2 spawnPosition = Center + CurrentDirection * BoomerangSpawnDistance;
            ISprite boomerangSprite = ProjectileSpriteFactory.Instance.CreateGoriyaBoomerangSprite();

            boomerang = new Boomerang(boomerangSprite, spawnPosition, CurrentDirection, this, ProjectileOwner.Enemy);
            projectiles.Add(boomerang);
            ChangeState(new GoriyaThrowingState(this));
        }
    }
}
