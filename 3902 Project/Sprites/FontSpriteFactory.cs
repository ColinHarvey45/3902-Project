using System;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using CSE_3902_Project;

namespace Sprites
{
    // Builds a sprite for each character of the NES font, for the words on the pause, game over and win screens.
    // The letters on the sheet are blue; they are made white when the sheet is loaded, so they can be drawn in any colour
    internal sealed class FontSpriteFactory
    {
        // Every character the font has, in the order they are on the sheet: the blue rows at the top right,
        // 16 to a row, each 8 x 8 and 16 pixels apart
        public const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ,!'&.\"?-";

        private const int GlyphsLeft = 336;
        private const int GlyphsTop = 88;
        private const int GlyphSize = 8;
        private const int GlyphSpacing = 16;
        private const int GlyphsPerRow = 16;

        private Texture2D texture;

        public static FontSpriteFactory Instance { get; } = new FontSpriteFactory();

        private FontSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = Whitened(content.Load<Texture2D>(ContentNames.FontSheet));
        }

        // character is one of Characters
        public ISprite CreateGlyphSprite(char character)
        {
            int index = Characters.IndexOf(character, StringComparison.Ordinal);
            if (index < 0)
                throw new ArgumentException($"The font has no '{character}'", nameof(character));

            int x = GlyphsLeft + index % GlyphsPerRow * GlyphSpacing;
            int y = GlyphsTop + index / GlyphsPerRow * GlyphSpacing;

            return new Sprite(texture, new SpriteFrame(new Rectangle(x, y, GlyphSize, GlyphSize)));
        }

        // A copy of sheet with every pixel that isn't see-through made white
        private static Texture2D Whitened(Texture2D sheet)
        {
            Color[] pixels = new Color[sheet.Width * sheet.Height];
            sheet.GetData(pixels);

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].A > 0)
                    pixels[i] = Color.White;
            }

            Texture2D white = new(sheet.GraphicsDevice, sheet.Width, sheet.Height);
            white.SetData(pixels);
            return white;
        }
    }
}
