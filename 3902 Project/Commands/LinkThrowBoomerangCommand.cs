using Animation;
using Interfaces;

namespace Commands
{
    internal class LinkThrowBoomerangCommand(Link link) : ICommand
    {

        public void Execute()
        {
            link.ThrowBoomerang();
        }

    }
}
