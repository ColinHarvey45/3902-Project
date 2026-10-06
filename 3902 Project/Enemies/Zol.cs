using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Zol : Enemy
    {
        public Zol(Vector2 startPosition)
            : base(startPosition)
        {
            Sprite = EnemySpriteFactory.Instance.CreateZolSprite();
        }
    }
}
