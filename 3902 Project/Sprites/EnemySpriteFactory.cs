using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds every enemy sprite: regular enemies from the dungeon enemies sheet, bosses from the bosses sheet
    internal sealed class EnemySpriteFactory
    {
        private const float WalkFrameTime = 0.15f;
        private const float BossWalkFrameTime = 0.3f;

        private static readonly Rectangle StalfosFrame = new(1, 59, 16, 16);
        private static readonly Rectangle[] ZolFrames = [new(77, 11, 16, 16), new(93, 11, 16, 16)];
        private static readonly Rectangle[] GelFrames = [new(0, 15, 9, 16), new(9, 15, 9, 16)];
        private static readonly Rectangle[] KeeseFrames = [new(183, 14, 16, 16), new(199, 14, 16, 16)];

        private static readonly Rectangle GoriyaDownFrame = new(224, 11, 16, 16);
        private static readonly Rectangle GoriyaUpFrame = new(240, 11, 16, 16);
        private static readonly Rectangle[] GoriyaRightFrames = [new(256, 11, 16, 16), new(274, 11, 16, 16)];

        private static readonly Rectangle[] WallmasterFrames = [new(393, 11, 16, 16), new(409, 11, 16, 16)];
        private static readonly Rectangle BladeTrapFrame = new(164, 59, 16, 16);

        // --- Bosses sheet ---
        // On the sheet the mouth-open pair comes first, then the mouth-closed pair
        private static readonly Rectangle[] AquamentusFrames = [new(51, 1, 24, 32), new(76, 1, 24, 32)];
        private static readonly Rectangle[] AquamentusMouthOpenFrames = [new(1, 1, 24, 32), new(26, 1, 24, 32)];

        private Texture2D texture;
        private Texture2D bossTexture;

        public static EnemySpriteFactory Instance { get; } = new EnemySpriteFactory();

        private EnemySpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZDungeonEnemies-transparent");
            bossTexture = content.Load<Texture2D>("TLOZBosses-transparent");
        }

        public ISprite CreateStalfosSprite()
        {
            return Walking(SpriteFrame.Flicker(StalfosFrame));
        }

        public ISprite CreateZolSprite()
        {
            return Walking(SpriteFrame.FromSources(ZolFrames));
        }

        public ISprite CreateGelSprite()
        {
            return Walking(SpriteFrame.FromSources(GelFrames));
        }

        public ISprite CreateKeeseSprite()
        {
            return Walking(SpriteFrame.FromSources(KeeseFrames));
        }

        // Goriya has a different look for each way it can walk
        public ISprite CreateGoriyaSprite(Vector2 direction)
        {
            if (direction.Y < 0)
                return Walking(SpriteFrame.Flicker(GoriyaUpFrame));
            if (direction.Y > 0)
                return Walking(SpriteFrame.Flicker(GoriyaDownFrame));
            if (direction.X < 0)
                return Walking(SpriteFrame.FromSources(GoriyaRightFrames, SpriteEffects.FlipHorizontally));

            return Walking(SpriteFrame.FromSources(GoriyaRightFrames));
        }

        // A hand that opens and closes as it moves
        public ISprite CreateWallmasterSprite()
        {
            return Walking(SpriteFrame.FromSources(WallmasterFrames));
        }

        public ISprite CreateBladeTrapSprite()
        {
            return new Sprite(texture, new SpriteFrame(BladeTrapFrame));
        }

        public ISprite CreateAquamentusSprite()
        {
            return new Sprite(bossTexture, SpriteFrame.FromSources(AquamentusFrames), BossWalkFrameTime, loops: true);
        }

        // The same walk, with its mouth open while it breathes fireballs
        public ISprite CreateAquamentusMouthOpenSprite()
        {
            return new Sprite(bossTexture, SpriteFrame.FromSources(AquamentusMouthOpenFrames), BossWalkFrameTime, loops: true);
        }

        private Sprite Walking(SpriteFrame[] frames)
        {
            return new Sprite(texture, frames, WalkFrameTime, loops: true);
        }
    }
}
