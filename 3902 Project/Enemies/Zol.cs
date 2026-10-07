using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    // Like a Gel but bigger and slower: creeps about a tile at a time with longer pauses
    internal sealed class Zol : HoppingEnemy
    {
        private const float ZolHopSpeed = 1f; // pixels per frame
        private const float ZolHopTime = 1f;
        private const float ZolMaxRestTime = 1.5f;

        public Zol(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds, ZolHopSpeed, ZolHopTime, ZolMaxRestTime)
        {
            Sprite = EnemySpriteFactory.Instance.CreateZolSprite();
        }
    }
}
