using Interfaces;
using Player;
using UI;

namespace Commands
{
    // Moves the highlight on the pause screen's items
    internal sealed class PauseMoveSelectionCommand(PauseScreen pauseScreen, Direction direction) : ICommand
    {

        public void Execute()
        {
            pauseScreen.MoveSelection(direction);
        }

    }
}
