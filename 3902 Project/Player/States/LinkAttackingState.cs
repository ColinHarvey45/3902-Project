using Colliders;
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
        private readonly float attackDuration = 0.2f;
        private float timer;

        // The sword's collider for this swing; RoomManager checks it against the enemies in the room
        public ICollider SwordCollider { get; }

        public LinkAttackingState(Link link)
        {
            this.link = link;
            sprite = LinkSpriteFactory.Instance.CreateSwordSprite(link.Facing);

            // Sized to the sword and offset from Link's position to the tile he is facing
            Point swordOffset = (link.Facing.ToVector() * (16 * Sprite.Scale)).ToPoint();
            SwordCollider = new BoxCollider(
                new Rectangle(0, 0, (int)(12 * Sprite.Scale), (int)(12 * Sprite.Scale)),
                swordOffset);
            SwordCollider.Update(link.Position);
        }

        public void Move(Direction direction) { }

        public void Stop() { }

        public void SwordAttack() { }

        public void UseItem(SecondaryItem item) { }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            sprite.Update(gameTime);

            if (timer >= attackDuration)
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