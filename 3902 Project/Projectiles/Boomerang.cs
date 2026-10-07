using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectiles
{
    internal sealed class Boomerang : IProjectile
    {
        private readonly ISprite sprite;
        private Vector2 position;
        private readonly Vector2 velocity;
        private readonly IBoomerangThrower owner;

        private float stateTimer;

        // will need to be changed to return once it hits either Link or an obstacle
        private readonly float travelDuration = 1.5f;
        private bool isReturning;
        private readonly float speed = 250f;

        public bool IsFinished { get; private set; }

        public Boomerang(ISprite sprite, Vector2 spawnPosition, Vector2 direction, IBoomerangThrower owner)
        {
            this.sprite = sprite;
            this.position = spawnPosition;
            this.velocity = direction * speed;
            this.owner = owner;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stateTimer += deltaTime;

            if (!isReturning)
            {
                position += velocity * deltaTime;

                if (stateTimer >= travelDuration)
                {
                    isReturning = true;
                }
            }
            else
            {
                Vector2 targetPos = owner.Position;
                Vector2 returnDirection = targetPos - position;

                if (returnDirection.Length() < 8f)
                {
                    IsFinished = true;
                    owner.OnBoomerangReturned();
                    return;
                }

                returnDirection.Normalize();
                position += returnDirection * speed * deltaTime;
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
