using Animation;
using Interfaces;

namespace Commands
{
    internal class LinkSwordAttackCommand(Link link) : ICommand
    {

        public void Execute()
        {
            link.SwordAttack();
        }

    }
}
