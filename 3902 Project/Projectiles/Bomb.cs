using Colliders;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;

namespace Projectiles
{
    // Sits where it was placed until the fuse runs out, then plays its explosion once,
    // hurting every enemy caught in the blast
    internal sealed class Bomb : IProjectile
    {
        private const float FuseTime = 1f;

        // The explosion's smoke cloud is one tile square, drawn 16 pixels left of the bomb (see
        // ProjectileSpriteFactory); the blast is two tiles square, centred on the cloud
        private const int BlastSize = 2 * RoomLayout.TileSize;
        private static readonly Point CloudCenter = new(-16 + RoomLayout.TileSize / 2, RoomLayout.TileSize / 2);
        private static readonly Point BlastOffset = new(CloudCenter.X - BlastSize / 2, CloudCenter.Y - BlastSize / 2);

        private readonly Vector2 position;
        private readonly ISprite bombSprite;
        private readonly ISprite explosionSprite;
        private float fuseTimer;
        private bool isExploding;

        public bool IsFinished => explosionSprite.IsFinished;
        public int Damage { get; } = 1;
        public ProjectileOwner Owner => ProjectileOwner.Link;
        public bool HitsEveryEnemyInRange => true;

        // None until it explodes, so nothing is hurt while the fuse burns
        public ICollider Collider { get; set; }

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
            if (fuseTimer < FuseTime)
                return;

            isExploding = true;
            Collider = new BoxCollider(new Rectangle(0, 0, BlastSize, BlastSize), BlastOffset);
            Collider.Update(position);
        }

        public void Hit() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            ISprite currentSprite = isExploding ? explosionSprite : bombSprite;
            currentSprite.Draw(spriteBatch, position);
        }
    }
}
