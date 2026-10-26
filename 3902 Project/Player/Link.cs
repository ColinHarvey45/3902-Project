using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player.States;
using Projectiles;
using Sprites;
using Colliders;

namespace Player
{
    // Link's own data (where he is, which way he faces, the boomerang he has out). What he does with
    // input is decided by his current State, and whether he is hurt by his HealthState
    internal sealed class Link : IPlayer, IBoomerangThrower
    {
        private const float MoveSpeed = 3f; // pixels per frame
        private const float TileSize = 16 * Sprite.Scale;
        private static readonly Vector2 BombTileOffset = new(16, 4); // centres the 8x14 bomb inside its tile

        private readonly ProjectileManager projectiles;
        private Direction? requestedDirection;
        private Boomerang boomerang;

        public Vector2 Position { get; private set; }

        // The middle of Link, who is one tile big; arrows and his boomerang start here
        public Vector2 Center => Position + new Vector2(TileSize / 2);

        public Direction Facing { get; set; } = Direction.Down;
        public ILinkState State { get; set; }
        public ILinkHealthState HealthState { get; set; }
        public ICollider collider { get; set; }

        // His sword's collider while he is swinging it, or null when he isn't
        // How much health his sword takes off an enemy it hits
        public int SwordDamage { get; } = 1;

        public ICollider SwordCollider => (State as LinkAttackingState)?.SwordCollider;

        private const int colliderWidth = (int)(12 * Sprite.Scale);
        private const int colliderHeight = (int)(12 * Sprite.Scale);
        private static readonly Point ColliderOffset = new(
            (int)(2 * Sprite.Scale), (int)(2 * Sprite.Scale));

        // What Link fires goes into projectiles, along with everything else flying around the room
        public Link(Vector2 startPosition, ProjectileManager projectiles)
        {
            Position = startPosition;
            this.projectiles = projectiles;

            State = new LinkStandingState(this);
            HealthState = new LinkHealthyState(this);
            // Initialize collider sized to Link's sprite (one tile)
            this.collider = new BoxCollider(
                new Rectangle((int)startPosition.X, (int)startPosition.Y, colliderWidth, colliderHeight),
                ColliderOffset);
        }

        // Link only walks in four directions, so if several are held the first one asked for wins
        public void Move(Direction direction)
        {
            requestedDirection ??= direction;
        }

        public void SwordAttack()
        {
            State.SwordAttack();
        }

        public void UseItem(SecondaryItem item)
        {
            State.UseItem(item);
        }

        public void TakeDamage()
        {
            HealthState.TakeDamage();
        }

        public void Update(GameTime gameTime)
        {
            if (requestedDirection is Direction direction)
                State.Move(direction);
            else
                State.Stop();
            requestedDirection = null;

            State.Update(gameTime);
            HealthState.Update(gameTime);
            collider.Update(Position);
        }

        // Apply an immediate displacement to Link (used for knockback)
        public void ApplyKnockback(Vector2 offset)
        {
            Position += offset;
            collider.Update(Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            State.Draw(spriteBatch, HealthState.Tint);
        }

        // Called by the walking state once per frame
        public void StepForward()
        {
            Position += Facing.ToVector() * MoveSpeed;
        }

        // Called by the states that let Link use an item right now
        public void SpawnProjectile(SecondaryItem item)
        {
            switch (item)
            {
                case SecondaryItem.Arrow:
                    projectiles.Add(new Arrow(Center, Facing));
                    break;

                case SecondaryItem.Boomerang:
                    // Only one boomerang can be out at a time; Link can throw again once it is caught
                    // (or cleared away when he leaves the room)
                    if (projectiles.Contains(boomerang)) return;

                    ISprite boomerangSprite = ProjectileSpriteFactory.Instance.CreateLinkBoomerangSprite();
                    boomerang = new Boomerang(boomerangSprite, Center, Facing.ToVector(), this, ProjectileOwner.Link);
                    projectiles.Add(boomerang);
                    break;

                case SecondaryItem.Bomb:
                    // Placed on the tile in front of Link
                    projectiles.Add(new Bomb(Position + Facing.ToVector() * TileSize + BombTileOffset));
                    break;
            }
        }
    }
}