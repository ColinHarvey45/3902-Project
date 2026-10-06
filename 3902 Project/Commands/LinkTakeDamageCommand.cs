using Animation;
using Interfaces;

namespace Commands
{
    internal class LinkTakeDamageCommand(Link link) : ICommand
    {

        public void Execute()
        {
            link.TakeDamage();
        }

    }
}
