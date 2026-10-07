using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Player.States
{
    internal sealed class LinkWalkingState : ILinkState
    {
        private readonly Link link;
        private readonly ISprite sprite;

        public LinkWalkingState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateWalkingSprite(link.Facing);
        }

        public void Move(Direction direction)
        {
            if (direction == link.Facing) return;

            // Turning restarts the walk animation facing the new way
            link.Facing = direction;
            link.State = new LinkWalkingState(link);
        }

        public void Stop()
        {
            link.State = new LinkStandingState(link);
        }

        public void SwordAttack()
        {
            link.State = new LinkAttackingState(link);
        }

        public void UseItem(SecondaryItem item)
        {
            link.SpawnProjectile(item);
        }

        public void Update(GameTime gameTime)
        {
            link.StepForward();
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            sprite.Draw(spriteBatch, link.Position, tint);
        }
    }
}
