using GameStates;
using Interfaces;

namespace Commands
{
    // Stops the game where it is, with PAUSED over the room
    internal sealed class PauseCommand(GameStateMachine game) : ICommand
    {

        public void Execute()
        {
            game.Pause();
        }

    }
}
