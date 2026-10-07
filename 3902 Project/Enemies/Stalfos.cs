using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal sealed class Stalfos : Enemy
    {
        public Stalfos(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds)
        {
            Sprite = EnemySpriteFactory.Instance.CreateStalfosSprite();
        }
    }
}
