using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Gel : Enemy
    {
        public Gel(Vector2 startPosition)
            : base(startPosition)
        {
            Sprite = EnemySpriteFactory.Instance.CreateGelSprite();
        }
    }
}
