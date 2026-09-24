using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    internal interface IState
    {

        public void ChangeDirection();
        public void TakeDamage(int damage);
        public void Attack();

    }
}
