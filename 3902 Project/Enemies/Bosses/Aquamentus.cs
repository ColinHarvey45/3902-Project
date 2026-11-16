using Audio;
using Enemies.States;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;
using System;
using Colliders;
using Enemies;

namespace Enemies.Bosses
{
    // The dungeon 1 boss: paces back and forth, and every so often opens its mouth to breathe
    // a spread of three fireballs. Its state decides whether its mouth is open
    internal sealed class Aquamentus : IDamageable
    {
        private const float PaceSpeed = 0.5f; // pixels per frame
        private const float PaceDistance = 48f; // how far it walks either side of where it started
        private const float FireballSpeed = 150f; // pixels per second

        // Six sword hits (three arrows, two bombs), as in the NES
        private const int AquamentusHealth = 6;

        // It roars as Link comes in, and again every few seconds while it is alive
        private const float RoarInterval = 3f; // seconds

        // Where the fireballs come out, relative to its top-left corner (it faces left)
        private static readonly Vector2 MouthOffset = new(0, 32);
        public ICollider Collider { get; set; }
        private const int ColliderWidth = (int)(24 * Sprites.Sprite.Scale);
        private const int ColliderHeight = (int)(32 * Sprites.Sprite.Scale);
        private static readonly Point ColliderOffset = Point.Zero;

        // They fly left in a fan: the outer two go this far up or down for every pixel left
        private const float FireballSpread = 0.4f;
        private static readonly Vector2[] FireballDirections =
        [
            Vector2.Normalize(new Vector2(-1, -FireballSpread)),
            new Vector2(-1, 0),
            Vector2.Normalize(new Vector2(-1, FireballSpread))
        ];

        private readonly float homeX;
        private readonly ProjectileManager projectiles;
        private float paceDirection = -1f;
        private IEnemyState state;
        private readonly EnemyHealth health = new(AquamentusHealth, Sound.BossHit);
        private float roarTimer = RoarInterval;

        public Vector2 Position { get; private set; }
        public ISprite Sprite { get; set; }
        public bool IsInvincible => health.IsInvincible;
        public bool IsDead => health.IsDead;

        // Its fireballs go into projectiles, along with everything else flying around the room
        public Aquamentus(Vector2 startPosition, ProjectileManager projectiles)
        {
            Position = startPosition;
            homeX = startPosition.X;
            this.projectiles = projectiles;
            state = new AquamentusWalkingState(this);

            this.Collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, ColliderWidth, ColliderHeight),
                ColliderOffset);
        }

        // Only called while Link is in its room, since only that room is updated
        private void Roar(GameTime gameTime)
        {
            roarTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (roarTimer < RoarInterval)
                return;

            roarTimer = 0f;
            AudioManager.Instance.Play(Sound.BossScream);
        }

        public void ChangeState(IEnemyState newState)
        {
            state = newState;
        }

        // Too big to be knocked back
        public void TakeDamage(int damage, Vector2 hitFrom)
        {
            health.TakeDamage(damage);
        }

        // The boomerang just bounces off it
        public void Stun() { }

        public void Update(GameTime gameTime)
        {
            if (IsDead)
                return;

            health.Update(gameTime);
            state.Update(gameTime);
            UpdateCollider();
            Roar(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead)
                return;

            Sprite.Draw(spriteBatch, Position, health.Tint);
        }

        // Called by both states: it keeps walking whether or not its mouth is open
        public void Pace(GameTime gameTime)
        {
            Position += new Vector2(paceDirection * PaceSpeed, 0);

            if (Math.Abs(Position.X - homeX) >= PaceDistance)
                paceDirection = -paceDirection;

            Sprite.Update(gameTime);
        }

        // Called by the attacking state when its mouth opens
        public void BreatheFireballs()
        {
            Vector2 mouth = Position + MouthOffset;

            foreach (Vector2 direction in FireballDirections)
                projectiles.Add(new Fireball(mouth, direction * FireballSpeed));
        }

        public void UpdateCollider()
        {
            Collider.Update(Position);
        }

        // Paced into a wall or a block: step back out and pace the other way
        public void OnBlocked(Vector2 push)
        {
            Position += push;
            UpdateCollider();
            paceDirection = -paceDirection;
        }
    }
}
