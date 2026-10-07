using System.Collections.Generic;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player.States;
using Projectiles;
using Sprites;

namespace Player
{
    // Link's own data (where he is, which way he faces, what he has thrown). What he does with
    // input is decided by his current State, and whether he is hurt by his HealthState
    internal sealed class Link : IPlayer, IBoomerangThrower
    {
        private const float MoveSpeed = 3f; // pixels per frame
        private const float TileSize = 16 * Sprite.Scale;
        private static readonly Vector2 BombTileOffset = new(16, 4); // centres the 8x14 bomb inside its tile

        private readonly Rectangle screenBounds;
        private readonly List<IProjectile> projectiles = [];
        private Direction? requestedDirection;
        private bool boomerangInFlight;

        public Vector2 Position { get; private set; }
        public Direction Facing { get; set; } = Direction.Down;
        public ILinkState State { get; set; }
        public ILinkHealthState HealthState { get; set; }

        public Link(Vector2 startPosition, Rectangle screenBounds)
        {
            Position = startPosition;
            this.screenBounds = screenBounds;

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

            foreach (IProjectile projectile in projectiles)
                projectile.Update(gameTime);
            projectiles.RemoveAll(projectile => projectile.IsFinished);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            State.Draw(spriteBatch, HealthState.Tint);

            foreach (IProjectile projectile in projectiles)
                projectile.Draw(spriteBatch);
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
                    projectiles.Add(new Arrow(Position, Facing, screenBounds));
                    break;

                case SecondaryItem.Boomerang:
                    // Only one boomerang can be out at a time; Link can throw again once it comes back
                    if (boomerangInFlight) return;

                    ISprite boomerangSprite = ProjectileSpriteFactory.Instance.CreateLinkBoomerangSprite();
                    projectiles.Add(new Boomerang(boomerangSprite, Position, Facing.ToVector(), this));
                    boomerangInFlight = true;
                    break;

                case SecondaryItem.Bomb:
                    // Placed on the tile in front of Link
                    projectiles.Add(new Bomb(Position + Facing.ToVector() * TileSize + BombTileOffset));
                    break;
            }
        }

        public void OnBoomerangReturned()
        {
            boomerangInFlight = false;
        }
    }
}
