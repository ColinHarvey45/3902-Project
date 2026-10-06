using Interfaces;
using Player;

namespace Commands
{
    internal class LinkPlaceBombCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.UseItem(SecondaryItem.Bomb);
        }

    }
}
