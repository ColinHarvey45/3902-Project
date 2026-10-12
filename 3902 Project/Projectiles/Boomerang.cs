using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectiles
{
    // Flies out in a straight line, then turns back and flies to whoever threw it until they catch it
    internal sealed class Boomerang : IProjectile
    {
        private const float Speed = 250f; // pixels per second

        // How long it flies out before turning back; it will also turn back when it hits something once collisions are added
        private const float TravelTime = 1.5f;

        // How close it has to come back to its thrower to count as caught
        private const float CatchDistance = 8f;

        private readonly ISprite sprite;
        private Vector2 position; // the middle of the boomerang
        private readonly Vector2 velocity;
        private readonly IBoomerangThrower thrower;

        private float timeFlyingOut;
        private bool isReturning;

        public bool IsFinished { get; private set; }
        public ProjectileOwner Owner { get; }

        public Boomerang(ISprite sprite, Vector2 spawnPosition, Vector2 direction, IBoomerangThrower thrower, ProjectileOwner owner)
        {
            this.sprite = sprite;
            this.position = spawnPosition;
            this.velocity = direction * Speed;
            this.thrower = thrower;
            Owner = owner;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isReturning)
            {
                position += velocity * deltaTime;
                timeFlyingOut += deltaTime;

                if (timeFlyingOut >= TravelTime)
                {
                    isReturning = true;
                }
            }
            else
            {
                Vector2 returnDirection = thrower.Center - position;

                if (returnDirection.Length() < CatchDistance)
                {
                    IsFinished = true;
                    return;
                }

                returnDirection.Normalize();
                position += returnDirection * Speed * deltaTime;
            }

            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }
    }
}
