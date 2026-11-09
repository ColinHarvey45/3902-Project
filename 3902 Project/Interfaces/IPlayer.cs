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

        // His hearts: how many he has left, and the most he can have (he starts with 3 of 3).
        // A heart restores one; a heart container adds one more and fills them all
        public int Health { get; }
        public int MaxHealth { get; }
        public void Heal(int amount);
        public void IncreaseMaxHealth();

        // The rupees, keys and bombs he is carrying
        public PlayerInventory Inventory { get; }

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
