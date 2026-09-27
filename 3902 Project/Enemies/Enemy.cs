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
        protected Vector2 currentDirection;
        protected bool isVisible = true;

        protected static readonly Random RandomGenerator = new();

        protected Enemy(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Point sourceRect, int totalFrames, float frameSpeed)
        {
            this.texture = texture;
            this.enemySpriteAnim = new AnimatedSprite(texture, spriteBatch, startPosition, sourceRect, totalFrames, frameSpeed);
        }

        protected Enemy(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition, Point sourceRect)
        {
            this.texture = texture;
            this.enemySprite = new Sprite(texture, spriteBatch, startPosition, sourceRect);
        }

        public virtual Vector2 GetNextMovement(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= 2.0f)
            {
                timer = 0f;
                Vector2[] directions = { new(1, 0), new(-1, 0), new(0, -1), new(0, 1) };
                currentDirection = directions[RandomGenerator.Next(directions.Length)];
            }

            return currentDirection;
        }

        public virtual void Update(GameTime gameTime)
        {
            Vector2 movement = GetNextMovement(gameTime);

            if (enemySpriteAnim != null)
            {
                enemySpriteAnim.UpdateAnimation(gameTime, movement);
            }

            if (enemySprite != null)
            {
                enemySprite.Position += movement;
            }
        }

        public virtual void Draw()
        {
            if (enemySpriteAnim != null)
            {
                enemySpriteAnim.Draw(texture);
            }
            else if (enemySprite != null)
            {
                enemySprite.Draw(texture);
            }
        }
    }
}

