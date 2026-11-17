using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Player.States
{
    // Caught by a Wallmaster: as in the NES, he can't walk, swing his sword or use items until it lets go of him.
    // He stays facing the way he was, and is moved only by whatever is holding him
    internal sealed class LinkHeldState : ILinkState
    {
        private readonly Link link;
        private readonly ISprite sprite;

        public LinkHeldState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateStandingSprite(link.Facing);
        }

        public void Move(Direction direction) { }

        public void Stop() { }

        public void SwordAttack() { }

        public void UseItem(SecondaryItem item) { }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            sprite.Draw(spriteBatch, link.Position, tint);
        }
    }
}
