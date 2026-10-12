using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal sealed class Stalfos : Enemy
    {
        public Stalfos(Vector2 startPosition)
            : base(startPosition)
        {
            Sprite = EnemySpriteFactory.Instance.CreateStalfosSprite();
        }
    }
}
