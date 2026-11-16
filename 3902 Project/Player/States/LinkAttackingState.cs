using Audio;
using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;

namespace Player.States
{
    // Link stands still and ignores input until his sword swing finishes. The swing itself is one of his
    // projectiles, which hits whatever is in front of him for as long as the swing lasts (see SwordSwing)
    internal sealed class LinkAttackingState : ILinkState
    {
        private const float AttackDuration = 0.2f; // seconds

        private readonly Link link;
        private readonly ISprite sprite;
        private float timer;

        public LinkAttackingState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateSwordSprite(link.Facing);

            link.SwingSword(AttackDuration);
            AudioManager.Instance.Play(Sound.SwordSlash);
        }

        public void Move(Direction direction) { }

        public void Stop() { }

        public void SwordAttack() { }

        public void UseItem(SecondaryItem item) { }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            sprite.Update(gameTime);

            if (timer >= AttackDuration)
            {
                link.State = new LinkStandingState(link);
                return;
            }

        }

        public void Draw(SpriteBatch spriteBatch, Color tint)
        {
            sprite.Draw(spriteBatch, link.Position, tint);
        }
    }
}