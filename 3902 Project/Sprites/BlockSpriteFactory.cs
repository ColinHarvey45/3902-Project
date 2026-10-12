using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Sprites
{
    // Builds the sprites for blocks, statues, doors, walls and other room pieces
    internal sealed class BlockSpriteFactory
    {
        private const float FireFrameTime = 0.15f;

        // --- Dungeon sheet ---
        private static readonly Rectangle SquareBlockFrame = new(1001, 11, 16, 16);
        private static readonly Rectangle FishStatueFrame = new(1018, 11, 16, 16);
        private static readonly Rectangle DragonStatueFrame = new(1035, 11, 16, 16);
        private static readonly Rectangle BlueGapFrame = new(1018, 28, 16, 16);
        private static readonly Rectangle FloorTileFrame = new(984, 11, 16, 16);
        private static readonly Rectangle BlackTileFrame = new(984, 28, 16, 16);
        private static readonly Rectangle SandTileFrame = new(1001, 28, 16, 16);
        private static readonly Rectangle StairsFrame = new(1035, 28, 16, 16);
        private static readonly Rectangle WhiteBrickFrame = new(984, 45, 16, 16);
        private static readonly Rectangle LadderFrame = new(1001, 45, 16, 16);

        // The room's outer wall, with an empty middle where the floor goes and a coloured gap in each
        // wall that a door piece covers
        private static readonly Rectangle RoomWallsFrame = new(521, 11, 256, 176);

        // A floor of 12 x 7 plain floor tiles, drawn in one go
        private static readonly Rectangle RoomFloorFrame = new(1, 192, 192, 112);

        // The top-wall version of each door piece; the left, right and bottom wall versions are in the rows below it
        private static readonly Rectangle WallFrame = new(815, 11, 32, 32);
        private static readonly Rectangle OpenDoorFrame = new(848, 11, 32, 32);
        private static readonly Rectangle KeyholeLockedDoorFrame = new(881, 11, 32, 32);
        private static readonly Rectangle DiamondLockedDoorFrame = new(914, 11, 32, 32);
        private static readonly Rectangle BombedWallOpeningFrame = new(947, 11, 32, 32);
        private const int DoorRowSpacing = 33;

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
        public ISprite CreateFloorTileSprite() => Still(FloorTileFrame);
        public ISprite CreateBlackTileSprite() => Still(BlackTileFrame);
        public ISprite CreateSandTileSprite() => Still(SandTileFrame);
        public ISprite CreateStairsSprite() => Still(StairsFrame);
        public ISprite CreateWhiteBrickSprite() => Still(WhiteBrickFrame);
        public ISprite CreateLadderSprite() => Still(LadderFrame);

        public ISprite CreateRoomWallsSprite() => Still(RoomWallsFrame);
        public ISprite CreateRoomFloorSprite() => Still(RoomFloorFrame);

        public ISprite CreateWallSprite(Direction side) => Door(WallFrame, side);
        public ISprite CreateOpenDoorSprite(Direction side) => Door(OpenDoorFrame, side);
        public ISprite CreateKeyholeLockedDoorSprite(Direction side) => Door(KeyholeLockedDoorFrame, side);
        public ISprite CreateDiamondLockedDoorSprite(Direction side) => Door(DiamondLockedDoorFrame, side);
        public ISprite CreateBombedWallOpeningSprite(Direction side) => Door(BombedWallOpeningFrame, side);

        public ISprite CreateFireSprite()
        {
            return new Sprite(npcTexture, SpriteFrame.Flicker(FireFrame), FireFrameTime, loops: true);
        }

        private Sprite Still(Rectangle frame)
        {
            return new Sprite(dungeonTexture, new SpriteFrame(frame));
        }

        // Each door piece is drawn differently depending on which wall it sits in
        private Sprite Door(Rectangle topWallFrame, Direction side)
        {
            int row = side switch
            {
                Direction.Up => 0,
                Direction.Left => 1,
                Direction.Right => 2,
                _ => 3
            };

            Rectangle frame = topWallFrame;
            frame.Y += row * DoorRowSpacing;

            return Still(frame);
        }
    }
}
