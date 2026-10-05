using Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal abstract class Enemy
    {
        protected Texture2D texture;
        protected AnimatedSprite enemySpriteAnim;
        protected Sprite enemySprite;
        protected float timer = 0f;
        private float flipTimer = 0f;
        public Vector2 currentDirection { get; private set; }
        private SpriteEffects currentEffect = SpriteEffects.None;
        protected bool isVisible = true;
        protected Vector2[] directions = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1) };

        protected static readonly Random RandomGenerator = new();

        protected Enemy(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Point sourceRect, int totalFrames, float frameSpeed)
        {
            this.texture = texture;
            this.enemySpriteAnim = new AnimatedSprite(texture, spriteBatch, startPosition, sourceRect, totalFrames, frameSpeed);
            this.currentDirection = directions[RandomGenerator.Next(directions.Length)];
        }

        protected Enemy(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Point sourceRect)
        {
            this.texture = texture;
            this.enemySprite = new Sprite(texture, spriteBatch, startPosition, sourceRect);
            this.currentDirection = directions[RandomGenerator.Next(directions.Length)];
        }

        public virtual Vector2 GetNextMovement(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 2.0f)
            {
                timer = 0f;
                currentDirection = directions[RandomGenerator.Next(directions.Length)];
            }

            return currentDirection;
        }

        public void SetVisibility(bool visible)
        {
            this.isVisible = visible;
        }

        public void Flip(GameTime gameTime)
        {
            flipTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (flipTimer >= 0.15f)
            {
                flipTimer = 0f;

                if (currentEffect == SpriteEffects.None)
                {
                    currentEffect = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    currentEffect = SpriteEffects.None;
                }

                this.enemySprite.SetEffects(currentEffect);
            }
        }

        public virtual void Update(GameTime gameTime)
        {

            if (isVisible)
            {

                Vector2 movement = GetNextMovement(gameTime);

                enemySpriteAnim?.UpdateAnimation(gameTime, movement);

                if (enemySprite != null)
                {
                    enemySprite.Position += movement;
                }

            }
        }

        public virtual void Draw()
        {
            
            if (isVisible)
            {

                if (enemySpriteAnim != null)
                {
                    enemySpriteAnim.Draw(texture);
                }
                else
                {
                    enemySprite?.Draw(texture);
                }

            }

        }
    }
}

