using Microsoft.Xna.Framework;
using Sprites;
using Enemies;

namespace Enemies.Hoppers
{
    // Darts about a tile at a time with short pauses in between
    internal sealed class Gel : HoppingEnemy
    {
        private const float GelHopSpeed = 2f; // pixels per frame
        private const float GelHopTime = 0.5f;
        private const float GelMaxRestTime = 1f;
        private const int GelColliderWidth = (int)(9 * Sprites.Sprite.Scale);
        private const int GelColliderHeight = (int)(12 * Sprites.Sprite.Scale);
        private static readonly Point GelColliderOffset = new(0, (int)(2 * Sprites.Sprite.Scale));

        // fromSplit is true for the two Gels a Zol splits into, which can't be hurt for a moment (see Zol)
        public Gel(Vector2 startPosition, bool fromSplit = false)
            : base(startPosition, GelHopSpeed, GelHopTime, GelMaxRestTime,
                GelColliderWidth, GelColliderHeight, GelColliderOffset)
        {
            Sprite = EnemySpriteFactory.Instance.CreateGelSprite();

            if (fromSplit)
                Health.StartGracePeriod();
        }

        protected override bool KilledByBoomerang => true;
    }
}
