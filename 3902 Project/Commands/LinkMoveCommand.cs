using Interfaces;
using Player;

namespace Commands
{
    // Runs every frame its key is held down
    internal sealed class LinkMoveCommand(IPlayer player, Direction direction) : ICommand
    {

        public void Execute()
        {
            player.Move(direction);
        }

    }
}
