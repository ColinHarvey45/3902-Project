using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Interfaces
{
    internal interface ICollider
    {

        public Rectangle Overlap(Rectangle boxA, Rectangle boxB);
        public void Update(Vector2 position);
        public Rectangle GetCollider();

    }
}
