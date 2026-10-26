using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Player;

namespace Interfaces
{
    // Everything the player character can be told to do; commands and collisions only talk to Link through this
    internal interface IPlayer
    {

        public void Move(Direction direction);
        public void SwordAttack();
        public void UseItem(SecondaryItem item);
        public void TakeDamage();

        public ICollider Collider { get; }

        // His sword's hitbox while he is swinging it, or null when he isn't, and how much it hurts an enemy
        public ICollider SwordCollider { get; }
        public int SwordDamage { get; }

        // Moves him straight away, e.g. back out of a wall he walked into
        public void Push(Vector2 offset);

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
}
