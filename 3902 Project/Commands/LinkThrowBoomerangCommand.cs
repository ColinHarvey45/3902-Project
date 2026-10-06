using Interfaces;
using Player;

namespace Commands
{
    internal class LinkThrowBoomerangCommand(IPlayer player) : ICommand
    {

        public void Execute()
        {
            player.UseItem(SecondaryItem.Boomerang);
        }

    }
}
