using Interfaces;
using Player;

namespace Commands
{
    internal sealed class LinkFireArrowCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.UseItem(SecondaryItem.Arrow);
        }

    }
}
