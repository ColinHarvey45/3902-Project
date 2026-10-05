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
    internal class Zol : Enemy
    {

        private const float ZolWalkSpeed = 0.15f;
        private static readonly Point ZolSourceRect = new(77, 10);
        private static readonly Point ZolSpriteSize = new(16, 16);
        private const int ZolFrames = 2;
        private ZolState zolState;

        public Zol(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, ZolSourceRect, ZolFrames, ZolWalkSpeed)
        {

            zolState = new ZolState(enemySpriteAnim);

            this.enemySpriteAnim.SetSpriteSize(ZolSpriteSize);
        }

    }
}


namespace Enemies
{
    internal class ZolState : EnemyState
    {
        private readonly AnimatedSprite moveAnimation;

        public ZolState(AnimatedSprite movement)
        {
            this.moveAnimation = movement;
            this.activeSprite = moveAnimation;
        }

        public override void ChangeDirection(Vector2 movement) { }

        public override void Update(GameTime gameTime, Vector2 movement) { base.Update(gameTime, movement); }
    }
}
