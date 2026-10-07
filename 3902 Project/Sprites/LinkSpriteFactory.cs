using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Sprites
{
    // Builds every sprite used to draw Link himself
    internal sealed class LinkSpriteFactory
    {
        private const float WalkFrameTime = 0.15f;
        private const float SwordFrameTime = 0.06f;
        private const int BodyWidth = 16;

        private static readonly Rectangle[] WalkDownFrames = [new(1, 1, 16, 16), new(17, 1, 16, 16)];
        private static readonly Rectangle[] WalkUpFrames = [new(71, 1, 16, 16), new(87, 1, 16, 16)];
        private static readonly Rectangle[] WalkRightFrames = [new(33, 1, 16, 16), new(49, 1, 16, 16)];

        private static readonly Rectangle[] SwordDownFrames = [new(1, 37, 16, 15), new(18, 37, 16, 27), new(35, 37, 15, 23), new(53, 37, 13, 19)];
        private static readonly Rectangle[] SwordUpFrames = [new(1, 99, 16, 16), new(18, 87, 16, 28), new(37, 88, 12, 27), new(54, 96, 12, 19)];
        private static readonly Rectangle[] SwordRightFrames = [new(1, 68, 15, 15), new(18, 68, 27, 15), new(46, 68, 23, 15), new(70, 67, 19, 16)];

        // The upward swing frames are taller than Link, so they are raised to keep his feet in place
        private static readonly Vector2[] SwordUpOffsets = [Vector2.Zero, new(0, -12), new(0, -11), new(0, -3)];

        private Texture2D texture;

        public static LinkSpriteFactory Instance { get; } = new LinkSpriteFactory();

        private LinkSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZLink-transparent2");
        }

        public ISprite CreateStandingSprite(Direction facing)
        {
            return new Sprite(texture, WalkFrames(facing)[0]);
        }

        public ISprite CreateWalkingSprite(Direction facing)
        {
            return new Sprite(texture, WalkFrames(facing), WalkFrameTime, loops: true);
        }

        public ISprite CreateSwordSprite(Direction facing)
        {
            return new Sprite(texture, SwordFrames(facing), SwordFrameTime, loops: false);
        }

        private static SpriteFrame[] WalkFrames(Direction facing) => facing switch
        {
            Direction.Up => SpriteFrame.FromSources(WalkUpFrames),
            Direction.Down => SpriteFrame.FromSources(WalkDownFrames),
            Direction.Left => SpriteFrame.FromSources(WalkRightFrames, SpriteEffects.FlipHorizontally),
            _ => SpriteFrame.FromSources(WalkRightFrames)
        };

        private static SpriteFrame[] SwordFrames(Direction facing) => facing switch
        {
            Direction.Up => SwordUp(),
            Direction.Down => SpriteFrame.FromSources(SwordDownFrames),
            Direction.Left => SwordLeft(),
            _ => SpriteFrame.FromSources(SwordRightFrames)
        };

        private static SpriteFrame[] SwordUp()
        {
            SpriteFrame[] frames = new SpriteFrame[SwordUpFrames.Length];
            for (int i = 0; i < frames.Length; i++)
                frames[i] = new SpriteFrame(SwordUpFrames[i], SpriteEffects.None, SwordUpOffsets[i]);

            return frames;
        }

        // Mirrors the right-facing swing; each frame is shifted left by however much wider than Link
        // it is, so his body stays still while the sword sticks out to the left
        private static SpriteFrame[] SwordLeft()
        {
            SpriteFrame[] frames = new SpriteFrame[SwordRightFrames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                Vector2 offset = new(BodyWidth - SwordRightFrames[i].Width, 0);
                frames[i] = new SpriteFrame(SwordRightFrames[i], SpriteEffects.FlipHorizontally, offset);
            }

            return frames;
        }
    }
}
