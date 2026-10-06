using Animation;
using Interfaces;

namespace Commands
{
    internal class LinkPlaceBombCommand(Link link) : ICommand
    {

        public void Execute()
        {
            link.PlaceBomb();
        }

    }
}
