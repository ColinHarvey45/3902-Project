using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds the sprites for the HUD: its frame and labels, the digits and hearts it counts with, and one
    // room on the dungeon map. Also the pieces of the pause screen, which is drawn from the same sheet
    internal sealed class HudSpriteFactory
    {
        // The whole HUD frame, 256 x 56: its rupee, key and bomb pictures, the B and A boxes and "-LIFE-".
        // It also has stand-ins (counters, item pictures, hearts) that the HUD covers up and draws over
        private static readonly Rectangle FrameSource = new(258, 11, 256, 56);

        // "LEVEL-", six letters, to be followed by the level's number
        private static readonly Rectangle LevelLabelSource = new(584, 1, 48, 8);

        // One room on the dungeon map: a blue block 7 x 3 (the map leaves a 1 pixel gap around each)
        private static readonly Rectangle MapRoomSource = new(663, 108, 7, 3);

        // The pause screen's two panels, each 256 x 88: the items Link can use (with boxes for the equipped item
        // and the others, and stand-in holes where the pictures go), and the dungeon map (an orange sheet of
        // paper with a checkerboard where the rooms go, and the labels for the map and compass)
        private static readonly Rectangle InventoryPanelSource = new(1, 11, 256, 88);
        private static readonly Rectangle MapPanelSource = new(258, 112, 256, 88);

        // The highlight round the chosen item is four 3 x 3 corners, each an L, in blue
        private const int CursorCornerSize = 3;
        private static readonly Point CursorTopLeft = new(536, 137);
        private static readonly Point CursorTopRight = new(549, 137);
        private static readonly Point CursorBottomLeft = new(536, 150);
        private static readonly Point CursorBottomRight = new(549, 150);

        // A row of 16 tiles, 8 x 8 and 9 pixels apart, for a room on the dungeon map: black with an orange rim,
        // with a gap in the rim for each doorway. Which tile is which: add 8 for a doorway at the top,
        // 4 at the bottom, 2 on the left and 1 on the right (0 is a room with no doorways at all)
        private const int RoomTilesLeft = 519;
        private const int RoomTilesTop = 108;
        private const int RoomTileSize = 8;
        private const int RoomTileSpacing = 9;

        // A row of 8 x 8 glyphs, 9 pixels apart: X, the digits 0-9, A, then an empty, a half and a full heart
        private const int GlyphSize = 8;
        private const int GlyphSpacing = 9;
        private const int GlyphsLeft = 519;
        private const int GlyphsTop = 117;
        private const int MultiplySignIndex = 0;
        private const int FirstDigitIndex = 1;
        private const int EmptyHeartIndex = 12;
        private const int FullHeartIndex = 14;

        private Texture2D texture;

        public static HudSpriteFactory Instance { get; } = new HudSpriteFactory();

        private HudSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZHUDandPause-transparent");
        }

        public ISprite CreateFrameSprite() => Still(FrameSource);
        public ISprite CreateLevelLabelSprite() => Still(LevelLabelSource);
        public ISprite CreateMapRoomSprite() => Still(MapRoomSource);

        public ISprite CreateInventoryPanelSprite() => Still(InventoryPanelSource);
        public ISprite CreateMapPanelSprite() => Still(MapPanelSource);

        // One corner of the highlight round the chosen item
        public ISprite CreateCursorCornerSprite(bool top, bool left)
        {
            Point corner = (top, left) switch
            {
                (true, true) => CursorTopLeft,
                (true, false) => CursorTopRight,
                (false, true) => CursorBottomLeft,
                _ => CursorBottomRight
            };

            return Still(new Rectangle(corner.X, corner.Y, CursorCornerSize, CursorCornerSize));
        }

        // A room on the pause screen's map, with a gap in its rim for each side that has a doorway
        public ISprite CreateDungeonRoomSprite(bool up, bool down, bool left, bool right)
        {
            int tile = (up ? 8 : 0) + (down ? 4 : 0) + (left ? 2 : 0) + (right ? 1 : 0);

            return Still(new Rectangle(RoomTilesLeft + tile * RoomTileSpacing, RoomTilesTop, RoomTileSize, RoomTileSize));
        }

        // The X in "X14"
        public ISprite CreateMultiplySignSprite() => Glyph(MultiplySignIndex);

        // digit is 0-9
        public ISprite CreateDigitSprite(int digit) => Glyph(FirstDigitIndex + digit);

        public ISprite CreateEmptyHeartSprite() => Glyph(EmptyHeartIndex);
        public ISprite CreateFullHeartSprite() => Glyph(FullHeartIndex);

        private Sprite Glyph(int index)
        {
            return Still(new Rectangle(GlyphsLeft + index * GlyphSpacing, GlyphsTop, GlyphSize, GlyphSize));
        }

        private Sprite Still(Rectangle frame)
        {
            return new Sprite(texture, new SpriteFrame(frame));
        }
    }
}
