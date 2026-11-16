using GameStates;
using Interfaces;

namespace Commands
{
    // Starts the level again from the beginning, with every room as it was and Link at the entrance
    internal sealed class ResetCommand(GameStateMachine game) : ICommand
    {

        public void Execute()
        {
            game.Reset();
        }

    }
}
