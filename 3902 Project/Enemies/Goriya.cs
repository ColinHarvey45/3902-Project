using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Goriya : Enemy
    {

        private const float GoriyaWalkSpeed = 0.15f;
        private static readonly Point GoriyaSourceRect = new(0, 15);
        private static readonly Point GoriyaSpriteSize = new(9, 16);
        private const int GoriyaFrames = 2;

        public Goriya(Texture2D texture, SpriteBatch spriteBatch, Vector2 startPosition)
            : base(texture, spriteBatch, startPosition, GoriyaSourceRect, GoriyaFrames, GoriyaWalkSpeed)
        {
            this.enemySprite.SetSpriteSize(GoriyaSpriteSize);
        }

    }
}
