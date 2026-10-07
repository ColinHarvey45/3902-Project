using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds the sprites for items Link can pick up
    internal sealed class ItemSpriteFactory
    {
        private const float FlashFrameTime = 0.15f;
        private const float FairyFlapFrameTime = 0.1f;

        // Items that flash alternate between two colours of the same picture
        private static readonly Rectangle[] HeartFrames = [new(0, 0, 7, 8), new(0, 8, 7, 8)];
        private static readonly Rectangle[] RupeeFrames = [new(72, 0, 8, 16), new(72, 16, 8, 16)];
        private static readonly Rectangle[] TriforcePieceFrames = [new(275, 3, 10, 10), new(275, 19, 10, 10)];
        private static readonly Rectangle[] FairyFrames = [new(40, 0, 8, 16), new(48, 0, 8, 16)];

        private static readonly Rectangle HeartContainerFrame = new(25, 1, 13, 13);
        private static readonly Rectangle ClockFrame = new(58, 0, 11, 16);
        private static readonly Rectangle MapFrame = new(88, 0, 8, 16);
        private static readonly Rectangle BoomerangFrame = new(129, 3, 5, 8);
        private static readonly Rectangle BombFrame = new(136, 0, 8, 16);
        private static readonly Rectangle BowFrame = new(144, 0, 8, 16);
        private static readonly Rectangle KeyFrame = new(240, 0, 8, 16);
        private static readonly Rectangle CompassFrame = new(258, 1, 11, 12);

        private Texture2D texture;

        public static ItemSpriteFactory Instance { get; } = new ItemSpriteFactory();

        private ItemSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZItems");
        }

        public ISprite CreateHeartSprite() => Flashing(HeartFrames);
        public ISprite CreateRupeeSprite() => Flashing(RupeeFrames);
        public ISprite CreateTriforcePieceSprite() => Flashing(TriforcePieceFrames);

        public ISprite CreateFairySprite()
        {
            return new Sprite(texture, SpriteFrame.FromSources(FairyFrames), FairyFlapFrameTime, loops: true);
        }

        public ISprite CreateHeartContainerSprite() => Still(HeartContainerFrame);
        public ISprite CreateClockSprite() => Still(ClockFrame);
        public ISprite CreateMapSprite() => Still(MapFrame);
        public ISprite CreateBoomerangSprite() => Still(BoomerangFrame);
        public ISprite CreateBombSprite() => Still(BombFrame);
        public ISprite CreateBowSprite() => Still(BowFrame);
        public ISprite CreateKeySprite() => Still(KeyFrame);
        public ISprite CreateCompassSprite() => Still(CompassFrame);

        private Sprite Flashing(Rectangle[] frames)
        {
            return new Sprite(texture, SpriteFrame.FromSources(frames), FlashFrameTime, loops: true);
        }

        private Sprite Still(Rectangle frame)
        {
            return new Sprite(texture, new SpriteFrame(frame));
        }
    }
}
