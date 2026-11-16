using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rooms;
using Sprites;

namespace UI
{
    // Writes words on screen in the NES font: capital letters, digits and a little punctuation, every character
    // the same width. Lower-case letters are written as capitals, and spaces are left blank
    internal sealed class TextRenderer
    {
        // Each character is 8 x 8 on the sheet
        public const int CharacterSize = 8 * (int)Sprite.Scale;

        private readonly Dictionary<char, ISprite> glyphs = [];

        public TextRenderer()
        {
            foreach (char character in FontSpriteFactory.Characters)
                glyphs[character] = FontSpriteFactory.Instance.CreateGlyphSprite(character);
        }

        public static int Width(string text)
        {
            return text.Length * CharacterSize;
        }

        // at is the top-left corner of the first character
        public void Draw(SpriteBatch spriteBatch, string text, Vector2 at, Color color)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    continue;

                char character = char.ToUpperInvariant(text[i]);
                if (!glyphs.TryGetValue(character, out ISprite glyph))
                    throw new ArgumentException($"The font has no '{text[i]}', in \"{text}\"", nameof(text));

                glyph.Draw(spriteBatch, at + new Vector2(i * CharacterSize, 0), color);
            }
        }

        // Centred across the room, with the top of the characters at y
        public void DrawCentred(SpriteBatch spriteBatch, string text, float y, Color color)
        {
            float x = RoomLayout.Bounds.Center.X - Width(text) / 2f;
            Draw(spriteBatch, text, new Vector2(x, y), color);
        }

        // Lines of text in a black box in the middle of the room, each line centred, with a blank line between
        // them and a character's width of black all round
        public void DrawMessage(SpriteBatch spriteBatch, IReadOnlyList<string> lines, Color color)
        {
            const int LineHeight = 2 * CharacterSize;
            int width = lines.Max(Width) + 2 * CharacterSize;
            int height = (2 * lines.Count + 1) * CharacterSize;
            Point middle = RoomLayout.Bounds.Center;
            Rectangle box = new(middle.X - width / 2, middle.Y - height / 2, width, height);

            Solid.Fill(spriteBatch, box, Color.Black);

            for (int i = 0; i < lines.Count; i++)
                DrawCentred(spriteBatch, lines[i], box.Top + CharacterSize + i * LineHeight, color);
        }
    }
}
