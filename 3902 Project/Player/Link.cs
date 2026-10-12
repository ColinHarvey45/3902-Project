using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player.States;
using Projectiles;
using Sprites;

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
        public Direction Facing { get; set; } = Direction.Down;
        public ILinkState State { get; set; }
        public ILinkHealthState HealthState { get; set; }

        // What Link fires goes into projectiles, along with everything else flying around the room
        public Link(Vector2 startPosition, ProjectileManager projectiles)
        {
            Position = startPosition;
            this.projectiles = projectiles;

            State = new LinkStandingState(this);
            HealthState = new LinkHealthyState(this);
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
                    projectiles.Add(new Arrow(Position, Facing));
                    break;

                case SecondaryItem.Boomerang:
                    // Only one boomerang can be out at a time; Link can throw again once it is caught
                    // (or cleared away when he leaves the room)
                    if (projectiles.Contains(boomerang)) return;

                    ISprite boomerangSprite = ProjectileSpriteFactory.Instance.CreateLinkBoomerangSprite();
                    boomerang = new Boomerang(boomerangSprite, Position, Facing.ToVector(), this, ProjectileOwner.Link);
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
