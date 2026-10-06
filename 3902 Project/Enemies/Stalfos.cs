using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Stalfos : Enemy
    {
        public Stalfos(Vector2 startPosition)
            : base(startPosition)
        {
            Sprite = EnemySpriteFactory.Instance.CreateStalfosSprite();
        }
    }
}
