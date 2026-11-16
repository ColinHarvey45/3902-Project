using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Movement;
using Rooms;
using System;
using Colliders;
using Sprites;

namespace Enemies
{
    // Shared behaviour for enemies that wander: walk in a straight line, pick a new random
    // direction every couple of seconds, and turn around instead of leaving the room's floor
    internal abstract class WanderingEnemy : IDamageable
    {
        protected const float DefaultDirectionChangeTime = 2f; // seconds

        // Most enemies are a tile big, with a hitbox 12 x 12 sheet pixels, 2 in from the sprite's edges
        protected const int DefaultColliderSize = (int)(12 * Sprites.Sprite.Scale);
        private static readonly Point DefaultColliderOffset = new((int)(2 * Sprites.Sprite.Scale), (int)(2 * Sprites.Sprite.Scale));
        private const float WalkSpeed = 1f; // pixels per frame

        // How long the boomerang stops an enemy for
        private const float StunTime = 2f; // seconds

        // A blow an enemy survives knocks it back a tile, quickly, unless it hits a wall or a block first
        private const float KnockbackDistance = RoomLayout.TileSize;
        private const float KnockbackSpeed = 8f; // pixels per frame

        // Every enemy starts with 1 health; a subclass that should be tougher sets Health to a new EnemyHealth(n)
        protected EnemyHealth Health { get; set; } = new();

        private float stunTimeLeft;
        private Vector2 knockbackDirection;
        private float knockbackLeft;

        protected static readonly Random RandomGenerator = new();

        public Vector2 Position { get; protected set; }
        protected RandomMovement Movement { get; }
        protected Vector2 CurrentDirection => Movement.Direction;
        protected ISprite Sprite { get; set; }
        public ICollider Collider { get; set; }

        public bool IsInvincible => Health.IsInvincible;
        public bool IsDead => Health.IsDead;

        protected WanderingEnemy(Vector2 startPosition)
            : this(startPosition, RandomMovement.FourDirections, DefaultDirectionChangeTime)
        {
        }

        protected WanderingEnemy(Vector2 startPosition, Vector2[] directions, float directionChangeTime)
            : this(startPosition, directions, directionChangeTime, DefaultColliderSize, DefaultColliderSize, DefaultColliderOffset)
        {
        }

        protected WanderingEnemy(Vector2 startPosition, Vector2[] directions, float directionChangeTime,
            int colliderWidth, int colliderHeight, Point colliderOffset = default)
        {
            Position = startPosition;
            Movement = new RandomMovement(directions, WalkSpeed, directionChangeTime, RoomLayout.FloorBounds);
            this.Collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, colliderWidth, colliderHeight),
                colliderOffset);

            // Where the hitbox really is, offset and all, before the enemy has first moved
            UpdateCollider();
        }

        public virtual void TakeDamage(int damage, Vector2 hitFrom)
        {
            if (IsDead || IsInvincible)
                return;

            Health.TakeDamage(damage);

            if (!IsDead)
                KnockBackFrom(hitFrom);
        }

        // The smallest enemies (Gel and Keese) are killed by the boomerang; the rest are only stunned
        protected virtual bool KilledByBoomerang => false;

        public void Stun()
        {
            if (KilledByBoomerang)
                TakeDamage(Health.HitPoints, Position);
            else
                stunTimeLeft = StunTime;
        }

        // Straight back along whichever way the blow mostly came from
        private void KnockBackFrom(Vector2 hitFrom)
        {
            Vector2 away = Collider.GetCollider().Center.ToVector2() - hitFrom;
            if (away == Vector2.Zero)
                return;

            knockbackDirection = Math.Abs(away.X) >= Math.Abs(away.Y)
                ? new Vector2(Math.Sign(away.X), 0)
                : new Vector2(0, Math.Sign(away.Y));
            knockbackLeft = KnockbackDistance;
        }

        // The hurt flash is always updated here, so enemies that move differently only override UpdateBehavior.
        // While it is being knocked back, or is stunned, it does nothing else
        public void Update(GameTime gameTime)
        {
            if (IsDead)
                return;

            Health.Update(gameTime);

            if (knockbackLeft > 0)
            {
                // It stops at the edge of the floor, as at a wall, so it is never knocked out into a doorway
                float step = Math.Min(KnockbackSpeed, knockbackLeft);
                Vector2 next = Position + knockbackDirection * step;
                if (Movement.Allows(next))
                {
                    knockbackLeft -= step;
                    Position = next;
                    UpdateCollider();
                }
                else
                {
                    knockbackLeft = 0;
                }

                return;
            }

            if (stunTimeLeft > 0)
            {
                stunTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }

            UpdateBehavior(gameTime);
        }

        // How this enemy moves each frame; walks around at random unless overridden
        protected virtual void UpdateBehavior(GameTime gameTime)
        {
            Wander(gameTime);
            this.Collider.Update(Position);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead)
                return;

            Sprite.Draw(spriteBatch, Position, Health.Tint);
        }

        protected void Wander(GameTime gameTime)
        {
            Vector2 oldDirection = CurrentDirection;
            Position = Movement.Wander(gameTime, Position);

            if (CurrentDirection != oldDirection)
                OnDirectionChanged();

            Sprite.Update(gameTime);
        }

        public void UpdateCollider()
        {
            Collider.Update(Position);
        }

        // Walked into a wall or a solid block: step back out, and head off a new way. Knocked back into one,
        // it stops there
        public virtual void OnBlocked(Vector2 push)
        {
            Position += push;
            UpdateCollider();
            knockbackLeft = 0;

            Vector2 oldDirection = CurrentDirection;
            Movement.TurnRandomly();
            if (CurrentDirection != oldDirection)
                OnDirectionChanged();
        }

        // Lets an enemy react when it turns (on its timer or at the edge of the floor),
        // e.g. by switching to a sprite facing the new way
        protected virtual void OnDirectionChanged() { }
    }
}