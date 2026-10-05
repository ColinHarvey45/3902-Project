using Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectiles
{
    internal class Boomerang
    {
        private readonly AnimatedSprite sprite;
        private Vector2 position;
        private readonly Vector2 velocity;
        private readonly Enemies.Goriya owner;

        private float stateTimer = 0f;

        // will need to be changed to return once it hits either Link or an obstacle
        private readonly float travelDuration = 1.5f;
        private bool isReturning = false;
        private readonly float speed = 250f;

        public bool IsDead { get; private set; } = false;

        public Boomerang(AnimatedSprite sprite, Vector2 spawnPosition, Vector2 direction, Enemies.Goriya owner)
        {
            this.sprite = sprite;
            this.position = spawnPosition;
            this.velocity = direction * speed;
            this.owner = owner;

            this.sprite.Position = spawnPosition;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDead) return;

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
                    IsDead = true;
                    owner.OnBoomerangReturned();
                    return;
                }

                returnDirection.Normalize();
                position += returnDirection * speed * deltaTime;
            }

            sprite.Position = position;
            sprite.UpdateAnimation(gameTime, new Vector2(0.1f, 0f));
        }

        public void Draw(Texture2D texture)
        {
            sprite.Draw(texture);
        }
    }
}
