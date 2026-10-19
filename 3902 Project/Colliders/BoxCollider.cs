using Interfaces;
using Microsoft.Xna.Framework;
using Projectiles;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colliders
{
    internal class BoxCollider(Rectangle collider) : ICollider
    {
        private Rectangle overlap;
        private Rectangle collider = collider;

        public Rectangle Overlap(Rectangle boxA, Rectangle boxB) 
        {
            Rectangle.Intersect(ref boxA, ref boxB, out overlap);
            return overlap;
        }

        public Rectangle GetCollider()
        {
            return collider;
        }

        public void Update(Vector2 position)
        {
            this.collider.X = (int)position.X;
            this.collider.Y = (int)position.Y;
        }

    }
}

