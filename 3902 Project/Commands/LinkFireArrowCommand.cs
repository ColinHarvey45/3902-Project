using Animation;
using Interfaces;

namespace Commands
{
    internal class LinkFireArrowCommand(Link link) : ICommand
    {

        public void Execute()
        {
            link.FireArrow();
        }

    }
}
