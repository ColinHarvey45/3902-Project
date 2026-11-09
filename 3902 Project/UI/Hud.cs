using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;
using System;

namespace UI
{
    // The bar across the top of the screen: the dungeon's level and map (with the room Link is in), how many
    // rupees, keys and bombs he has, what is in his B and A slots, and his hearts.
    // Everything below is laid out in sheet pixels, like the HUD picture it is drawn on, and scaled up when drawn
    internal sealed class Hud
    {
        private const int Scale = (int)Sprite.Scale;

        // The frame is 256 x 56 sheet pixels: as wide as a dungeon room, and the room goes underneath
        public const int Height = 56 * Scale;

        private const int GlyphSize = 8;

        // The counters are an X and two digits; the stand-ins in the frame have room for no more
        private const int MaxCountShown = 99;

        // Hearts fill two rows of 8, the bottom row first
        private const int HeartsPerRow = 8;
        private const int MaxHearts = 2 * HeartsPerRow;

        // Where things go on the frame
        private static readonly Point LevelLabelAt = new(16, 8);
        private static readonly Point LevelNumberAt = new(LevelLabelAt.X + 6 * GlyphSize, LevelLabelAt.Y);
        private static readonly Point MapAt = new(16, 16);
        private static readonly Point RupeeCounterAt = new(96, 16);
        private static readonly Point KeyCounterAt = new(96, 32);
        private static readonly Point BombCounterAt = new(96, 40);
        private static readonly Point BItemAt = new(128, 24);
        private static readonly Point AItemAt = new(152, 24);
        private static readonly Point HeartsAt = new(176, 32);

        // The frame's stand-ins for the three counters, the two item pictures and the hearts, which are
        // blacked out before the real ones are drawn
        private static readonly Rectangle[] StandIns =
        [
            new(96, 16, 3 * GlyphSize, GlyphSize),
            new(96, 32, 3 * GlyphSize, GlyphSize),
            new(96, 40, 3 * GlyphSize, GlyphSize),
            new(128, 24, 8, 16),
            new(152, 24, 8, 16),
            new(176, 32, HeartsPerRow * GlyphSize, 2 * GlyphSize)
        ];

        // The map is 8 x 8 rooms; each takes 8 x 4 pixels, and the marker for the room Link is in is a small block inside it
        private const int MapRoomWidth = 8;
        private const int MapRoomHeight = 4;
        private static readonly Rectangle MapMarker = new(2, 0, 3, 3);
        private static readonly Color MapMarkerColor = new(252, 252, 252);

        // A single white pixel, stretched and tinted to fill in areas and draw the map marker
        private static Texture2D pixel;

        private readonly IPlayer link;
        private readonly RoomManager rooms;
        private readonly int level;

        private readonly ISprite frame = HudSpriteFactory.Instance.CreateFrameSprite();
        private readonly ISprite levelLabel = HudSpriteFactory.Instance.CreateLevelLabelSprite();
        private readonly ISprite multiplySign = HudSpriteFactory.Instance.CreateMultiplySignSprite();
        private readonly ISprite[] digits = new ISprite[10];
        private readonly ISprite fullHeart = HudSpriteFactory.Instance.CreateFullHeartSprite();
        private readonly ISprite emptyHeart = HudSpriteFactory.Instance.CreateEmptyHeartSprite();
        private readonly ISprite mapRoom = HudSpriteFactory.Instance.CreateMapRoomSprite();

        // Link's B slot holds a bomb, and his A slot his sword
        private readonly ISprite bItem = ItemSpriteFactory.Instance.CreateBombSprite();
        private readonly ISprite aItem = ItemSpriteFactory.Instance.CreateSwordSprite();

        // level is the dungeon's number, shown after "LEVEL-"
        public Hud(IPlayer link, RoomManager rooms, int level, GraphicsDevice graphicsDevice)
        {
            this.link = link;
            this.rooms = rooms;
            this.level = level;

            for (int digit = 0; digit < digits.Length; digit++)
                digits[digit] = HudSpriteFactory.Instance.CreateDigitSprite(digit);

            if (pixel == null)
            {
                pixel = new Texture2D(graphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            frame.Draw(spriteBatch, Vector2.Zero);

            foreach (Rectangle standIn in StandIns)
                Fill(spriteBatch, standIn, Color.Black);

            levelLabel.Draw(spriteBatch, Screen(LevelLabelAt));
            DrawNumber(spriteBatch, LevelNumberAt, level);

            DrawCount(spriteBatch, RupeeCounterAt, link.Inventory.Rupees);
            DrawCount(spriteBatch, KeyCounterAt, link.Inventory.Keys);
            DrawCount(spriteBatch, BombCounterAt, link.Inventory.Bombs);

            bItem.Draw(spriteBatch, Screen(BItemAt));
            // The sword is 7 pixels wide in a slot 8 wide, so it is shifted half a pixel to sit in the middle
            aItem.Draw(spriteBatch, Screen(AItemAt) + new Vector2(Scale / 2f, 0));

            DrawHearts(spriteBatch);
            DrawMap(spriteBatch);
        }

        // "X14": an X, then the number
        private void DrawCount(SpriteBatch spriteBatch, Point at, int count)
        {
            multiplySign.Draw(spriteBatch, Screen(at));
            DrawNumber(spriteBatch, new Point(at.X + GlyphSize, at.Y), Math.Min(count, MaxCountShown));
        }

        private void DrawNumber(SpriteBatch spriteBatch, Point at, int number)
        {
            string text = number.ToString();
            for (int i = 0; i < text.Length; i++)
                digits[text[i] - '0'].Draw(spriteBatch, Screen(new Point(at.X + i * GlyphSize, at.Y)));
        }

        // One heart for each Link can hold: red for the ones he has left, an outline for the ones he has lost.
        // The first 8 fill the bottom row from the left; any past that go on the row above
        private void DrawHearts(SpriteBatch spriteBatch)
        {
            int hearts = Math.Min(link.MaxHealth, MaxHearts);

            for (int i = 0; i < hearts; i++)
            {
                int row = i < HeartsPerRow ? 1 : 0;
                Point at = new(HeartsAt.X + i % HeartsPerRow * GlyphSize, HeartsAt.Y + row * GlyphSize);

                ISprite heart = i < link.Health ? fullHeart : emptyHeart;
                heart.Draw(spriteBatch, Screen(at));
            }
        }

        // A block for every room that has a place on the map, and a marker on the one Link is in
        private void DrawMap(SpriteBatch spriteBatch)
        {
            foreach (Room room in rooms.AllRooms)
            {
                if (room.MapPosition is Point cell)
                    mapRoom.Draw(spriteBatch, Screen(MapRoomAt(cell)));
            }

            if (rooms.CurrentRoom.MapPosition is Point here)
            {
                Point origin = MapRoomAt(here);
                Fill(spriteBatch, new Rectangle(origin.X + MapMarker.X, origin.Y + MapMarker.Y, MapMarker.Width, MapMarker.Height), MapMarkerColor);
            }
        }

        private static Point MapRoomAt(Point cell)
        {
            return new Point(MapAt.X + cell.X * MapRoomWidth, MapAt.Y + cell.Y * MapRoomHeight);
        }

        // area is in sheet pixels
        private static void Fill(SpriteBatch spriteBatch, Rectangle area, Color color)
        {
            spriteBatch.Draw(pixel, new Rectangle(area.X * Scale, area.Y * Scale, area.Width * Scale, area.Height * Scale), color);
        }

        // Where a point on the frame, in sheet pixels, is on screen
        private static Vector2 Screen(Point sheetPixels)
        {
            return new Vector2(sheetPixels.X * Scale, sheetPixels.Y * Scale);
        }
    }
}
