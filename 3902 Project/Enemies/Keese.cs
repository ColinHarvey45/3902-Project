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
    internal class Keese : Enemy
    {

        private const float KeeseWalkSpeed = 0.15f;
        private static readonly Point KeeseSourceRect = new(183, 14);
        private static readonly Point KeeseSpriteSize = new(16, 16);
        private const int KeeseFrames = 2;
        private KeeseState keeseState;

        public Keese(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, KeeseSourceRect, KeeseFrames, KeeseWalkSpeed)
        {

            keeseState = new KeeseState(enemySpriteAnim);

            this.enemySpriteAnim.SetSpriteSize(KeeseSpriteSize);
            this.directions = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1) };
        }

    }
}


namespace Enemies
{
    internal class KeeseState : EnemyState
    {
        private readonly AnimatedSprite moveAnimation;

        public KeeseState(AnimatedSprite movement)
        {
            this.activeSprite = movement;
        }

        public override void ChangeDirection(Vector2 movement) { }

        public override void Update(GameTime gameTime, Vector2 movement)
        {
            base.Update(gameTime, movement);
        }
    }
}