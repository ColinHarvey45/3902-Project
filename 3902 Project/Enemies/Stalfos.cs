using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Stalfos : Enemy
    {
        public Stalfos(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds)
        {
            Sprite = EnemySpriteFactory.Instance.CreateStalfosSprite();
        }
    }
}
