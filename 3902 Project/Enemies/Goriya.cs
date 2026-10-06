using Animation;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Goriya : Enemy, IBoomerangThrower
    {

        private const float GoriyaWalkSpeed = 0.15f;
        private const float GoriyaAttackSpeed = 0.1f;
        private static readonly Point GoriyaSourceRect = new(224, 11);
        private static readonly Point GoriyaSpriteSize = new(16, 17);
        private const int GoriyaFrames = 2;
        private bool isAttacking = false;
        private Boomerang activeBoomerang = null;
        private AnimatedSprite boomerangSprite;
        private GoriyaState goriyaState;
        private Sprite downSprite;
        private Sprite upSprite;
        private AnimatedSprite horizontalSprite;


        public Vector2 Position { get => goriyaState.currentPosition; }


        public Goriya(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, GoriyaSourceRect, GoriyaFrames, GoriyaWalkSpeed)
        {

            downSprite = new Sprite(texture, spriteBatch, startPosition, new Point(224, 11));
            upSprite = new Sprite(texture, spriteBatch, startPosition, new Point(240, 11));
            horizontalSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(256, 11), 2, GoriyaWalkSpeed);

            this.boomerangSprite = new AnimatedSprite(texture, spriteBatch, startPosition, new Point(290, 15), 3, GoriyaAttackSpeed);
            boomerangSprite.SetSpriteSize(new Point(9, 16));

            goriyaState = new GoriyaState(upSprite, downSprite, horizontalSprite, boomerangSprite);

            this.enemySpriteAnim.SetSpriteSize(GoriyaSpriteSize);
        }

        // Will need to move attacking logic into state class
        public override void Update(GameTime gameTime)
        {

            if (isVisible)
            {

                if (isAttacking)
                {
                    enemySpriteAnim?.UpdateAnimation(gameTime, new Vector2(0.001f, 0f));
                    activeBoomerang?.Update(gameTime);
                }
                else
                {
                    Vector2 movement = GetNextMovement(gameTime);
                    goriyaState.ChangeDirection(movement);
                    goriyaState.Update(gameTime, movement);
                    boomerangSprite.Position = goriyaState.currentPosition;

                    if (timer == 0f && RandomGenerator.Next(0, 50) < 20)
                    {
                        ThrowBoomerang();
                    }

                }
            }

        }

        public void ThrowBoomerang()
        {
            if (isAttacking) return;

            isAttacking = true;

            Vector2 launchDirection = currentDirection;
            Vector2 spawnOffset = currentDirection * 24f;
            Vector2 spawnPos = goriyaState.currentPosition + spawnOffset;

            activeBoomerang = new Boomerang(boomerangSprite, spawnPos, launchDirection, this);
        }

        public void OnBoomerangReturned()
        {
            isAttacking = false;
            activeBoomerang = null;
        }

        public override void Draw()
        {
            if (isVisible)
            {
                goriyaState.activeSprite.Draw(texture);

                if (isAttacking)
                {
                    activeBoomerang?.Draw(texture);
                }
            }
        }

    }
}


namespace Enemies
{
    internal class GoriyaState : EnemyState
    {
        private readonly Sprite upSprite;
        private readonly Sprite downSprite;
        private readonly AnimatedSprite horizontalAnimation;
        private readonly AnimatedSprite attackAnimation;
        public Vector2 currentPosition { get; private set; }

        public GoriyaState(Sprite up, Sprite down, AnimatedSprite horizontal, AnimatedSprite attack)
        {
            this.upSprite = up;
            this.downSprite = down;
            this.horizontalAnimation = horizontal;
            this.attackAnimation = attack;

            this.activeSprite = downSprite;
        }

        public override void ChangeDirection(Vector2 movement)
        {
            if (movement.Equals(Up))
            {
                activeSprite = upSprite;
            }
            else if (movement.Equals(Down))
            {
                activeSprite = downSprite;
            }
            else if (movement.Equals(Left))
            {
                activeSprite = horizontalAnimation;
                activeSprite.SetEffects(SpriteEffects.FlipHorizontally);
            }
            else if (movement.Equals(Right))
            {
                activeSprite = horizontalAnimation;
                activeSprite.SetEffects(SpriteEffects.None);
            }
        }

        public override void Update(GameTime gameTime, Vector2 movement)
        {

            base.Update(gameTime, movement);

            currentPosition = activeSprite.Position;
            upSprite.Position = currentPosition;
            downSprite.Position = currentPosition;
            horizontalAnimation.Position = currentPosition;

            if (movement.Equals(Up) || movement.Equals(Down))
            {
                this.activeSprite.Flip(gameTime);
            }
        }
    }
}
