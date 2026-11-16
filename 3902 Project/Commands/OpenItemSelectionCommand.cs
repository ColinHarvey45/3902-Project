using GameStates;
using Interfaces;

namespace Commands
{
    // Stops the game and shows the inventory screen, where Link chooses the item for his B slot
    internal sealed class OpenItemSelectionCommand(GameStateMachine game) : ICommand
    {

        public void Execute()
        {
            game.OpenItemSelection();
        }

    }
}
