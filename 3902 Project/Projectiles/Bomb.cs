using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Projectiles
{
    // Sits where it was placed until the fuse runs out, then plays its explosion once
    internal sealed class Bomb : IProjectile
    {
        private const float FuseTime = 1f;

        private readonly Vector2 position;
        private readonly ISprite bombSprite;
        private readonly ISprite explosionSprite;
        private float fuseTimer;
        private bool isExploding;

        public bool IsFinished => explosionSprite.IsFinished;

        public Bomb(Vector2 position)
        {
            this.position = position;
            bombSprite = ProjectileSpriteFactory.Instance.CreateBombSprite();
            explosionSprite = ProjectileSpriteFactory.Instance.CreateExplosionSprite();
        }

        public void Update(GameTime gameTime)
        {
            if (isExploding)
            {
                explosionSprite.Update(gameTime);
                return;
            }

            fuseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            isExploding = fuseTimer >= FuseTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ISprite currentSprite = isExploding ? explosionSprite : bombSprite;
            currentSprite.Draw(spriteBatch, position);
        }
    }
}
