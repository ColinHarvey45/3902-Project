using Colliders;
using Enemies;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;
using Sprites;

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
        private const int ColliderWidth = (int)(8 * Sprite.Scale);
        private const int ColliderHeight = (int)(8 * Sprite.Scale);
        private static readonly Point ColliderOffset = Point.Zero;

        private readonly ISprite sprite;
        private Vector2 position; // the middle of the boomerang
        private readonly Vector2 velocity;
        private readonly IBoomerangThrower thrower;

        private float timeFlyingOut;
        private bool isReturning;
        public ICollider collider { get; set; }

        public bool IsFinished { get; private set; }
        public int Damage { get; } = 1;
        public ProjectileOwner Owner { get; }

        public Boomerang(ISprite sprite, Vector2 spawnPosition, Vector2 direction, IBoomerangThrower thrower, ProjectileOwner owner)
        {
            this.sprite = sprite;
            this.position = spawnPosition;
            this.velocity = direction * Speed;
            this.thrower = thrower;
            Owner = owner;

            collider = new BoxCollider(
                new Rectangle((int)spawnPosition.X, (int)spawnPosition.Y, ColliderWidth, ColliderHeight),
                ColliderOffset);
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
                    isReturning = true;
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

            collider.Update(position);
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position);
        }

        // Turns back when it hits something
        public void Hit()
        {
            isReturning = true;
        }
    }
}