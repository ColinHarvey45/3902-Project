using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Player.States
{
    // Link stands still and ignores input until his sword swing finishes
    internal sealed class LinkAttackingState : ILinkState
    {
        private readonly Link link;
        private readonly ISprite sprite;

        public LinkAttackingState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateSwordSprite(link.Facing);
        }

        public void Move(Direction direction) { }

        public void Stop() { }

        public void SwordAttack() { }

        public void UseItem(SecondaryItem item) { }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);

            if (sprite.IsFinished)
                link.State = new LinkStandingState(link);
        }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            sprite.Draw(spriteBatch, link.Position, tint);
        }
    }
}
