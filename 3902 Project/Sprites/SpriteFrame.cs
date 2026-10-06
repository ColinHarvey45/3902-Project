using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // One frame of a sprite: where it is on the sheet, whether it is flipped,
    // and how far (in sheet pixels) to shift it from the object's position
    internal readonly record struct SpriteFrame(Rectangle Source, SpriteEffects Effects = SpriteEffects.None, Vector2 Offset = default)
    {

        public static SpriteFrame[] FromSources(Rectangle[] sources, SpriteEffects effects = SpriteEffects.None)
        {
            SpriteFrame[] frames = new SpriteFrame[sources.Length];
            for (int i = 0; i < sources.Length; i++)
                frames[i] = new SpriteFrame(sources[i], effects);

            return frames;
        }

        // Alternates between the frame as drawn and its mirror image, used for walking enemies and fire
        public static SpriteFrame[] Flicker(Rectangle source)
        {
            return [new SpriteFrame(source), new SpriteFrame(source, SpriteEffects.FlipHorizontally)];
        }

    }
}
