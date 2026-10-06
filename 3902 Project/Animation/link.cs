using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using Projectiles;

namespace Animation
{
    internal class Link : ISprite, IBoomerangThrower
    {
        private enum Direction { Up, Down, Left, Right }

        private Direction currentDirection = Direction.Down;
        private bool isAttacking = false;
        private bool boomerangInFlight = false;
        private float damageTimer = 0f;

        // --- Movement sprites ---
        private readonly AnimatedSprite downSprite;
        private readonly AnimatedSprite upSprite;
        private readonly AnimatedSprite horizontalSprite;

        // --- Sword swing sprites ---
        private readonly OneShotAnimation swordDown;
        private readonly OneShotAnimation swordUp;
        private readonly OneShotAnimation swordHorizontal;

        // Every sprite that can draw Link's body, kept on the same position and tint
        private readonly Sprite[] bodySprites;

        // --- Arrows, boomerangs and bombs Link has used ---
        private readonly Texture2D texture;
        private readonly SpriteBatch spriteBatch;
        private readonly Rectangle screenBounds;
        private readonly List<IProjectile> projectiles = new List<IProjectile>();

        private readonly IController controller;

        private const float WalkFrameSpeed = 0.15f;
        private const float SwingFrameSpeed = 0.06f;
        private const float MoveSpeed = 3f;

        private static readonly Point BoomerangSourceLocation = new(64, 179);
        private static readonly Point BoomerangSpriteSize = new(9, 8);
        private const int BoomerangFrames = 3;
        private const float BoomerangFrameSpeed = 0.1f;

        private const float TileSize = 64f; // one 16px tile at the 4x sprite scale
        private static readonly Vector2 BombTileOffset = new(16, 4); // centres the 8x14 bomb inside its tile

        private const float DamageDuration = 1f;
        private const float DamageFlashInterval = 0.1f;

        public Link(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, IController controller, Rectangle screenBounds)
        {
            this.texture = texture;
            this.spriteBatch = spriteBatch;
            this.controller = controller;
            this.screenBounds = screenBounds;

            downSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(1, 1), 2, WalkFrameSpeed);
            upSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(71, 1), 2, WalkFrameSpeed);
            horizontalSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(33, 1), 2, WalkFrameSpeed);

            swordDown = new OneShotAnimation(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 37, 16, 15), new Rectangle(18, 37, 16, 27), new Rectangle(35, 37, 15, 23), new Rectangle(53, 37, 13, 19) },
                new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero },
                SwingFrameSpeed);

            swordUp = new OneShotAnimation(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 99, 16, 16), new Rectangle(18, 87, 16, 28), new Rectangle(37, 88, 12, 27), new Rectangle(54, 96, 12, 19) },
                new[] { Vector2.Zero, new Vector2(0, -12), new Vector2(0, -11), new Vector2(0, -3) },
                SwingFrameSpeed);

            swordHorizontal = new OneShotAnimation(texture, spriteBatch, startPosition,
                new[] { new Rectangle(1, 68, 15, 15), new Rectangle(18, 68, 27, 15), new Rectangle(46, 68, 23, 15), new Rectangle(70, 67, 19, 16) },
                new[] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero },
                SwingFrameSpeed);

            bodySprites = new Sprite[] { downSprite, upSprite, horizontalSprite, swordDown, swordUp, swordHorizontal };
        }

        public Vector2 Position => ActiveSprite.Position;

        public void SwordAttack()
        {
            if (isAttacking) return;

            isAttacking = true;
            ActiveSword.Play();
        }

        public void FireArrow()
        {
            if (isAttacking) return;

            projectiles.Add(new Arrow(texture, spriteBatch, Position, DirectionVector, screenBounds));
        }

        // Only one boomerang can be out at a time; Link can throw again once it comes back
        public void ThrowBoomerang()
        {
            if (isAttacking || boomerangInFlight) return;

            AnimatedSprite boomerangSprite = new AnimatedSprite(texture, spriteBatch, Position, BoomerangSourceLocation, BoomerangFrames, BoomerangFrameSpeed);
            boomerangSprite.SetSpriteSize(BoomerangSpriteSize);

            projectiles.Add(new Boomerang(boomerangSprite, Position, DirectionVector, this));
            boomerangInFlight = true;
        }

        public void OnBoomerangReturned()
        {
            boomerangInFlight = false;
        }

        // Drops a bomb on the tile in front of Link
        public void PlaceBomb()
        {
            if (isAttacking) return;

            Vector2 bombPosition = Position + DirectionVector * TileSize + BombTileOffset;
            projectiles.Add(new Bomb(texture, spriteBatch, bombPosition));
        }

        // Link flashes while hurt and can't be hurt again until the flashing stops
        public void TakeDamage()
        {
            if (damageTimer > 0) return;

            damageTimer = DamageDuration;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 movement = controller.UpdateMovement() * MoveSpeed;

            if (isAttacking)
            {
                ActiveSword.Update(gameTime);
                if (!ActiveSword.IsPlaying)
                    isAttacking = false;
            }
            else
            {
                if (movement.Y < 0) currentDirection = Direction.Up;
                else if (movement.Y > 0) currentDirection = Direction.Down;
                else if (movement.X < 0) currentDirection = Direction.Left;
                else if (movement.X > 0) currentDirection = Direction.Right;

                ActiveSprite.UpdateAnimation(gameTime, movement);
            }

            foreach (IProjectile projectile in projectiles)
                projectile.Update(gameTime);
            projectiles.RemoveAll(p => p.IsFinished);

            Vector2 currentPos = isAttacking ? ActiveSword.Position : ActiveSprite.Position;
            Color tint = UpdateDamageTint(gameTime);
            foreach (Sprite sprite in bodySprites)
            {
                sprite.Position = currentPos;
                sprite.SetColor(tint);
            }

            bool facingLeft = currentDirection == Direction.Left;
            horizontalSprite.SetEffects(facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            swordHorizontal.SetEffects(facingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }

        // Counts down the hurt timer and returns the colour to draw Link in this frame
        private Color UpdateDamageTint(GameTime gameTime)
        {
            if (damageTimer <= 0) return Color.White;

            damageTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool flashOn = damageTimer > 0 && (int)(damageTimer / DamageFlashInterval) % 2 == 0;

            return flashOn ? Color.Red : Color.White;
        }

        private Vector2 DirectionVector => currentDirection switch
        {
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1),
            Direction.Left => new Vector2(-1, 0),
            _ => new Vector2(1, 0)
        };

        private AnimatedSprite ActiveSprite => currentDirection switch
        {
            Direction.Up => upSprite,
            Direction.Down => downSprite,
            _ => horizontalSprite
        };

        private OneShotAnimation ActiveSword => currentDirection switch
        {
            Direction.Up => swordUp,
            Direction.Down => swordDown,
            _ => swordHorizontal
        };

        public void Draw(Texture2D texture)
        {
            if (isAttacking)
                ActiveSword.Draw(texture);
            else
                ActiveSprite.Draw(texture);

            foreach (IProjectile projectile in projectiles)
                projectile.Draw(texture);
        }
    }
}
