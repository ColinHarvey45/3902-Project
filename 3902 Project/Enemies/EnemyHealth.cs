using Microsoft.Xna.Framework;

namespace Enemies
{
    // An enemy's hit points and its hurt flash. After a hit it flashes and can't be hurt again for
    // a moment; once its health reaches 0 it is dead. Shared by every enemy that can be killed
    internal sealed class EnemyHealth
    {
        public const int DefaultHealth = 1;

        private const float FlashInterval = 0.1f;
        private const float HurtDuration = 0.8f;

        private static readonly Color[] FlashColors =
        {
            Color.White,   // original sprite color
            Color.Blue,
            Color.White,
            Color.Teal     // dungeon teal
        };

        private int flashIndex;
        private float flashTimer;
        private float hurtTimer;

        public int Current { get; private set; }
        public bool IsDamaged { get; private set; }
        public bool IsDead => Current <= 0;

        // What to tint the sprite this frame
        public Color Tint => IsDamaged ? FlashColors[flashIndex] : Color.White;

        public EnemyHealth(int health = DefaultHealth)
        {
            Current = health;
        }

        // Ignored while already hurt (it flashes and can't be hit again for a moment) or dead
        public void TakeDamage(int damage)
        {
            if (IsDead || IsDamaged)
                return;

            Current -= damage;

            if (IsDead)
                return;

            IsDamaged = true;
            flashIndex = 0;
            flashTimer = 0f;
            hurtTimer = 0f;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsDamaged)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            flashTimer += deltaTime;
            hurtTimer += deltaTime;

            if (flashTimer >= FlashInterval)
            {
                flashTimer -= FlashInterval;
                flashIndex = (flashIndex + 1) % FlashColors.Length;
            }

            if (hurtTimer >= HurtDuration)
            {
                IsDamaged = false;
                flashIndex = 0;
                flashTimer = 0f;
                hurtTimer = 0f;
            }
        }
    }
}
