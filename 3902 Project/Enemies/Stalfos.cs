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
    internal class Stalfos : Enemy
    {
        private static readonly Point StalfosSourceRect = new(1, 58);
        private static readonly Point StalfosSpriteSize = new(16, 16);
        private readonly StalfosState stalfosState;

        public Stalfos(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, StalfosSourceRect)
        {

            stalfosState = new StalfosState(enemySprite);

            this.enemySprite.SetSpriteSize(StalfosSpriteSize);
        }


        //public override void Update(GameTime gameTime)
        //{

        //    if (isVisible)
        //    {

        //        Vector2 movement = GetNextMovement(gameTime);

        //        enemySpriteAnim?.UpdateAnimation(gameTime, movement);

        //        if (enemySprite != null)
        //        {
        //            enemySprite.Position += movement;
        //            Flip(gameTime);
        //        }

        //    }
        //}

        public override void Update(GameTime gameTime)
        {

            if (isVisible)
            {
                Vector2 movement = GetNextMovement(gameTime);
                stalfosState.Update(gameTime, movement);
            }

        }

    }
}

namespace Enemies
{
    internal class StalfosState : EnemyState
    {
        private readonly Sprite moveSprite;

        public StalfosState(Sprite moveSprite)
        {
            this.activeSprite = moveSprite;
        }

        public override void ChangeDirection(Vector2 movement) { }

        public override void Update(GameTime gameTime, Vector2 movement)
        {

            this.activeSprite.Flip(gameTime);
            base.Update(gameTime, movement);

        }
    }
}