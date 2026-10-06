using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds every enemy sprite from the dungeon enemies sheet
    internal class EnemySpriteFactory
    {
        private const float WalkFrameTime = 0.15f;

        private static readonly Rectangle StalfosFrame = new(1, 59, 16, 16);
        private static readonly Rectangle[] ZolFrames = [new(77, 11, 16, 16), new(93, 11, 16, 16)];
        private static readonly Rectangle[] GelFrames = [new(0, 15, 9, 16), new(9, 15, 9, 16)];
        private static readonly Rectangle[] KeeseFrames = [new(183, 14, 16, 16), new(199, 14, 16, 16)];

        private static readonly Rectangle GoriyaDownFrame = new(224, 11, 16, 16);
        private static readonly Rectangle GoriyaUpFrame = new(240, 11, 16, 16);
        private static readonly Rectangle[] GoriyaRightFrames = [new(256, 11, 16, 16), new(274, 11, 16, 16)];

        private Texture2D texture;

        public static EnemySpriteFactory Instance { get; } = new EnemySpriteFactory();

        private EnemySpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZDungeonEnemies-transparent");
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

        private Sprite Walking(SpriteFrame[] frames)
        {
            return new Sprite(texture, frames, WalkFrameTime, loops: true);
        }
    }
}
