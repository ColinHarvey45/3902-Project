using Animation;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enemies
{
    internal abstract class EnemyState : IState
    {

        public Sprite activeSprite { get; protected set; }

        protected static readonly Vector2 Up = new Vector2(0, -1);
        protected static readonly Vector2 Down = new Vector2(0, 1);
        protected static readonly Vector2 Left = new Vector2(-1, 0);
        protected static readonly Vector2 Right = new Vector2(1, 0);

        private float flipTimer = 0f;
        private SpriteEffects currentEffect = SpriteEffects.None;

        public abstract void ChangeDirection(Vector2 movement);

        public virtual void Update(GameTime gameTime, Vector2 movement)
        {

            if (activeSprite != null)
            {
                activeSprite.Position += movement;

                if (activeSprite is AnimatedSprite animated)
                {
                    animated.UpdateAnimation(gameTime, movement);
                }
            }
        }

        public virtual void Draw(Texture2D texture)
        {
            activeSprite?.Draw(texture);
        }

        public virtual void TakeDamage(int damage) { }
        public virtual void Attack(GameTime gameTime, Vector2 movement) { }
    }
}
