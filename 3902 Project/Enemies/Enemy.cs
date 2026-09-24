using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemies
{
    internal class Enemy
    {

        protected int health;
        protected Vector2 direction;
        protected Point sourceRect;
        private Vector2 startLocation;
        protected SpriteEffects spriteEffect;
        private SpriteBatch spriteBatch;
        private Texture2D spriteSheet;


    }
}
