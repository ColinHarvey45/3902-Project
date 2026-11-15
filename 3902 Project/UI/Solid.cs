using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UI
{
    // Fills areas with one colour, for the parts of the screen that have no picture on the sprite sheet
    // (it stretches and tints a single white pixel). Load must be called once before anything is drawn
    internal static class Solid
    {

        private static Texture2D pixel;

        public static void Load(GraphicsDevice graphicsDevice)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        // area is in screen pixels
        public static void Fill(SpriteBatch spriteBatch, Rectangle area, Color color)
        {
            spriteBatch.Draw(pixel, area, color);
        }

    }
}
