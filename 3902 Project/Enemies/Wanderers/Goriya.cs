using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Rooms;
using Sprites;
using CSE_3902_Project;
using Enemies;

namespace Enemies.Wanderers
{
    // Wanders like other enemies, but sometimes stops to throw a boomerang, and stands still until it comes back
    internal sealed class Goriya : WanderingEnemy, IBoomerangThrower
    {
        private const int ThrowChancePercent = 40; // chance of throwing each time it turns
        private const float BoomerangSpawnDistance = 24f;

        // Three sword hits, as in the NES
        private const int GoriyaHealth = 3;

        private readonly ProjectileManager projectiles;
        private IProjectile boomerang;

        // Its boomerang goes into projectiles, along with everything else flying around the room
        public Goriya(Vector2 startPosition, ProjectileManager projectiles)
            : base(startPosition)
        {
            this.projectiles = projectiles;
            Health = new EnemyHealth(GoriyaHealth);
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);
        }

        // The middle of Goriya, who is one tile big; its boomerang is thrown from just in front of here
        public Vector2 Center => Position + new Vector2(RoomLayout.TileSize / 2f);

        // True until its boomerang is caught (or cleared away when Link leaves the room)
        public bool IsBoomerangOut => projectiles.Contains(boomerang);

        // Stands still while its boomerang is out, and walks about the rest of the time
        protected override void UpdateBehavior(GameTime gameTime)
        {
            if (!IsBoomerangOut)
                Wander(gameTime);

            UpdateCollider();
        }

        protected override void OnDirectionChanged()
        {
            Sprite = EnemySpriteFactory.Instance.CreateGoriyaSprite(CurrentDirection);

            if (RandomGenerator.Next(GameSettings.Percent) < ThrowChancePercent)
                ThrowBoomerang();
        }

        private void ThrowBoomerang()
        {
            Vector2 spawnPosition = Center + CurrentDirection * BoomerangSpawnDistance;
            boomerang = ProjectileFactory.CreateGoriyaBoomerang(spawnPosition, CurrentDirection, this);
            projectiles.Add(boomerang);
        }
    }
}
