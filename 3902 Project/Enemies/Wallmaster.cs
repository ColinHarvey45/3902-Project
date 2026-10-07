using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal class Wallmaster : Enemy
    {
        public Wallmaster(Vector2 startPosition, Rectangle screenBounds)
            : base(startPosition, screenBounds)
        {
            Sprite = EnemySpriteFactory.Instance.CreateWallmasterSprite();
        }
    }
}
