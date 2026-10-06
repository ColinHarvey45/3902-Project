using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds the sprites for blocks, statues, doors, walls and other room pieces
    internal class BlockSpriteFactory
    {
        private const float FireFrameTime = 0.15f;

        // --- Dungeon sheet ---
        private static readonly Rectangle SquareBlockFrame = new(1001, 11, 16, 16);
        private static readonly Rectangle FishStatueFrame = new(1018, 11, 16, 16);
        private static readonly Rectangle DragonStatueFrame = new(1035, 11, 16, 16);
        private static readonly Rectangle BlueGapFrame = new(1018, 28, 16, 16);
        private static readonly Rectangle StairsFrame = new(1035, 28, 16, 16);
        private static readonly Rectangle WhiteBrickFrame = new(984, 45, 16, 16);
        private static readonly Rectangle LadderFrame = new(1001, 45, 16, 16);
        private static readonly Rectangle WallFrame = new(815, 11, 32, 32);
        private static readonly Rectangle OpenDoorFrame = new(848, 11, 32, 32);
        private static readonly Rectangle KeyholeLockedDoorFrame = new(881, 11, 32, 32);
        private static readonly Rectangle DiamondLockedDoorFrame = new(914, 11, 32, 32);
        private static readonly Rectangle BombedWallOpeningFrame = new(947, 11, 32, 32);

        // --- NPC sheet ---
        private static readonly Rectangle FireFrame = new(52, 11, 16, 16);

        private Texture2D dungeonTexture;
        private Texture2D npcTexture;

        public static BlockSpriteFactory Instance { get; } = new BlockSpriteFactory();

        private BlockSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            dungeonTexture = content.Load<Texture2D>("TLOZDungeon-transparent");
            npcTexture = content.Load<Texture2D>("TLOZNPCs-transparent");
        }

        public ISprite CreateSquareBlockSprite() => Still(SquareBlockFrame);
        public ISprite CreateFishStatueSprite() => Still(FishStatueFrame);
        public ISprite CreateDragonStatueSprite() => Still(DragonStatueFrame);
        public ISprite CreateBlueGapSprite() => Still(BlueGapFrame);
        public ISprite CreateStairsSprite() => Still(StairsFrame);
        public ISprite CreateWhiteBrickSprite() => Still(WhiteBrickFrame);
        public ISprite CreateLadderSprite() => Still(LadderFrame);
        public ISprite CreateWallSprite() => Still(WallFrame);
        public ISprite CreateOpenDoorSprite() => Still(OpenDoorFrame);
        public ISprite CreateKeyholeLockedDoorSprite() => Still(KeyholeLockedDoorFrame);
        public ISprite CreateDiamondLockedDoorSprite() => Still(DiamondLockedDoorFrame);
        public ISprite CreateBombedWallOpeningSprite() => Still(BombedWallOpeningFrame);

        public ISprite CreateFireSprite()
        {
            return new Sprite(npcTexture, SpriteFrame.Flicker(FireFrame), FireFrameTime, loops: true);
        }

        private Sprite Still(Rectangle frame)
        {
            return new Sprite(dungeonTexture, new SpriteFrame(frame));
        }
    }
}
