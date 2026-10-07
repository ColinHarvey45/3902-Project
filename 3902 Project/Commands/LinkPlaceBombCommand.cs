using Interfaces;
using Player;

namespace Commands
{
    internal sealed class LinkPlaceBombCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.UseItem(SecondaryItem.Bomb);
        }

    }
}
