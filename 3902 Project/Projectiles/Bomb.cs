using Animation;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectiles
{
    // Sits where it was placed until the fuse runs out, then plays its explosion once
    internal class Bomb : IProjectile
    {
        private static readonly Point BombSourceLocation = new(129, 175);
        private static readonly Point BombSpriteSize = new(8, 14);

        private static readonly Rectangle[] ExplosionFrames =
        {
            new Rectangle(138, 175, 16, 16),
            new Rectangle(155, 175, 16, 16),
            new Rectangle(172, 175, 16, 16)
        };

        // Shifts the 16px-wide smoke cloud left so it is centred on the 8px-wide bomb
        private static readonly Vector2 ExplosionOffset = new(-4, 0);

        private const float FuseTime = 1f;
        private const float ExplosionFrameSpeed = 0.1f;

        private readonly Sprite bombSprite;
        private readonly OneShotAnimation explosion;
        private float fuseTimer = 0f;
        private bool isExploding = false;

        public bool IsFinished { get; private set; } = false;

        public Bomb(Texture2D texture, SpriteBatch spriteBatch, Vector2 position)
        {
            bombSprite = new Sprite(texture, spriteBatch, position, BombSourceLocation);
            bombSprite.SetSpriteSize(BombSpriteSize);

            Vector2[] explosionOffsets = { ExplosionOffset, ExplosionOffset, ExplosionOffset };
            explosion = new OneShotAnimation(texture, spriteBatch, position, ExplosionFrames, explosionOffsets, ExplosionFrameSpeed);
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            if (isExploding)
            {
                explosion.Update(gameTime);
                IsFinished = !explosion.IsPlaying;
            }
            else
            {
                fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (fuseTimer >= FuseTime)
                {
                    isExploding = true;
                    explosion.Play();
                }
            }
        }

        public void Draw(Texture2D texture)
        {
            if (IsFinished) return;

            if (isExploding)
                explosion.Draw(texture);
            else
                bombSprite.Draw(texture);
        }
    }
}
