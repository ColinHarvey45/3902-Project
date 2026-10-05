using Animation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    internal interface IState
    {

        public void ChangeDirection(Vector2 movement);
        public void TakeDamage(int damage);
        public void Attack(GameTime gameTime, Vector2 movement);

    }
}
