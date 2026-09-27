using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Zol : Enemy
    {

        private const float ZolWalkSpeed = 0.15f;
        private static readonly Point ZolSourceRect = new(77, 10);
        private static readonly Point ZolSpriteSize = new(16, 16);
        private const int ZolFrames = 2;

        public Zol(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, ZolSourceRect, ZolFrames, ZolWalkSpeed)
        {
            this.enemySpriteAnim.SetSpriteSize(ZolSpriteSize);
        }

    }
}
