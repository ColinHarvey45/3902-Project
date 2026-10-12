using Microsoft.Xna.Framework;
using Sprites;

namespace Enemies
{
    internal sealed class Wallmaster : Enemy
    {
        public Wallmaster(Vector2 startPosition)
            : base(startPosition)
        {
            Sprite = EnemySpriteFactory.Instance.CreateWallmasterSprite();
        }
    }
}
