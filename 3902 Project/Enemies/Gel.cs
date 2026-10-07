using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    // Darts about a tile at a time with short pauses in between
    internal class Gel : HoppingEnemy
    {
        private const float GelHopSpeed = 2f; // pixels per frame
        private const float GelHopTime = 0.5f;
        private const float GelMaxRestTime = 1f;

        public Gel(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds, GelHopSpeed, GelHopTime, GelMaxRestTime)
        {
            Sprite = EnemySpriteFactory.Instance.CreateGelSprite();
        }
    }
}
