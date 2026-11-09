using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds the sprites for the HUD: its frame and labels, the digits and hearts it counts with,
    // and one room on the dungeon map
    internal sealed class HudSpriteFactory
    {
        // The whole HUD frame, 256 x 56: its rupee, key and bomb pictures, the B and A boxes and "-LIFE-".
        // It also has stand-ins (counters, item pictures, hearts) that the HUD covers up and draws over
        private static readonly Rectangle FrameSource = new(258, 11, 256, 56);

        // "LEVEL-", six letters, to be followed by the level's number
        private static readonly Rectangle LevelLabelSource = new(584, 1, 48, 8);

        // One room on the dungeon map: a blue block 7 x 3 (the map leaves a 1 pixel gap around each)
        private static readonly Rectangle MapRoomSource = new(663, 108, 7, 3);

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
