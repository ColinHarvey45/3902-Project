using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Sprites
{
    // Builds the sprites for characters who don't fight, from the NPC sheet
    internal class NpcSpriteFactory
    {
        private static readonly Rectangle OldManFrame = new(1, 11, 16, 16);

        private Texture2D texture;

        public static NpcSpriteFactory Instance { get; } = new NpcSpriteFactory();

        private NpcSpriteFactory()
        {
        }

        public void LoadAllTextures(ContentManager content)
        {
            texture = content.Load<Texture2D>("TLOZNPCs-transparent");
        }

        public ISprite CreateOldManSprite()
        {
            return new Sprite(texture, new SpriteFrame(OldManFrame));
        }
    }
}
