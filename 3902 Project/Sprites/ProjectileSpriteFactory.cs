using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Sprites
{
    // Builds the sprites for things that are thrown, fired or placed: arrows, boomerangs, bombs
    internal sealed class ProjectileSpriteFactory
    {
        private const float BoomerangFrameTime = 0.1f;
        private const float ExplosionFrameTime = 0.1f;

        // --- Link sheet ---
        private static readonly Rectangle ArrowUpFrame = new(3, 175, 5, 16);
        private static readonly Rectangle ArrowRightFrame = new(10, 180, 16, 5);
        private static readonly Rectangle[] LinkBoomerangFrames = [new(64, 179, 9, 8), new(73, 179, 9, 8), new(82, 179, 9, 8)];
        private static readonly Rectangle BombFrame = new(129, 175, 8, 14);
        private static readonly Rectangle[] ExplosionFrames = [new(138, 175, 16, 16), new(155, 175, 16, 16), new(172, 175, 16, 16)];

        // Shifts the 16px-wide smoke cloud left so it is centred on the 8px-wide bomb
        private static readonly Vector2 ExplosionOffset = new(-4, 0);

        // --- Enemy sheet ---
        private static readonly Rectangle[] GoriyaBoomerangFrames = [new(290, 15, 9, 8), new(299, 15, 9, 8), new(308, 15, 9, 8)];

        // --- Bosses sheet: Aquamentus's fireball cycles through four colours ---
        private const float FireballFrameTime = 0.05f;
        private static readonly Rectangle[] FireballFrames = [new(101, 4, 8, 10), new(110, 4, 8, 10), new(119, 4, 8, 10), new(128, 4, 8, 10)];

        private Texture2D linkTexture;
        private Texture2D enemyTexture;
        private Texture2D bossTexture;

        public static ProjectileSpriteFactory Instance { get; } = new ProjectileSpriteFactory();

        private ProjectileSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            linkTexture = content.Load<Texture2D>("TLOZLink-transparent2");
            enemyTexture = content.Load<Texture2D>("TLOZDungeonEnemies-transparent");
            bossTexture = content.Load<Texture2D>("TLOZBosses-transparent");
        }

        // Arrows and boomerangs are drawn centred on their position, so they can be fired from
        // the middle of whoever fires them and fly back to it
        public ISprite CreateArrowSprite(Direction direction)
        {
            SpriteFrame frame = direction switch
            {
                Direction.Up => Centred(ArrowUpFrame),
                Direction.Down => Centred(ArrowUpFrame, SpriteEffects.FlipVertically),
                Direction.Left => Centred(ArrowRightFrame, SpriteEffects.FlipHorizontally),
                _ => Centred(ArrowRightFrame)
            };

            return new Sprite(linkTexture, frame);
        }

        public ISprite CreateLinkBoomerangSprite()
        {
            return new Sprite(linkTexture, Centred(LinkBoomerangFrames), BoomerangFrameTime, loops: true);
        }

        public ISprite CreateBombSprite()
        {
            return new Sprite(linkTexture, new SpriteFrame(BombFrame));
        }

        public ISprite CreateExplosionSprite()
        {
            SpriteFrame[] frames = new SpriteFrame[ExplosionFrames.Length];
            for (int i = 0; i < frames.Length; i++)
                frames[i] = new SpriteFrame(ExplosionFrames[i], SpriteEffects.None, ExplosionOffset);

            return new Sprite(linkTexture, frames, ExplosionFrameTime, loops: false);
        }

        public ISprite CreateGoriyaBoomerangSprite()
        {
            return new Sprite(enemyTexture, Centred(GoriyaBoomerangFrames), BoomerangFrameTime, loops: true);
        }

        public ISprite CreateFireballSprite()
        {
            return new Sprite(bossTexture, SpriteFrame.FromSources(FireballFrames), FireballFrameTime, loops: true);
        }

        // Shifted up and left by half its size, so it is drawn centred on the position it is given
        private static SpriteFrame Centred(Rectangle source, SpriteEffects effects = SpriteEffects.None)
        {
            return new SpriteFrame(source, effects, new Vector2(-source.Width / 2f, -source.Height / 2f));
        }

        private static SpriteFrame[] Centred(Rectangle[] sources)
        {
            SpriteFrame[] frames = new SpriteFrame[sources.Length];
            for (int i = 0; i < frames.Length; i++)
                frames[i] = Centred(sources[i]);

            return frames;
        }
    }
}
