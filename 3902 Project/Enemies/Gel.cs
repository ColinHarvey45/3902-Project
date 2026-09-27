using Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Gel : Enemy
    {
        private const float GelWalkSpeed = 0.15f;
        private static readonly Point GelSourceRect = new(0, 15);
        private static readonly Point GelSpriteSize = new(9, 16);
        private const int GelFrames = 2;
        

        public Gel(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, GelSourceRect, GelFrames, GelWalkSpeed)
        {
            this.enemySprite.SetSpriteSize(GelSpriteSize);
        }


    }
}

