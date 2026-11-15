using CSE_3902_Project;
using Interfaces;

namespace Commands
{
    // Pauses the game, or carries on if it is already paused
    internal sealed class TogglePauseCommand(Game1 game) : ICommand
    {

        public void Execute()
        {
            game.TogglePause();
        }

    }
}
