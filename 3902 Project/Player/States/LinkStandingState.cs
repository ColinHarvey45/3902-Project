using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Player.States
{
    internal class LinkStandingState : ILinkState
    {
        private readonly Link link;
        private readonly ISprite sprite;

        public LinkStandingState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateStandingSprite(link.Facing);
        }

        public void Move(Direction direction)
        {
            link.Facing = direction;
            link.State = new LinkWalkingState(link);
        }

        public void Stop() { }

        public void SwordAttack()
        {
            link.State = new LinkAttackingState(link);
        }

        public void UseItem(SecondaryItem item)
        {
            link.SpawnProjectile(item);
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            sprite.Draw(spriteBatch, link.Position, tint);
        }
    }
}
